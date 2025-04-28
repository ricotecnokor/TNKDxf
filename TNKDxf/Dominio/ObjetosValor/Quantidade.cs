using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNKDxf.Dominio.ObjetosValor
{
    public class Quantidade
    {
        public Quantidade(string input)
        {
            if (input != "")
            {
                Valor = int.Parse(input, CultureInfo.InvariantCulture);
            }
            else
            {
                Valor = 1;
            }
        }

        public int Valor { get; private set; }

        public override string ToString()
        {
            return "QTD.";
        }

        public static Quantidade Default()
        {
            return new Quantidade("0");
        }
    }
}
