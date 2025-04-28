using System;
using System.Globalization;

namespace TNKDxf.Dominio.ObjetosValor
{
    public class Peso
    {
        public Peso(string input)
        {
            var peso = double.Parse(input, CultureInfo.InvariantCulture);
            Valor = Math.Round(peso, 1);
        }

        public Peso(double peso)
        {
            Valor = Math.Round(peso, 1);
        }

        public double Valor { get; private set; }

        public override string ToString()
        {
            return "PESO";
        }

        public static Peso Default()
        {
            return new Peso("0");
        }

        public void AddPeso(double peso)
        {
            Valor += peso;
        }
    }
}
