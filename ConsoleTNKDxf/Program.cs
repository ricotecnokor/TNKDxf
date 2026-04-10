using System;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string VERSAO_TSEP = "1.11.1";

            Console.ForegroundColor = ConsoleColor.Red;

            //RespostaModelo resposta = BuscarModeloUso();
            TSM.Model modelTemp = new TSM.Model();


            if(!modelTemp.GetConnectionStatus())
            {
                Console.WriteLine("Não foi possível conectar ao modelo.");
                return;
            }

            string nomeModel = modelTemp.GetInfo().ModelName;


            TSD.DrawingHandler dh = new TSD.DrawingHandler();

            var dg = dh.GetDrawingSelector().GetSelected();

            if (!(dg.GetSize() > 0))
            {
                Console.WriteLine("Modelo corrente não possui desenho selecionado. Selecione o modelo e os desenhos a exportar no Document manager");
                return;
            }

            ExportacaoDxf.Exportar();

            IAdapterDesenho adapterDesenho = new AdapterDesenho(modelTemp);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Coletando arquivos na versão {VERSAO_TSEP} ...");
            Console.ForegroundColor = ConsoleColor.Green;

            var resposta = adapterDesenho.ColetarArquivos(VERSAO_TSEP);

            if (!resposta.Sucesso)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(resposta.Mensagem);
                return;
            }


            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($@"Desenhos tipo dgt na versão {VERSAO_TSEP}  criados na pasta .\PlotFiles\Enviar.");
            Console.WriteLine("Pressione qualquer tecla para sair.");

            Console.ReadKey();

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

        
    }
}
