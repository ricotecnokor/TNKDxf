using System.Collections.Generic;
using System.IO;
using Tekla.Structures.Model.Operations;

namespace TNKDxf.TeklaManipulacao
{
    public class LeitorRelatorio
    {

        private LeitorRelatorioParams _parametros;
        private RelatorioMultiDesenhos _relatorio;


        public LeitorRelatorio(LeitorRelatorioParams parametros)
        {

            _parametros = parametros;
        }

        public RelatorioMultiDesenhos Ler()
        {
            List<string[]> linhas = new List<string[]>();
            Operation.CreateReportFromAll(
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
                                    _relatorio = new RelatorioMultiDesenhos(linha[1], linha[2], linha[3], linha[4]);
                                }
                                break;
                            case "VALORES":
                                {
                                    _relatorio.AdicionarPropriedadesDesenho(new PropriedadesDesenho
                                    (
                                        linha[1].Trim(),
                                        linha[2].Trim(),
                                        linha[3].Trim(),
                                        linha[4].Trim(),
                                        linha[5].Trim(),
                                        linha[6].Trim(),
                                        linha[7].Trim(),
                                        linha[8].Trim(),
                                        linha[9].Trim()    
                                    ));
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

    public class LeitorRelatorioParams
    {
        public LeitorRelatorioParams(string template, string filename, string title = "", string subtitle = "", string footer = "")
        {
            Template = template;
            Filename = filename;
            Title = title;
            Subtitle = subtitle;
            Footer = footer;
        }

        public string Template { get; set; }
        public string Filename { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Footer { get; set; }

    }
}
