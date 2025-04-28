namespace TNKDxf.Dominio.ObjetosValor
{
    public class Erro
    {
        string _mensagem;
        Ponto2d _ponto;

        public Erro(string mensagem, Ponto2d ponto)
        {
            _mensagem = mensagem;
            _ponto = ponto;
        }

        public string Mensagem { get => _mensagem; private set => _mensagem = value; }
        public Ponto2d Ponto { get => _ponto; private set => _ponto = value; }
    }
}
