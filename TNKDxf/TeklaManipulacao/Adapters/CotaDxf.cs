using netDxf;
using netDxf.Entities;
using Tekla.Structures.Drawing;
using TSD = Tekla.Structures.Drawing;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public abstract class CotaDxf : ObjetosDxf
    {
        protected DxfDocument _doc;

        protected CotaDxf(DxfDocument doc)
        {
            _doc = doc;
        }

        // Método comum para configurar o estilo da cota no netDxf
        protected void ConfigurarEstilo(Dimension dxfDim, TSD.DimensionBase cotaTekla)
        {
            // Aqui você pode mapear cores e tamanhos de texto do Tekla para o DXF
            dxfDim.Color = netDxf.AciColor.Default;
        }

        protected void ConfigurarEstilo(Dimension dxfDim, TSD.DimensionSetBase cotaTekla)
        {
            // Aqui você pode mapear cores e tamanhos de texto do Tekla para o DXF
            dxfDim.Color = netDxf.AciColor.Default;
        }
    }
}

