namespace TNKDxf.Dominio.ObjetosValor
{
    public class Observacao
    {
        public Observacao(string input)
        {
            Valor = input.Trim();
        }

        public string Valor { get; private set; }

        public override string ToString()
        {
            return "OBSERVACAO";
        }

        public static Observacao Default()
        {
            return new Observacao("");
        }
    }
}
