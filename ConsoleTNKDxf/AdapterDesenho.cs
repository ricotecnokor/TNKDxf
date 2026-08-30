using ConsoleTNKDxf.Dgts;
using netDxf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tekla.Structures;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class AdapterDesenho : IAdapterDesenho
    {
        TSM.Model _model;
        string _pastaSaida;
        List<string> _arquivosExistentes = new List<string>();

        public AdapterDesenho(TSM.Model model)
        {
            _model = model;
            string modelPath = _model.GetInfo().ModelPath;

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            _pastaSaida = modelPath + xsplot.Replace(".", "");
        }

        public RespostaModelo ColetarArquivos(string versaoTsep)
        {
            if (!Directory.Exists(_pastaSaida))
            {
                return new RespostaModelo(false, null, "Pasta de saída não encontrada. Verifique se o caminho está correto.");
            }

            _arquivosExistentes = Directory.GetFiles(_pastaSaida, "*.dxf").ToList();

            if (_arquivosExistentes.Count < 1)
            {
                return new RespostaModelo(false, null, "Nenhum arquivo DXF encontrado na pasta de saída. Verifique se os desenhos foram plotados corretamente.");
            }

            TSD.DrawingHandler dh = new TSD.DrawingHandler();

            var dg = dh.GetDrawingSelector().GetSelected();

            while (dg.MoveNext())
            {
                var drawing = dg.Current;
                if (drawing == null) break;

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Identificando tipo de desenho...");
                var processador = AbsProcessador.ObterTipo(_model, drawing);

                if (processador.ToString() == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Tipo do desenho {drawing.Title1} não foi identificado!");
                    Console.ForegroundColor = ConsoleColor.Green;
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Tipo de desenho identificado: " + processador);
                Console.ForegroundColor = ConsoleColor.Green;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Processando o desenho {drawing.Title1}...");
                Console.ForegroundColor = ConsoleColor.Green;

                if (!_arquivosExistentes.Any(a => a.Split('\\').Last().StartsWith(drawing.Title1)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Arquivo para desenho {drawing.Title1} não encontrado. Verifique se o desenho foi plotado corretamente.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    continue;
                }

                if (drawing.GetSheet() == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Desenho {drawing.Title1} não possui folha associada. Verifique o desenho.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    continue;
                }

                if (drawing.GetSheet().GetAllViews().GetSize() < 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Desenho {drawing.Title1} não possui vistas associadas. Verifique o desenho.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    continue;
                }

                if (drawing.GetSheet().GetAllViews().GetEnumerator().MoveNext() == false)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Desenho {drawing.Title1} não possui vistas associadas. Verifique o desenho.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    continue;
                }

                if (processador.CriarLM == "SIM")
                {
                    Console.WriteLine("Cria LM para o desenho");
                }
                else
                {
                    Console.WriteLine("Não cria LM para o desenho");
                }

                if (processador.ListarElementosObra == "SIM")
                {
                    Console.WriteLine("Lista elementos da obra");
                }
                else
                {
                    Console.WriteLine("Não lista elementos da obra");
                }

                string nomeArquivo = _arquivosExistentes.First(a => a.Split('\\').Last().StartsWith(drawing.Title1));
                var dxf = DxfDocument.Load(nomeArquivo);

                ConsoleAnimation.RunWithSpinner(
                                 $"Processando desenho de {processador.Tipo}...",
                                 () => processador.Processar(versaoTsep, dxf)
                             );
                Console.WriteLine("Desenho de montagem processado.");

                Console.WriteLine($"Salvando arquivo dgt...");
                salvarDados(nomeArquivo, dxf);
                Console.WriteLine("Arquivo dgt salvo.");

                dxf = null;
                Console.ForegroundColor = ConsoleColor.Green;
            }

            return new RespostaModelo(true, null, "Informações coletatas.");
        }

        private static void salvarDados(string nomeArquivoProcessado, DxfDocument dxf)
        {
            if (Directory.Exists(nomeArquivoProcessado.Replace(nomeArquivoProcessado.Split('\\').Last(), $"Enviar")) == false)
            {
                Directory.CreateDirectory(nomeArquivoProcessado.Replace(nomeArquivoProcessado.Split('\\').Last(), $"Enviar"));
            }

            var caminhoSalvar = nomeArquivoProcessado.Replace(nomeArquivoProcessado.Split('\\').Last(), $"Enviar\\{nomeArquivoProcessado.Split('\\').Last()}");
            dxf.Save(caminhoSalvar, true);

            var caminhoSalvarR3D = caminhoSalvar.Replace(".dxf", ".dgt");
            if (File.Exists(caminhoSalvarR3D)) File.Delete(caminhoSalvarR3D);
            File.Move(caminhoSalvar, caminhoSalvarR3D);
            File.Delete(caminhoSalvar);

            File.Delete(nomeArquivoProcessado);
        }
    }
}
