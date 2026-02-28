using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using Tekla.Structures.Drawing.UI;
using Tekla.Structures.DrawingInternal;
using TSD = Tekla.Structures.Drawing;
using TSO = Tekla.Structures.Model.Operations;

namespace TNKDxf.TeklaManipulacao
{
    public class DesenhoSelector
    {
        private TSD.DrawingHandler _drawingHandler;

        public DesenhoSelector()
        {
            _drawingHandler = new TSD.DrawingHandler();
        }

        public List<int> Selecionar(List<string> desenhosParaSelecionar)
        {
            List<int> selecionados = new List<int>();
            //PRJ-00019-D-00126
            DrawingSelector selector = _drawingHandler.GetDrawingSelector();

            // Pegamos todos os desenhos (inclui GADrawings, Assembly, Multi, etc.)
            DrawingEnumerator enumerator = _drawingHandler.GetDrawings();

            ArrayList desenhosParaDestacar = new ArrayList();

            while (enumerator.MoveNext())
            {

                MultiDrawing multi = enumerator.Current as MultiDrawing;

                if (multi != null)
                {
                    if (desenhosParaSelecionar.Contains(multi.Title1))
                    {
                        int id = multi.GetIdentifier().ID;
                        TSO.Operation.RunMacro($"DrawingList.SelectById({id});");
                        selecionados.Add(id);
                    }
                }



            }
            return selecionados;
        }
    }
}