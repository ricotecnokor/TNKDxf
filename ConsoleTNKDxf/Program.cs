using System;

namespace ConsoleTNKDxf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IAdapterDesenho adapterDesenho = new AdapterDesenho();

            Console.WriteLine("Coletando arquivos...");

            adapterDesenho.ColetarArquivos();

            Console.WriteLine(@"Processo concluído com arquivos na pasta .\PlotFiles\Enviar, Pressione qualquer tecla para sair.");

            //Environment.Exit(0);

        }
    }
}
