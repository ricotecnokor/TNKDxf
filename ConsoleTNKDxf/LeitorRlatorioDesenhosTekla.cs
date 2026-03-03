using System;
using System.Collections.Generic;
using System.IO;
using Tekla.Structures.Model.Operations;

namespace ConsoleTNKDxf
{
    public class LeitorRlatorioDesenhosTekla
    {
     
        private RelatorioMultiDesenhos _relatorio;
        private ArquivoRelatorioMultiDesenhos _arquivoRelatorio;

        public LeitorRlatorioDesenhosTekla(string nome)
        {
            _arquivoRelatorio = new ArquivoRelatorioMultiDesenhos(nome);
            _arquivoRelatorio.CriarTemplateRelatorio();
        }

        public RelatorioMultiDesenhos Ler()
        {
            _arquivoRelatorio.DeleteRelatorios();
            var relatorio = criar();
            _arquivoRelatorio.DeleteTemplate(_arquivoRelatorio.Template);
           
            return relatorio;
        }

        public RelatorioMultiDesenhos criar()
        {
            List<string[]> linhas = new List<string[]>();
            Operation.CreateReportFromSelected(
                _arquivoRelatorio.Template,
                _arquivoRelatorio.FilenameRelatorio,
                _arquivoRelatorio.TitleRelatorio,
                _arquivoRelatorio.SubtitleRelatorio,
                _arquivoRelatorio.FooterRelatorio);

            if (File.Exists(_arquivoRelatorio.FilenameRelatorio))
            {
                StreamReader sr = new StreamReader(_arquivoRelatorio.FilenameRelatorio, System.Text.Encoding.Default);
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
