using Dynamic.Tekla.Structures;
using System.Windows;
using System.Windows.Media;
using TSD = Dynamic.Tekla.Structures.Drawing;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var dh = new TSD.DrawingHandler();
            var drawing = dh.GetActiveDrawing();
            if (!DxfFoiExportado(drawing))
            {
                
                lblplot.Content = $"FAVOR EXPORTAR O DESENHO";

            }
            else
            {
                lblplot.Content = $"DESENHO EXPORTADO:";
                lblcara.Content = $"Name: {drawing.Name}, Title1: {drawing.Title1}, Title2: {drawing.Title2}, Title3 {drawing.Title3}";
            }
        }

        private bool DxfFoiExportado(TSD.Drawing drawing)
        {
           
            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            var model = new TSM.Model();
            var info = model.GetInfo();

            var pathModel = info.ModelPath;

            var caminho = pathModel + xsplot + "\\" + drawing.Title1 + " rev0.dxf";

            if (!System.IO.File.Exists(caminho))
            {
                return false;
            }

            return true;

            
        }

    }
}
