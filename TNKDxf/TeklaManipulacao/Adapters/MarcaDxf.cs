using netDxf;
using netDxf.Entities;
using Tekla.Structures.Drawing;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class MarcaDxf : ObjetosDxf
    {
        private DxfDocument _doc;

        public MarcaDxf(DxfDocument doc)
        {
            _doc = doc;
        }

        public void Processar(TSD.Mark marca, TSD.View view)
        {
          
            var formas = marca.GetRelatedObjects();

            while (formas.MoveNext())
            {
                var forma = formas.Current;

                if (forma is TSD.LeaderLine leaderLine)
                {
                    TSG.Point pInicio = transformarPonto(leaderLine.StartPoint, view);
                    TSG.Point pFim = transformarPonto(leaderLine.EndPoint, view);

                    netDxf.Entities.Line dxfLeader = new netDxf.Entities.Line(
                        new Vector3(pInicio.X, pInicio.Y, 0),
                        new Vector3(pFim.X, pFim.Y, 0)
                    );

                    dxfLeader.Layer = new netDxf.Tables.Layer("MARCAS_LINHA_CHAMADA");
                    _doc.Entities.Add(dxfLeader);
                }

                if (forma is TSD.Text text)
                {
                    TSG.Point pontoTransformado = transformarPonto(text.InsertionPoint, view);
                    netDxf.Entities.Text dxfText = new netDxf.Entities.Text(
                        text.TextString,
                        new netDxf.Vector3(pontoTransformado.X, pontoTransformado.Y, 0),
                        text.Attributes.Font.Height,
                        netDxf.Tables.TextStyle.Default
                    );
                    dxfText.Rotation = text.Attributes.Angle;
                    dxfText.Layer = new netDxf.Tables.Layer("MARCAS_TEXTO");
                    _doc.Entities.Add(dxfText);
                }

                

            }
        }
    }
}