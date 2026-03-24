using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Tekla.Structures;
using TSM = Tekla.Structures.Model;
using TSO = Tekla.Structures.Model.Operations;
using TSD = Tekla.Structures.Drawing;
using System.Collections.Generic;

namespace ConsoleTNKDxf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //RespostaModelo resposta = BuscarModeloUso();
            TSM.Model modelTemp = new TSM.Model();


            string nomeModel = modelTemp.GetInfo().ModelName;


            TSD.DrawingHandler dh = new TSD.DrawingHandler();

            var dg = dh.GetDrawingSelector().GetSelected();

            if (!(dg.GetSize() > 0))
            {
                Console.WriteLine("Modelo corrente não possui desenho selecionado. Selecione o modelo e os desenhos a exportar no Document manager");
                return;
                //return new RespostaModelo(false, null, );
            }

            //if (!resposta.Sucesso)
            //{
            //    Console.WriteLine(resposta.Mensagem);
            //    return;
            //}

            //TSM.Model model = resposta.Model;

            exportar();

            IAdapterDesenho adapterDesenho = new AdapterDesenho(modelTemp);

            Console.WriteLine("Coletando arquivos...");

            var resposta = adapterDesenho.ColetarArquivos();

            if (!resposta.Sucesso)
            {
                Console.WriteLine(resposta.Mensagem);
                return;
            }

            Console.WriteLine(@"Processo concluído com arquivos na pasta .\PlotFiles\Enviar, Pressione qualquer tecla para sair.");

            //Environment.Exit(0);

        }

        private static RespostaModelo BuscarModeloUso()
        {
            //RespostaModelo resposta = null;
            //string nomeDoModelo = string.Empty;
            //var processosTekla = Process.GetProcessesByName("TeklaStructures");




            //if (processosTekla.Count() == 1)
            //{
            //    TSM.Model modelUnico = new TSM.Model();
            //    return new RespostaModelo(true, modelUnico, "Sucesso! Somente um modelo encontrado.");
            //}

            //TSD.DrawingEnumerator dg = null;
            //TSM.Model modelTemp = null;
            //Process process = null;
            //List<string> modelosProcessos = new List<string>();
            //foreach (var processo in processosTekla)
            //{

            //    var nomeProcesso = processo.MainWindowTitle.Split('\\').Last().Trim();

            //    //TSM.Model modelPath = new TSM.Model();

            //    // Tenta conectar à instância específica deste processo
            //    // Nota: A API do Tekla 2024 permite vincular ao ID do processo
            //    //if (modelPath.GetConnectionStatus())
            //    //{
            //        //var info = modelPath.GetInfo();
            //        modelosProcessos.Add(nomeProcesso);
            //        //modelPath = null;
            //    //}

            //    //modelPath = null;
            //}

            //List<string> modelosComDesenho = new List<string>();
            //foreach (var processo in processosTekla)
            //{

                //try
                //{
                    //var nomeProcesso = processo.MainWindowTitle.Split('\\').Last().Trim();
                    TSM.Model modelTemp = new TSM.Model();


                    string nomeModel = modelTemp.GetInfo().ModelName;


                    TSD.DrawingHandler dh = new TSD.DrawingHandler();

                    var dg = dh.GetDrawingSelector().GetSelected();

                    //if (dg == null || dg.GetSize() == 0)
                    //{
                    //    modelTemp = null;
                    //    continue;
                    //}

                    if(!(dg.GetSize() > 0))
                    {
                        return new RespostaModelo(false, null, "Modelo corrente não possui desenho selecionado. Selecione o modelo e os desenhos a exportar no Document manager");
                    }

                    //if (dg.GetSize() > 0 && !modelosComDesenho.Any(x => x == nomeProcesso))    
                    //{
                    //    modelosComDesenho.Add(nomeProcesso);

                    //}

                    //var modelCerto = new TSM.Model();
                    return new RespostaModelo(true, modelTemp, "Modelo encontrado.");

                //}
                //catch
                //{
                //    //Console.WriteLine("Não foi possível acessar o processo " + nomeProcesso + ", ele pode estar protegido ou ser de outra instância do Tekla.");
                //    return new RespostaModelo(false, null, "Não foi possível acessar o o Tekla"); //+ nomeProcesso);
                //}
           // }

            //return new RespostaModelo(false, null, "Não foi possível acessar o o Tekla");
            //if (modelTemp == null)
            //{
            //    return new RespostaModelo(false, null, "Nenhum dos modelos possui desenho selecionado. Selecione em um deles");
            //}

            //if (modelosComDesenho.Count > 1)
            //{
            //    return new RespostaModelo(false, null, "Mais de um modelo possui desenho selecionado. Selecione em apenas um deles");
            //}

            //var modelCerto = new TSM.Model();
            //return new RespostaModelo(true, modelCerto, "Modelo encontrado.");
        }

        //private static TSM.Model conectarAoTeklaEspecifico(string nomeDoModeloDesejado, Process[] processosTekla)
        //{
        //    // Lista todos os processos do Tekla abertos
        //    // var processosTekla = Process.GetProcessesByName("TeklaStructures");

        //    foreach (var processo in processosTekla)
        //    {
        //        TSM.Model modelTemp = new TSM.Model();

        //        // Tenta conectar à instância específica deste processo
        //        // Nota: A API do Tekla 2024 permite vincular ao ID do processo
        //        if (modelTemp.GetConnectionStatus())
        //        {
        //            var info = modelTemp.GetInfo();

        //            string nomeModelo = info.ModelName.Replace(".db1","").Trim();
        //            // Verifica se o caminho ou nome do modelo coincide com o que você busca
        //            if (nomeDoModeloDesejado.Contains(nomeModelo))
        //            {
        //                return modelTemp; // Retorna a conexão correta
        //            }
        //        }
        //    }
        //    return null; // Nenhum modelo correspondente encontrado
        //}

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
