namespace TNKDxf.Dominio.ObjetosValor
{
    public class Linha
    {
        Ponto2d _pontoInicial;
        Ponto2d _pontoFinal;

        public Linha(Ponto2d pontoInicial, Ponto2d pontoFinal)
    {
        _pontoInicial = pontoInicial;
        _pontoFinal = pontoFinal;
    }

    public Ponto2d PontoInicial { get => _pontoInicial; }
    public Ponto2d PontoFinal { get => _pontoFinal; }
}
}
