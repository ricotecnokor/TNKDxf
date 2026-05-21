using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTNKDxf.EstruturaDxf
{
    public struct Linha
    {
        public Ponto2D PontoInicial { get; set; }
        public Ponto2D PontoFinal { get; set; }

        public Linha(Ponto2D pontoInicial, Ponto2D pontoFinal)
        {
            PontoInicial = pontoInicial;
            PontoFinal = pontoFinal;
        }

    }
}
