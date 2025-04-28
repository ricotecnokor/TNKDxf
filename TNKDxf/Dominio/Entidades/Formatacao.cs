using System.Collections.Generic;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Entidades
{
    public class Formatacao
    {
        public string Id { get; set; }

        public string NumeroProjeto { get; set; }

        public string Formato { get; set; }

        public int QtdLinhasRevisao { get; set; }

        public double EspacoLinhasRevisao { get; set; }

        public double EspacoLinhasLM { get; set; }
        public string DirecaoLM { get; set; }
        public Ponto2d CantoSuperiorDireitoTituloCabecalhoLM { get; set; }
        public Ponto2d CantoInferiorEsquerdoTituloCabecalhoLM { get; set; }
        public Ponto2d CantoInferiorEsquerdoPrimeiraLinhaLM { get; set; }
        public Ponto2d CantoSuperiorDireitoPrimeiraLinhaLM { get; set; }

        public List<CampoTexto> CamposFormato { get; set; } = new List<CampoTexto>();

        public List<CampoTexto> LinhaRevisao { get; set; } = new List<CampoTexto>();
        public List<CampoTexto> LinhaLM { get; set; } = new List<CampoTexto>();
    }
}
