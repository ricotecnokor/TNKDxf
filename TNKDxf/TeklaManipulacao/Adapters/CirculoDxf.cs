using netDxf;
using netDxf.Entities;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class CirculoDxf : ObjetosDxf
    {
        private DxfDocument _doc;

        public CirculoDxf(DxfDocument doc)
        {
            _doc = doc;
        }

        public void Processar(TSD.Circle circulo, TSD.View view)
        {
            // Transforma o centro do círculo para a folha
            TSG.Point centroTransformado = transformarPonto(circulo.CenterPoint, view);

            // O raio permanece o mesmo (escala 1:1 na matriz já tratada)
            double raio = circulo.Radius;

            // No netDxf 3.0.1, o centro é um Vector3
            Circle dxfCircle = new Circle(new Vector3(centroTransformado.X, centroTransformado.Y, 0), raio);

            _doc.Entities.Add(dxfCircle);
        }
    }
}