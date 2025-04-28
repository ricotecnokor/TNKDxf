namespace TNKDxf.Dominio.ObjetosValor
{
    public class Descricao
    {
        public Descricao(string input)
        {
            Valor = input.Trim();
        }

        public string Valor { get; private set; }

        public override string ToString()
        {
            return "DESCRICAO";
        }

        public static Descricao Default()
        {
            return new Descricao("");
        }


    }
}
