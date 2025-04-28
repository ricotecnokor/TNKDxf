namespace TNKDxf.Dominio.ObjetosValor
{
    public class Janela
    {
        Ponto2d _cantoInferiorEsquerdo;
        Ponto2d _cantoSuperiorDireito;

        public Janela(Ponto2d cantoInferiorEsquerdo, Ponto2d cantoSuperiorDireito)
        {
            _cantoInferiorEsquerdo = cantoInferiorEsquerdo;
            _cantoSuperiorDireito = cantoSuperiorDireito;
        }

        public Ponto2d CantoInferiorEsquerdo { get => _cantoInferiorEsquerdo; private set => _cantoInferiorEsquerdo = value; }
        public Ponto2d CantoSuperiorDireito { get => _cantoSuperiorDireito; private set => _cantoSuperiorDireito = value; }
    }
}
