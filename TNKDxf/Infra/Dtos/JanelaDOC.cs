using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Infra.Dtos
{
    public class JanelaDOC
    {
        public Ponto2dDOC CantoInferiorEsquerdo { get; set; }
        public Ponto2dDOC CantoSuperiorDireito { get; set; }

        public Janela Converter()
        {
            return new Janela(CantoInferiorEsquerdo.Converter(), CantoSuperiorDireito.Converter());
        }
    }
}
