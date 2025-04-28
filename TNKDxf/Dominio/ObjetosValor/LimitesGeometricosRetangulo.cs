namespace TNKDxf.Dominio.ObjetosValor
{
    public class LimitesGeometricosRetangulo
    {
        public LimitesGeometricosRetangulo(Ponto2d pontoInicalRetangulo, Ponto2d pontoFinalRetangulo)
        {

            PontoInicial = pontoInicalRetangulo;
            PontoFinal = pontoFinalRetangulo;
        }

        public Ponto2d PontoInicial { get; }
        public Ponto2d PontoFinal { get; }

        public static LimitesGeometricosRetangulo Default()
        {
            return new LimitesGeometricosRetangulo(Ponto2d.CriarSemEscala(0.0, 0.0), Ponto2d.CriarSemEscala(0.0, 0.0));
        }
    }
}

