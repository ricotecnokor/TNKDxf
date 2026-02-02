using Dynamic.Tekla.Structures;
using System.Diagnostics;
using System.IO;
using TSM = Dynamic.Tekla.Structures.Model;
using TSO = Dynamic.Tekla.Structures.Model.Operations;

namespace TNKDxf
{
    public static class ExportadoraDxf
    {
        public static void Run(string DWGFolder)
        {

            TSM.Operations.Operation.DisplayPrompt("Exporting DWG Files.");

            string TSBinaryDir = "";

            TSM.Model CurrentModel = new TSM.Model();

            TeklaStructuresSettings.GetAdvancedOption("XSBIN", ref TSBinaryDir);


            string ApplicationName = "Dwg.exe";

            string ApplicationPath = Path.Combine(TSBinaryDir, "Applications\\Tekla\\Drawings\\DwgExport\\" + ApplicationName);

            string dwgxportParams = "export outputDirectory=\""  + DWGFolder + "\"";

            //string configName = "TNK_DXF";
            //string dwgxportParams = "export outputDirectory=\"" + DWGFolder + "\" selectionConfig=\"" + configName + "\"";

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
