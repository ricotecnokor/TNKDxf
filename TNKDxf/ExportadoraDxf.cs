using Tekla.Structures;
using System.Diagnostics;
using System.IO;
using TSM = Tekla.Structures.Model;
using TSO = Tekla.Structures.Model.Operations;

namespace TNKDxf
{
    public static class ExportadoraDxf
    {
        public static void Run()
        {
            TSM.Model model = new TSM.Model();
            string modelPath = model.GetInfo().ModelPath;

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            var destino = modelPath + xsplot.Replace(".", "");

            TSM.Operations.Operation.DisplayPrompt("Exporting DWG Files.");

            string TSBinaryDir = "";

            TSM.Model CurrentModel = new TSM.Model();

            TeklaStructuresSettings.GetAdvancedOption("XSBIN", ref TSBinaryDir);


            string ApplicationName = "Dwg.exe";

            string ApplicationPath = Path.Combine(TSBinaryDir, "Applications\\Tekla\\Drawings\\DwgExport\\" + ApplicationName);

            string dwgxportParams = "export outputDirectory=\""  + destino + "\"";

          
            Process NewProcess = new Process();


            if (File.Exists(ApplicationPath))
            {

                NewProcess.StartInfo.FileName = ApplicationPath;


                try

                {
           
                    NewProcess.StartInfo.Arguments = dwgxportParams;

                    NewProcess.Start();

                    NewProcess.WaitForExit();

                }

                catch

                {

                    TSO.Operation.DisplayPrompt(ApplicationName + " failed to start.");

                }

            }

            else

            {

                TSO.Operation.DisplayPrompt(ApplicationName + " not found.");

            }

            TSM.Operations.Operation.DisplayPrompt("DWG Files Exported.");

        }
    }
}
