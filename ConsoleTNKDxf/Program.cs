using System;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            var wifi = ServicoWifi.VerificarEConectar();
            Console.ForegroundColor = wifi.Conectado ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(wifi.Mensagem);
            Console.ForegroundColor = ConsoleColor.Red;

            bool conectou = TesteConexao.Testar(out string mensagemConexao);
            Console.ForegroundColor = conectou ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(mensagemConexao);
            Console.ForegroundColor = ConsoleColor.Red;

            ExportacaoDxf.Exportar();

            const string VERSAO_TSEP = "1.16.1";

            TSM.Model modelTemp = new TSM.Model();
            bool conectado = modelTemp.GetConnectionStatus();
            Console.WriteLine($"ConnectionStatus: {conectado}");

            if (!conectado)
            {
                Console.WriteLine("Não foi possível conectar ao modelo.");
                return;
            }

            string nomeModel = modelTemp.GetInfo().ModelName;
            Console.WriteLine($"Modelo conectado: {nomeModel}");

            TSD.DrawingHandler dh = new TSD.DrawingHandler();

            var dg = dh.GetDrawingSelector().GetSelected();

            if (!(dg.GetSize() > 0))
            {
                Console.WriteLine("Modelo corrente não possui desenho selecionado. Selecione o modelo e os desenhos a exportar no Document manager");
                return;
            }

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
    }
}
