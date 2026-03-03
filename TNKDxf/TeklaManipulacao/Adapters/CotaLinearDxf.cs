using netDxf;
using netDxf.Entities;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class CotaLinearDxf : CotaDxf
    {
        public CotaLinearDxf(DxfDocument doc) : base(doc) { }

        public void Processar(TSD.StraightDimension cota, TSD.View view)
        {
            // Transforma os pontos para o espaço da folha
            TSG.Point p1 = transformarPonto(cota.StartPoint, view);
            TSG.Point p2 = transformarPonto(cota.EndPoint, view);

            // No netDxf 3.0.1, LinearDimension usa Vector2
            var dxfDim = new LinearDimension(
                new Vector2(p1.X, p1.Y),
                new Vector2(p2.X, p2.Y),
                cota.Distance,
                0 // O ângulo é calculado automaticamente pelo netDxf baseado nos pontos
            );

            ConfigurarEstilo(dxfDim, cota);
            _doc.Entities.Add(dxfDim);
        }
    }
}