using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Infra.Dtos
{
    public class CampoFormatoDOC
    {
        public TextoDOC Texto { get; set; }
        public JanelaDOC Janela { get; set; }

        public CampoTexto Converter()
        {
            return new CampoTexto(Texto.Converter(), Janela.Converter());
        }
    }
}
