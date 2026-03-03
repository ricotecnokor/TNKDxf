using netDxf;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;


namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class PolilinhaDxf : ObjetosDxf
    {
        private DxfDocument _doc;

        public PolilinhaDxf(DxfDocument doc)
        {
            _doc = doc;
        }

        public void Processar(TSD.Polyline poly, TSD.View view)
        {

            // Certifique-se de que a lista usa o tipo completo ou o namespace acima
            var vertices = new List<Polyline2DVertex>();

            foreach (TSG.Point p in poly.Points)
            {
                // Usa o método transformarPonto herdado de ObjetosDxf
                var pt = transformarPonto(p, view);

                // No netDxf 2023, a forma mais segura de criar o vértice é passando um Vector2
                vertices.Add(new Polyline2DVertex(new Vector2(pt.X, pt.Y)));
            }

            // Cria a polilinha (LWPolyline) e adiciona ao documento DXF
            Polyline2D lwPoly = new Polyline2D(vertices, false);
            _doc.Entities.Add(lwPoly);
        }
    }
}
