using System.Collections.Generic;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Listas;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Infra.Dtos
{
    public class FormatacaoDTO
    {
        public string Id { get; set; }

        public string NumeroProjeto { get; set; }

        public string Formato { get; set; }

        public int QtdLinhasRevisao { get; set; }

        public double EspacoLinhasRevisao { get; set; }

        public double EspacoLinhasLM { get; set; }
        public string DirecaoLM { get; set; }
        public Ponto2dDOC CantoSuperiorDireitoTituloCabecalhoLM { get; set; }
        public Ponto2dDOC CantoInferiorEsquerdoTituloCabecalhoLM { get; set; }
        public Ponto2dDOC CantoInferiorEsquerdoPrimeiraLinhaLM { get; set; }
        public Ponto2dDOC CantoSuperiorDireitoPrimeiraLinhaLM { get; set; }

        public List<CampoFormatoDOC> CamposFormato { get; set; }

        public List<CampoFormatoDOC> LinhaRevisao { get; set; }
        public List<CampoFormatoDOC> LinhaLM { get; set; }

        public Formatacao Converter()
        {
            return new Formatacao
            {
                Id = Id,
                NumeroProjeto = NumeroProjeto,
                Formato = Formato,
                QtdLinhasRevisao = QtdLinhasRevisao,
                EspacoLinhasRevisao = EspacoLinhasRevisao,
                EspacoLinhasLM = EspacoLinhasLM,
                DirecaoLM = DirecaoLM,
                CantoSuperiorDireitoTituloCabecalhoLM = Ponto2d.CriarSemEscala(CantoSuperiorDireitoTituloCabecalhoLM.X, CantoSuperiorDireitoTituloCabecalhoLM.Y),
                CantoInferiorEsquerdoTituloCabecalhoLM = Ponto2d.CriarSemEscala(CantoInferiorEsquerdoTituloCabecalhoLM.X, CantoInferiorEsquerdoTituloCabecalhoLM.Y),
                CantoInferiorEsquerdoPrimeiraLinhaLM = Ponto2d.CriarSemEscala(CantoInferiorEsquerdoPrimeiraLinhaLM.X, CantoInferiorEsquerdoPrimeiraLinhaLM.Y),
                CantoSuperiorDireitoPrimeiraLinhaLM = Ponto2d.CriarSemEscala(CantoSuperiorDireitoPrimeiraLinhaLM.X, CantoSuperiorDireitoPrimeiraLinhaLM.Y),
                CamposFormato = ListaDeCampos.Converter(CamposFormato),
                LinhaRevisao = ListaDeCampos.Converter(LinhaRevisao),
                LinhaLM = ListaDeCampos.Converter(LinhaLM)

            };
        }
    }
}
