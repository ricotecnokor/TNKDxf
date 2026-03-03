namespace TNKDxf.TeklaManipulacao.Adapters
{
    public interface IAdapterDesenho
    {
        void ColetarInformacoesDesenho();

        void InserirInformacoesDesenho(string nomeArquivo, RelatorioMultiDesenhos relatorio);

    }

}
