using ConsoleAppLista.TeklaRelatorios;
using System;

namespace ConsoleAppLista
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string nomeRelatorio = Guid.NewGuid().ToString("N").Substring(0, 8);
            LeitorRelatorio leitorRelatorio = new LeitorRelatorio(
                new LeitorRelatorioParams(
                    @"Q:\05 - PADRONIZACAO\PADRONIZACAO\Listas\TNK_LEITURA_PROGRAMA_PADRAO.rpt",
                    @"C:\TeklaStructuresModels\PONTE-CSN3_ricardo\Listas\" + nomeRelatorio + ".xsr"));

            var linhas = leitorRelatorio.Ler();

            var colecaoPadronizacao = new ColecaoPadronizacao();

            colecaoPadronizacao.Coletar(linhas);
        }
    }
}
