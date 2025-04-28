using Dynamic.Tekla.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TNKDxf.TestesViabilidade
{
    public static class TesteA
    {
        /// <summary>
        /// Select single part drawing for the selected part in model using document manager 
        /// or drawing list as appropriate.
        /// Assumes document manager or drawing list is open.
        /// </summary>

        public static void SelectSinglePartDrawingForSelectedPartInDocumentManagerOrDrawingList()
        {
            if (!TeklaStructures.Connect()) return;

            // requires that either drawing list or document manager is already open
            var builder = new MacroBuilder();


            // Is the loaded version of MacroBuilder capable of handling document manager and is document manager the chosen UI?
            // Also, check that the required MethodInfo(s) are supported.

            /*Tekla.Macros.Wpf.Runtime.IWpfMacroHost wpf = runtime.Get<Tekla.Macros.Wpf.Runtime.IWpfMacroHost>();
            wpf.View("DocumentManager.MainWindow").Find("AID_DOCMAN_DataGridControl", "AID_DocMgr_Mark").As.Button.Invoke();
            wpf.View("DocumentManager.MainWindow").Find("AID_DOCMAN_DataGridControl", "AID_DocMgr_DocumentType").As.Button.Invoke();
            wpf.View("DocumentManager.MainWindow").Find("AID_DOCMAN_DataGridControl", "AID_DocMgr_CreationDate").As.Button.Invoke(Tekla.Macros.Runtime.ModifierKeys.Shift);*/

            builder.PushButton("dia_draw_filter_by_parts", "Drawing_selection");

            //builder.ValueChange("Drawing_selection", "diaSavedSearchOptionMenu", "10");
            //builder.PushButton("dia_draw_filter_by_parts", "Drawing_selection");
            //builder.TableSelect("Drawing_selection", "dia_draw_select_list", 1);

            builder.Run();
                //MacroBuilder.WaitForMacroToRun();
                return;
            

            // XS_USE_OLD_DRAWING_LIST_DIALOG is set to false, but unable to find all the macrobuilder methods needed
            MessageBox.Show("This version of Tekla Structures does not fully support automation of Document Manager using this macro.");
            return;
        }
    }
}
