namespace TNKDxf.Dominio.ObjetosValor
{
    public class CampoTexto
    {
        public CampoTexto(Texto texto, Janela janela)
        {
            Texto = texto;
            Janela = janela;

        }

        public Texto Texto { get; private set; }
        public Janela Janela { get; private set; }

    }
}
