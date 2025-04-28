namespace TNKDxf.ViewModel
{
    public class CampoErroWpf
    {
        public CampoErroWpf(string campo, string descricaoErro)
        {
            Campo = campo;
            DescricaoErro = descricaoErro;
        }

        public string Campo { get; set; }
        public string DescricaoErro { get; set; }
    }
}
