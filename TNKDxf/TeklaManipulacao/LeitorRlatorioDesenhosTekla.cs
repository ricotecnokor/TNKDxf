using System;
using System.Collections.Generic;
using System.IO;
using Tekla.Structures.Model.Operations;

namespace TNKDxf.TeklaManipulacao
{
    public class LeitorRlatorioDesenhosTekla
    {
        private LeitorRelatorioParams _parametros;
        private RelatorioMultiDesenhos _relatorio;
        private ArquivoRelatorioMultiDesenhos _arquivoRelatorio;

        public LeitorRlatorioDesenhosTekla(string nome)
        {
            _arquivoRelatorio = new ArquivoRelatorioMultiDesenhos(nome);

            string nomeRelatorio = Guid.NewGuid().ToString("N").Substring(0, 8);


            string caminhoSalvarTemplate = string.Empty;




            _arquivoRelatorio.CriarArquivoRelatorio();
            _parametros = new LeitorRelatorioParams(
                   //@"C:\APP\TNKDxf  Lista de multi desenhos.rpt",
                   _arquivoRelatorio.Caminho,
                   @"C:\APP\" + nomeRelatorio + ".xsr");
        }

        public int Contar()
        {
            throw new NotImplementedException();
        }

        public RelatorioMultiDesenhos Ler()
        {
            var relatorio = criar();
            _arquivoRelatorio.DeleteArquivo(_arquivoRelatorio.Template);
            return relatorio;
        }

        public RelatorioMultiDesenhos criar()
        {
            List<string[]> linhas = new List<string[]>();
            Operation.CreateReportFromSelected(
                _parametros.Template,
                _parametros.Filename,
                _parametros.Title,
                _parametros.Subtitle,
                _parametros.Subtitle);

            if (File.Exists(_parametros.Filename))
            {
                StreamReader sr = new StreamReader(_parametros.Filename, System.Text.Encoding.Default);
                string line = " ";
                while ((line = sr.ReadLine()) != null)
                {
                    var linha = line.Split(';');
                    if (linha.Length > 1)
                    {
                        string tipoLinha = linha[0].Trim();
                        switch (tipoLinha)
                        {
                            case "CABECALHO":
                                {
                                    _relatorio = new RelatorioMultiDesenhos(line);
                                }
                                break;
                            case "VALORES":
                                {
                                    _relatorio.AdicionarPropriedadesDesenho(line);
                                }
                                break;
                            default:
                                break;
                        }

                    }
                }
            }

            return _relatorio;
        }

        
    }
}
