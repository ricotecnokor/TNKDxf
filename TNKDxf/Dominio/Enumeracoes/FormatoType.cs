using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Enumeracoes
{
    public class FormatoType : Enumeration
    {
        public FormatoType(int id, string name) : base(id, name)
        {
        }

        public static FormatoType A1 = new FormatoType(1, nameof(A1));
        public static FormatoType A2 = new FormatoType(1, nameof(A2));
        public static FormatoType A3 = new FormatoType(1, nameof(A2));
        public static FormatoType A4 = new FormatoType(2, nameof(A4));
        public static FormatoType DEZA4 = new FormatoType(3, "10A4");
        public static FormatoType INEXISTENTE = new FormatoType(3, "INEXISTENTE");

        public static FormatoType ObterTipo(string tipo)
        {
            switch (tipo)
            {
                case "A1":
                    return FormatoType.A1;
                case "A2":
                    return FormatoType.A2;
                case "A3":
                    return FormatoType.A3;
                case "A4":
                    return FormatoType.A4;
                case "10A4":
                    return FormatoType.DEZA4;
                default:
                    return FormatoType.INEXISTENTE;
            }
        }

        public Ponto2d PontoMaximo()
        {
            switch (this.Name)
            {
                case "A1":
                    return Ponto2d.CriarSemEscala(841, 594);
                case "A2":
                    return Ponto2d.CriarSemEscala(594, 297);
                case "A3":
                    return Ponto2d.CriarSemEscala(412, 297);
                case "A4":
                    return Ponto2d.CriarSemEscala(297, 210);
                case "10A4":
                    return Ponto2d.CriarSemEscala(1051, 594);
                default:
                    return Ponto2d.CriarSemEscala(841, 594);
            }

        }
    }
}
