using System;

namespace ConsoleTNKDxf.EstruturaDxf
{
    public class Cota
    {
        public Ponto2D PontoInicial { get; set; }
        public Ponto2D PontoFinal { get; set; }
        public double Distancia { get; set; }

        public Cota(Ponto2D pontoInicial, Ponto2D pontoFinal, double distancia)
        {
            PontoInicial = pontoInicial;
            PontoFinal = pontoFinal;
            Distancia = distancia;
        }
    }
}
