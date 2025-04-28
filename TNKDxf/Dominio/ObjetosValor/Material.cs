namespace TNKDxf.Dominio.ObjetosValor
{
    public class Material
    {
        public Material(string input)
        {
            Valor = input.Trim();
        }

        public string Valor { get; private set; }

        public override string ToString()
        {
            return "MATERIAL";
        }

        public static Material Default()
        {
            return new Material("");
        }
    }
}
