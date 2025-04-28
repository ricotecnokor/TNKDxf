namespace TNKDxf.Dominio.ObjetosValor
{
    public class Codigo
    {
        public Codigo(string input)
        {
            Valor = input.Trim();
        }

        public string Valor { get; private set; }

        public override string ToString()
        {
            return "CÒDIGO";
        }

        public static Codigo Default()
        {
            return new Codigo("");
        }
    }
}
