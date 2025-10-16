using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.ObjetosValor;
using Line = netDxf.Entities.Line;

namespace TNKDxf.Dominio.Coletores
{
    public class ColetaLmTNK : AbsColeta
    {
        protected List<Linha> _linhasHorizontais = new List<Linha>();
        protected List<Linha> _linhasVerticais = new List<Linha>();
        protected List<Texto> _textos = new List<Texto>();
        public ColetaLmTNK(Formato formato) : base(formato)
        {
        }

        public void Coletar()
        {
            foreach (var entity in DxfSingleton.DxfDocument.Entities.All)
            {
                if (entity.Type == EntityType.Insert)
                {
                    incluiBlocodaLMParaApagar((Insert)entity);

                    buscarEntidades((Insert)entity);
                }
                else
                {
                    coletarLinhasTextos(entity);
                }
            }
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

        public List<Texto> ObterTodosTextos()
        {
            return _textos;
        }

        public List<Texto> ObterTextos()
        {
            return _textos.Where(t =>
            t.PontoInsercao.MaiorOuIgual(Formato.CantoInferiorEsquerdoPrimeiraLinhaLM)
            && t.PontoInsercao.MenorOuIgual(Formato.CantoSuperiorDireitoPrimeiraLinhaLM)).ToList();
        }

        public List<Texto> ObterTextosPorYBase(double yBase)
        {
            return _textos.Where(t =>
                            t.PontoInsercao.MaiorOuIgual(Ponto2d.CriarSemEscala(Formato.CantoInferiorEsquerdoPrimeiraLinhaLM.X, yBase))
                            && t.PontoInsercao.MenorOuIgual(Ponto2d.CriarSemEscala(Formato.CantoSuperiorDireitoPrimeiraLinhaLM.X, yBase + Formato.EspacoLinhasLM)))
                            .ToList();
        }

    
        private void incluiBlocodaLMParaApagar(Insert bloco)
        {
            try
            {
                var margem = 10.0 * _formato.FatorEscala;

                var pontoMax = bloco.Block.ExtMax();
                if (pontoMax != null)
                {
                    var xFormatoLocal = _formato.LimiteFormato.PontoFinal.X - margem;
                    var yFormatoLocal = _formato.LimiteFormato.PontoFinal.Y - margem;

                    if (pontoMax.X.IgualComTolerancia(xFormatoLocal, 3.0, _formato.FatorEscala)
                        && pontoMax.Y.IgualComTolerancia(yFormatoLocal, 3.0, _formato.FatorEscala))
                    {
                        _objectIdsToDelete.Add(bloco.Handle, "BLOCO");
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }

        }

        private void coletarLinhasTextos(EntityObject entity)
        {
            if (entity.Type == EntityType.Line)
            {
                var line = (Line)entity;
                coletaLinha(line.CriaLinha(1.0));
                incluiLinhaDaLMParaApagar(line);
            }

            if (entity.Type == EntityType.Text)
            {
                var text = (Text)entity;
                var textoColetado = text.CriarTexto(1.0);
                addTexto(textoColetado);



                incluiTextoDaLMParaApagar(text);
            }
        }

        private bool addTexto(Texto texto)
        {
            _textos.Add(texto);
            return true;
        }


        private bool coletaLinha(Linha linha)
        {
            if (linha.PontoInicial.X.IgualComTolerancia(linha.PontoFinal.X, 0.5, Formato.FatorEscala))
            {
                var linhaEncontrada = _linhasVerticais.FirstOrDefault(l => l.PontoInicial.X.IgualComTolerancia(linha.PontoInicial.X, 0.5, Formato.FatorEscala));
                if (linhaEncontrada == null)
                {
                    _linhasVerticais.Add(linha);
                    return true;
                }

            }

            if (linha.PontoInicial.Y.IgualComTolerancia(linha.PontoFinal.Y, 0.5, Formato.FatorEscala))
            {
                var linhaEncontrada = _linhasHorizontais.FirstOrDefault(l => l.PontoInicial.Y.IgualComTolerancia(linha.PontoInicial.Y, 0.5, Formato.FatorEscala));
                if (linhaEncontrada == null)
                {
                    _linhasHorizontais.Add(linha);
                    return true;
                }

            }

            return false;
        }

        private void incluiTextoDaLMParaApagar(Text texto)
        {
            var limiteGeometrico = _formato.LimiteLM;
            var espacoLinhas = _formato.EspacoLinhasLM;

            var xTexto = texto.Position.X;
            if (limiteGeometrico.PontoInicial.X <= xTexto && xTexto <= limiteGeometrico.PontoFinal.X)
            {
                var yTexto = texto.Position.Y;
                var yAbaixoTexto = limiteGeometrico.PontoInicial.Y;
                while (yAbaixoTexto >= 0)
                {
                    if (yAbaixoTexto < yTexto)
                    {
                        _objectIdsToDelete.Add(texto.Handle, "TEXTO");

                        return;
                    }

                    if (_formato.DirecaoLM == "Down")
                    {
                        yAbaixoTexto = yAbaixoTexto - espacoLinhas;
                    }
                    else
                    {
                        yAbaixoTexto = yAbaixoTexto + espacoLinhas;
                    }
                }
            }
        }

        private void incluiLinhaDaLMParaApagar(Line line)
        {
            var limiteGeometrico = _formato.LimiteLM;
            var espacoLinhas = _formato.EspacoLinhasLM;

            var xline = line.StartPoint.X;
            if (limiteGeometrico.PontoInicial.X <= xline && xline <= limiteGeometrico.PontoFinal.X)
            {
                var yline = line.StartPoint.Y;
                var yAbaixoLine = limiteGeometrico.PontoInicial.Y;
                while (yAbaixoLine >= 0)
                {
                    if (yAbaixoLine.IgualComTolerancia(yline, 1.0, 1.0))
                    {
                        _objectIdsToDelete.Add(line.Handle, "LINHA");
                        return;
                    }

                    if (_formato.DirecaoLM == "Down")
                    {
                        yAbaixoLine = yAbaixoLine - espacoLinhas;
                    }
                    else
                    {
                        yAbaixoLine = yAbaixoLine + espacoLinhas;
                    }

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
                    incluiBlocodaLMParaApagar((Insert)entity);

                    buscarEntidades((Insert)entity);
                }
                else
                {
                    coletarLinhasTextos(entity);
                }
            }
        }

        
    }
}
