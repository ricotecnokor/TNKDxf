using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Coletores
{
    public class ColetaRevisoes 
    {
        IColetaErros _coletaErros;
        protected List<Texto> _textos = new List<Texto>();
        private List<Dictionary<string, string>> _revisoes = new List<Dictionary<string, string>>();
        Formato _formato;
        protected Dictionary<string, string> _objectIdsToDelete = new Dictionary<string, string>();
        public ColetaRevisoes(Formato formato, IColetaErros coletaErros) 
        {
            _formato = formato;
            _coletaErros = coletaErros;
        }

        public int QtdConjuntos => throw new NotImplementedException();

        public void Coletar()
        {
            foreach (var entity in DxfSingleton.DxfDocument.Entities.All)
            {
                if (entity.Type == EntityType.Insert)
                {
                    incluiBlocodoFormatoApagar((Insert)entity);

                    buscarEntidades((Insert)entity);
                }
                else
                {
                    coletarLinhasTextos(entity); 
                }
            }
        }



        private void buscarEntidades(Insert insert) 
        {
            var block = insert.Block;

            foreach (var entity in block.Entities)
            {
                if (entity.Type == EntityType.Insert)
                {
                    buscarEntidades((Insert)entity); 
                }
                else
                {
                    coletarLinhasTextos(entity); 
                }
            }


        }

        private void incluiBlocodoFormatoApagar(Insert insert)
        {
            try
            {
                var pontoInferiorEsquerdo = insert.Block.ExtMin();

                if (pontoInferiorEsquerdo is null)
                    return;

                var pontoSuperiorDireito = insert.Block.ExtMax();

                if (pontoInferiorEsquerdo is null || pontoSuperiorDireito is null)
                    return;

                var limite = _formato.LimiteFormato;

                if (limite.PontoInicial.IgualComTolerancia(pontoInferiorEsquerdo, 2.0) && limite.PontoFinal.IgualComTolerancia(pontoSuperiorDireito, 2.0))
                {
                    _objectIdsToDelete.Add(insert.Handle, "BLOCO");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void coletarLinhasTextos(EntityObject entity)
        {
            if (entity.Type == EntityType.Text)
            {
                var texto = (Text)entity;
                var textoColetado = texto.CriarTexto(1.0);



                addTexto(textoColetado);

            }
        }

        private bool addTexto(Texto texto)
        {
            if (texto.PontoInsercao.MaiorOuIgual(_formato.LimiteTabelaRevisao.PontoInicial)
                && texto.PontoInsercao.MenorOuIgual(_formato.LimiteTabelaRevisao.PontoFinal))
            {
                _textos.Add(texto);
                return true;
            }
            return false;
        }

        public List<Dictionary<string, string>> ObterRevisoes()
        {
            return _revisoes;
        }

        public void CriarRevisoes()
        {
            if (_textos is null || _textos.Count == 0)
                return;

            var listaYs = _textos.Select(l => Math.Round(l.PontoInsercao.Y, 0)).Distinct().ToList();

            foreach (var y in listaYs)
            {
                var dic = new Dictionary<string, string>();

                var linha = _textos.Where(l => l.PontoInsercao.Y.IgualComTolerancia(y, 1.0, _formato.FatorEscala)).ToList();

                foreach (var topico in _formato.LinhaRevisao)
                {
                    var texto = linha.FirstOrDefault(t =>
                            t.PontoInsercao.X >= topico.Janela.CantoInferiorEsquerdo.X * _formato.FatorEscala
                            && t.PontoInsercao.X <= topico.Janela.CantoSuperiorDireito.X * _formato.FatorEscala);

                    if (texto != null)
                    {
                        if (textoErrado(texto.Atributo, texto.Valor))
                        {
                            _coletaErros.InserirErro(new ErroColetado(texto.PontoInsercao, texto.Atributo, texto.Valor));
                        }

                        dic.Add(topico.Texto.Valor, texto.Valor);
                    }
                }

                _revisoes.Add(dic);

            }
        }

        private bool textoErrado(string atributo, string valor)
        {
            return false;
        }

        public void ApagarSelecao()
        {
            if (_objectIdsToDelete.Count() > 0)
            {
                var blocos = _objectIdsToDelete.Where(o => o.Value.Equals("BLOCO"));
                if (blocos.Count() > 0)
                {
                    foreach (var bloco in blocos)
                    {
                        var blocoToDelete = DxfSingleton.DxfDocument.Entities.Inserts.FirstOrDefault(i => i.Handle == bloco.Key);
                        DxfSingleton.DxfDocument.Entities.Remove(blocoToDelete);
                    }
                }
                else
                {
                    var linhas = _objectIdsToDelete.Where(o => o.Value.Equals("LINHA"));
                    if (linhas.Count() > 0)
                    {
                        foreach (var linha in linhas)
                        {
                            var linhaToDelete = DxfSingleton.DxfDocument.Entities.Lines.FirstOrDefault(i => i.Handle == linha.Key);
                            DxfSingleton.DxfDocument.Entities.Remove(linhaToDelete);
                        }
                    }

                    var textos = _objectIdsToDelete.Where(o => o.Value.Equals("TEXTO"));
                    if (textos.Count() > 0)
                    {
                        foreach (var text in textos)
                        {
                            var textToDelete = DxfSingleton.DxfDocument.Entities.Texts.FirstOrDefault(i => i.Handle == text.Key);
                            DxfSingleton.DxfDocument.Entities.Remove(textToDelete);
                        }
                    }
                }


            }
        }
    }
}
