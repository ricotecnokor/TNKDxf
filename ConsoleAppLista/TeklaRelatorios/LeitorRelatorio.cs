using Dynamic.Tekla.Structures.Model.Operations;
using System.Collections.Generic;
using System.IO;



namespace ConsoleAppLista.TeklaRelatorios
{
    public class LeitorRelatorio
    {

        private LeitorRelatorioParams _parametros;

        public LeitorRelatorio(LeitorRelatorioParams parametros)
        {

            _parametros = parametros;
        }

        public List<string[]> Ler()
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
                System.IO.StreamReader sr = new System.IO.StreamReader(_parametros.Filename, System.Text.Encoding.Default);
                string line = " ";
                while ((line = sr.ReadLine()) != null)
                {
                    var linha = line.Split(';');
                    if (linha.Length > 1)
                    {
                        linhas.Add(linha);
                    }
                }
            }

            return linhas;
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
