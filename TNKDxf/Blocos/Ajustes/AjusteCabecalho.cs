using netDxf.Entities;
using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.Listas;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Colecoes;
using TNKDxf.Dominio.Abstracoes;
using TNKDxf.Dominio.Dxfs;

namespace TNKDxf.Blocos
{
    public class AjusteCabecalho
    {

        protected const double RECUO_ESTICAMENTO = 10.0;
        protected Formato _formato;
        private ArquivoDxf _arquivoDxf;
        public AjusteCabecalho(Formato formato, ArquivoDxf arquivoDxf) 
        {
            _formato = formato;
            _arquivoDxf = arquivoDxf;
        }

        public void Ajustar(Insert insert)
        {
            double recuoLinha = RECUO_ESTICAMENTO;

            double acrescimoComprimento = 0.0;
            if (_formato.DirecaoLM == "Down")
            {
                acrescimoComprimento = _arquivoDxf.ObterTamanhoDaLista() * -1;
                recuoLinha = recuoLinha * -1;
            }
            else
            {
                acrescimoComprimento = _arquivoDxf.ObterTamanhoDaLista(); 
            }

            foreach (var ent in insert.Block.Entities)
            {
                if (ent.Type == EntityType.Line)
                {
                    var line = (Line)ent;

                    Vector3 ptZero = new Vector3(0.0, 0.0, 0.0);

                    double yReferencia1 = -38.0;
                    double yReferencia2 = -28.0;

                    if (line.StartPoint.X.IgualComTolerancia(line.EndPoint.X, 0.5, _formato.FatorEscala))
                    {
                        double yMenor = line.StartPoint.Y < line.EndPoint.Y ? line.StartPoint.Y : line.EndPoint.Y;
                        if (yMenor.IgualComTolerancia(yReferencia1, 0.5, _formato.FatorEscala) || yMenor.IgualComTolerancia(yReferencia2, 0.5, _formato.FatorEscala))
                        {
                            string strPonto = line.StartPoint.Y == yMenor ? "StartPoint" : "EndPoint";
                            switch (strPonto)
                            {
                                case "StartPoint":
                                    {
                                        line.StartPoint = new Vector3(
                                        line.StartPoint.X,
                                        line.StartPoint.Y + acrescimoComprimento - recuoLinha,
                                        line.StartPoint.Z);
                                    }
                                    break;
                                case "EndPoint":
                                    {
                                        line.EndPoint = new Vector3(
                                        line.EndPoint.X,
                                        line.EndPoint.Y + acrescimoComprimento - recuoLinha,
                                        line.EndPoint.Z);
                                    }
                                    break;
                            }
                        }

                    }

                    if (line.StartPoint.Y.IgualComTolerancia(line.EndPoint.Y, 0.5, _formato.FatorEscala))
                    {
                        if (line.StartPoint.Y.IgualComTolerancia(yReferencia1, 0.5, _formato.FatorEscala) || line.StartPoint.Y.IgualComTolerancia(yReferencia2, 0.5, _formato.FatorEscala))
                        {
                            moverLinhaCabecalho(acrescimoComprimento, recuoLinha, line);
                        }
                    }
                }

                if (ent.Type == EntityType.Text)
                {
                    var texto = (Text)ent;
                    if (texto.Value.Contains("CALCULADO"))
                    {
                        texto.Value =
                        $"PESO TOTAL CALCULADO={Math.Round(_arquivoDxf.ObterPesoTotalDaLM(), 1)}kg";
                        moverTextoCabecalho(acrescimoComprimento, recuoLinha, texto);
                    }
                }


            }

        }
        private void moverTextoCabecalho(double deslocamento, double desconto, Text texto)
        {
            Vector3 novoPt = new Vector3(
                                        texto.Position.X,
                                        texto.Position.Y + deslocamento - desconto,
                                        texto.Position.Z);
            texto.Position = novoPt;

        }

        private void moverLinhaCabecalho(double deslocamento, double desconto, Line line)
        {
            Vector3 novoPtIni = new Vector3(
                                        line.StartPoint.X,
                                        line.StartPoint.Y + deslocamento - desconto,
                                        line.StartPoint.Z);

            Vector3 novoPtFinal = new Vector3(
                            line.EndPoint.X,
                            line.EndPoint.Y + deslocamento - desconto,
                            line.EndPoint.Z);

            line.StartPoint = novoPtIni;
            line.EndPoint = novoPtFinal;
        }

    }
}
