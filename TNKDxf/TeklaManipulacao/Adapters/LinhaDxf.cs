using netDxf;
using TSD = Tekla.Structures.Drawing;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class LinhaDxf : ObjetosDxf
    {
        private DxfDocument _doc;

        public LinhaDxf(DxfDocument doc)
        {
            _doc = doc;
        }

        public void Processar(TSD.Line linha, TSD.View view)
        {
            var p1 = transformarPonto(linha.StartPoint, view);
            var p2 = transformarPonto(linha.EndPoint, view);

            _doc.Entities.Add(new netDxf.Entities.Line(
                new Vector3(p1.X, p1.Y, 0),
                new Vector3(p2.X, p2.Y, 0)
            ));
        }
    }
}
