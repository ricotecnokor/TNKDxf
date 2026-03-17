using System;
using System.Diagnostics;
using System.IO;
using Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSO = Tekla.Structures.Model.Operations;

namespace ConsoleTNKDxf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //exportar();


            IAdapterDesenho adapterDesenho = new AdapterDesenho();

            Console.WriteLine("Coletando arquivos...");

            adapterDesenho.ColetarArquivos();

            Console.WriteLine(@"Processo concluído com arquivos na pasta .\PlotFiles\Enviar, Pressione qualquer tecla para sair.");

            Environment.Exit(0);

        }

        public static void exportar()
        {
            TSM.Model model = new TSM.Model();
            string modelPath = model.GetInfo().ModelPath;

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            var destino = modelPath + xsplot.Replace(".", "");


            if (Directory.Exists(destino))
            {
                try
                {
                    var arquivosExistentes = Directory.GetFiles(destino, "*.dxf");
                    foreach (var arquivo in arquivosExistentes)
                    {
                        try
                        {
                            File.Delete(arquivo);
                        }
                        catch { }
                    }
                }
                catch { }
            }



            TSM.Operations.Operation.DisplayPrompt("Exporting DWG Files.");

            string TSBinaryDir = "";

            TSM.Model CurrentModel = new TSM.Model();

            TeklaStructuresSettings.GetAdvancedOption("XSBIN", ref TSBinaryDir);


            string ApplicationName = "Dwg.exe";

            string ApplicationPath = Path.Combine(TSBinaryDir, "Applications\\Tekla\\Drawings\\DwgExport\\" + ApplicationName);

            string dwgxportParams = "export outputDirectory=\"" + destino + "\"";


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
