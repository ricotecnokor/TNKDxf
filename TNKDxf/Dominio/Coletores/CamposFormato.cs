using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Coletores
{
    public class CamposFormato : AbsColeta //IColetaCampos
    {
       
        private List<string[]> _campos = new List<string[]>();
        protected Dictionary<string, string> _objectIdsToDelete = new Dictionary<string, string>();

        protected IColetaErros _coletaErros;
        public CamposFormato(Formato formato, IColetaErros coletaErros) : base(formato)
        {
          
            _coletaErros = coletaErros;
        }

        public string ObterCampo(string campo)
        {
            return _campos.Find(x => x[0] == campo)[1];
        }

        public void Coletar()//CampoExtraido campos)
        {
            foreach (var entity in DxfSingleton.DxfDocument.Entities.All)
            {
                if (entity.Type == EntityType.Insert)
                {

                    incluiBlocodoFormatoApagar((Insert)entity);

                    buscarEntidades((Insert)entity);//, campos);
                }
                else
                {
                    coletarLinhasTextos(entity); //, campos);
                }
            }
        }

        private void incluiBlocodoFormatoApagar(Insert bloco) //CampoExtraido campos)
        {
            try
            {
                var pontoMin = bloco.Block.ExtMin();

                if (pontoMin == null)
                {
                    return;
                }

                Ponto2d pontoInferiorEsquerdo = Ponto2d.CriarSemEscala(pontoMin.X, pontoMin.Y);

                var pontoMax = bloco.Block.ExtMax();
                Ponto2d pontoSuperiorDireito = Ponto2d.CriarSemEscala(pontoMax.X, pontoMax.Y);

                var limite = _formato.LimiteFormato;

                if (limite.PontoInicial.IgualComTolerancia(pontoInferiorEsquerdo, 2.0) && limite.PontoFinal.IgualComTolerancia(pontoSuperiorDireito, 2.0))
                {
                    _objectIdsToDelete.Add(bloco.Handle, "BLOCO");
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string[] ObterCampo(Texto texto)
        {
            var possivelValorCampo = texto.Valor;
            foreach (var c in _formato.CamposFormato)
            {
                var cantoInferiorEsquerdoRef = c.Janela.CantoInferiorEsquerdo;
                var cantoSuperiorDireitoRef = c.Janela.CantoSuperiorDireito;

                var pontoInsercaoRef_X = c.Texto.PontoInsercao.X;
                var pontoInsercaoRef_Y = c.Texto.PontoInsercao.Y;
                var pontoTexto_X = texto.PontoInsercao.X;
                var pontoTexto_Y = texto.PontoInsercao.Y;

                var nomeCampo = c.Texto.Valor;


                if (cantoInferiorEsquerdoRef.MenorQue(texto.PontoInsercao))
                {
                    if (cantoSuperiorDireitoRef.MaiorQue(texto.PontoInsercao))
                    {
                        var deu = texto.Valor;
                        if (c.Texto.PontoInsercao.IgualComTolerancia(texto.PontoInsercao, 3.0, _formato.FatorEscala))
                        {

                            if (textoErrado(texto.Atributo, texto.Valor))
                            {
                                _coletaErros.InserirErro(new ErroColetado(texto.PontoInsercao, texto.Atributo, texto.Valor));
                            }

                            return new string[] { nomeCampo, possivelValorCampo };



                        }
                    }
                }


            }

            return null;
        }

        private bool textoErrado(string atributo, string valor)
        {
            return valor == "DF-SD-1000KN-M-88148-M-505408" ? true : false;
        }

        private void buscarEntidades(Insert insert) //CampoExtraido campos)
        {
            var block = insert.Block;

            foreach (var entity in block.Entities)
            {
                if (entity.Type == EntityType.Insert)
                {
                    buscarEntidades((Insert)entity); //campos);
                }
                else
                {
                    coletarLinhasTextos(entity);   //, campos);
                }
            }


        }

        private void coletarLinhasTextos(EntityObject entity)//, CampoExtraido campos)
        {
            entity.GetType().ToString();

            if (entity.Type == EntityType.Text)
            {
                var texto = (Text)entity;
                var textoCriado = texto.CriarTexto(1.0);
                addTexto(texto.CriarTexto(1.0));

            }
        }

        public bool addTexto(Texto texto)
        {
            var valores = ObterCampo(texto);

            if (valores != null)
            {
                _campos.Add(valores);
                return true;
            }
            return false;
        }

        public override void ApagarSelecao()
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

        public List<string[]> ObterCampos()
        {
            return _campos;
        }
    }
}
