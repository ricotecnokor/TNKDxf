using netDxf;
using netDxf.Entities;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class PecaDxf : ObjetosDxf
    {
        private DxfDocument _doc;

        public PecaDxf(DxfDocument doc)
        {
            _doc = doc;
        }

        public void Processar(TSD.Part peca, TSD.View view)
        {



            // O GetObjects aqui retorna as linhas que compõem o desenho da peça
            var drawingObjetcts = peca.GetRelatedObjects();


            while (drawingObjetcts.MoveNext())
            {


                var drawingObjetct = drawingObjetcts.Current;


                if (drawingObjetct is TSD.Mark marca)
                {
                    new MarcaDxf(_doc).Processar(marca, view);
                }





                // 1. Processa Linhas de contorno da peça
                else if(drawingObjetct is TSD.Line linhaPeca)
                {
                    var p1 = transformarPonto(linhaPeca.StartPoint, view);
                    var p2 = transformarPonto(linhaPeca.EndPoint, view);

                    var dxfLine = new netDxf.Entities.Line(
                        new Vector3(p1.X, p1.Y, 0),
                        new Vector3(p2.X, p2.Y, 0)
                    );

                    // Diferencia camadas (Visible vs Hidden) se desejar
                    dxfLine.Layer = new netDxf.Tables.Layer("PECA_CONTORNO");
                    _doc.Entities.Add(dxfLine);
                }
                // 2. Processa Polilinhas (comum em cantos arredondados ou chapas)
                else if (drawingObjetct is TSD.Polyline polyPeca)
                {
                    var vertices = new System.Collections.Generic.List<Polyline2DVertex>();
                    foreach (TSG.Point p in polyPeca.Points)
                    {
                        var pt = transformarPonto(p, view);
                        vertices.Add(new Polyline2DVertex(new Vector2(pt.X, pt.Y)));
                    }
                    var dxfPoly = new Polyline2D(vertices, false);
                    dxfPoly.Layer = new netDxf.Tables.Layer("PECA_CONTORNO");
                    _doc.Entities.Add(dxfPoly);
                }
            }
        }
    }
}