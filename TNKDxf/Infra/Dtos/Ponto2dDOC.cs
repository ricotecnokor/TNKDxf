using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Infra.Dtos
{
    public class Ponto2dDOC
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Ponto2d Converter()
        {
            return Ponto2d.CriarSemEscala(X, Y);
        }
    }
}
