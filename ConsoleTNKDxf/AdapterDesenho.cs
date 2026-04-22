using ConsoleTNKDxf.Dgts;
using netDxf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class AdapterDesenho : IAdapterDesenho
    {
        TSM.Model _model;
        string _pastaSaida;
        List<string> _arquivosExistentes = new List<string>();
        //private RelatorioMultiDesenhos _relatorio;
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

            //LeitorRlatorioDesenhosTekla leitor = new LeitorRlatorioDesenhosTekla("multiTemp.rpt");
            //_relatorio = leitor.Ler();


            TSD.DrawingHandler dh = new TSD.DrawingHandler();

            var dg = dh.GetDrawingSelector().GetSelected();

            int qtd = dg.GetSize();

            while (dg.MoveNext())
            {
                var drawing = dg.Current;
                if (drawing == null) break;

                // LayoutInspector layoutInspector = new LayoutInspector();
                //bool isDiagrama = layoutInspector.IsDiagramaDrawing(drawing);

                var tipo = drawing.GetType();

                if (tipo == typeof(TSD.MultiDrawing))
                {

                    var multiDrawing = drawing as TSD.MultiDrawing;

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Processando o desenho {multiDrawing.Title1}...");
                    Console.ForegroundColor = ConsoleColor.Green;


                    if (!_arquivosExistentes.Any(a => a.Split('\\').Last().StartsWith(multiDrawing.Title1)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Arquivo para desenho {multiDrawing.Title1} não encontrado. Verifique se o desenho foi plotado corretamente.");
                        Console.ForegroundColor = ConsoleColor.Green;
                        continue;
                    }

                    if (multiDrawing.GetSheet() == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Desenho {multiDrawing.Title1} não possui folha associada. Verifique o desenho.");
                        Console.ForegroundColor = ConsoleColor.Green;
                        continue;
                    }

                    if (multiDrawing.GetSheet().GetAllViews().GetSize() < 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Desenho {multiDrawing.Title1} não possui vistas associadas. Verifique o desenho.");
                        Console.ForegroundColor = ConsoleColor.Green;
                        continue;
                    }

                    if (multiDrawing.GetSheet().GetAllViews().GetEnumerator().MoveNext() == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Desenho {multiDrawing.Title1} não possui vistas associadas. Verifique o desenho.");
                        Console.ForegroundColor = ConsoleColor.Green;
                        continue;
                    }



                    string nomeArquivo = _arquivosExistentes.First(a => a.Split('\\').Last().StartsWith(multiDrawing.Title1));
                    var dxf = DxfDocument.Load(nomeArquivo);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Identificando tipo de desenho...");
                    TipoDesenho tipoDesenho = new TipoDesenho(multiDrawing);

                    string tipoIdentificado = tipoDesenho.Tipo != null ? tipoDesenho.Tipo : "DETALHE";
                    Console.WriteLine("Tipo de desenho identificado: " + tipoIdentificado);

                    if (tipoDesenho.CriarLM == "SIM")
                    {
                        Console.WriteLine("Cria LM para o desenho");
                    }
                    else
                    {
                        Console.WriteLine("Não cria LM para o desenho");
                    }

                    if (tipoDesenho.ListarElementosObra == "SIM")
                    {
                        Console.WriteLine("Lista elementos da obra");
                    }
                    else
                    {
                        Console.WriteLine("Não lista elementos da obra");
                    }


                    switch (tipoIdentificado)
                    {
                        case "MONTAGEM":
                            {
                                Console.WriteLine($"Processando desenho de montagem...");
                                processarMontagem(versaoTsep, multiDrawing, dxf);
                                Console.WriteLine("Desenho de montagem processado.");
                            }
                            break;
                        case "DETALHE":
                            {
                                Console.WriteLine($"Processando desenho de detalhes...");
                                processarDetalhe(versaoTsep, multiDrawing, dxf);
                                Console.WriteLine("Desenho de detalhes processado.");
                            }
                            break;
                        default:
                            {
                                Console.WriteLine($"Processando desenho de detalhes...");
                                processarDetalhe(versaoTsep, multiDrawing, dxf);
                                Console.WriteLine("Desenho de detalhes processado.");
                            }
                            break;
                    }





                    Console.WriteLine($"Salvando arquivo dgt...");
                    salvarDados(nomeArquivo, dxf);
                    Console.WriteLine("Arquivo dgt salvo.");
                    dxf = null;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
            return new RespostaModelo(true, null, "Informações coletatas.");

            //return new RespostaModelo(true, _model, "Processamento concluído com sucesso.");

        }

        private void processarDetalhe(string versaoTsep, MultiDrawing multiDrawing, DxfDocument dxf)
        {
            var camposFormato = new CamposFormatoDgt(multiDrawing);
            string prefixoConjunto = int.Parse(camposFormato.Title1.Split('-')[3]).ToString();
            var coletorLm = new LmDetalhesDtg(_model, prefixoConjunto);
            Console.WriteLine($"Definindo desenho...");
            var desenhoDgt = new DesenhoDetalhesDgt(multiDrawing, _model, camposFormato, coletorLm);
            Console.WriteLine($"Preparando arquivo dgt...");
            var xDadosFormato = new XDadosFormato<ConjuntoDetalhadoDgt>(dxf, desenhoDgt);
            xDadosFormato.InserirInformacoes(versaoTsep, "DETALHE");
            Console.WriteLine($"Arquivo dgt de {multiDrawing.Title1} definido.");
        }

        private void processarMontagem(string versaoTsep, MultiDrawing multiDrawing, DxfDocument dxf)
        {
            var camposFormato = new CamposFormatoDgt(multiDrawing);
            var coletorLm = new LmMontagemDgt(_model);
            Console.WriteLine($"Definindo desenho...");
            var desenhoDgt = new DesenhoMontagemDgt(multiDrawing, _model, camposFormato, coletorLm);
            Console.WriteLine($"Preparando arquivo dgt...");
            var xDadosFormato = new XDadosFormato<ConjuntoMontagemDgt>(dxf, desenhoDgt);
            xDadosFormato.InserirInformacoes(versaoTsep, "MONTAGEM");
            Console.WriteLine($"Arquivo dgt de {multiDrawing.Title1} definido.");
        }





        //private Desenho coletarDesenho(TSD.MultiDrawing multiDrawing, string nomeArquivo)
        //{



        //    var desenho = new Desenho(multiDrawing, _model, nomeArquivo);

        //    return desenho;
        //    //HashSet<Identifier> pecasUnicasNoDesenho = obterPecasUnicasDesenho(multiDrawing);

        //    //foreach (Identifier partId in pecasUnicasNoDesenho)
        //    //{
        //    //    var modelObj = _model.SelectModelObject(partId);

        //    //    if (modelObj is TSM.Part modelPart)
        //    //    {
        //    //        desenho.AddPeca(modelPart);
        //    //    }
        //    //}



        //}




        private HashSet<Identifier> obterPecasUnicasDesenho(MultiDrawing multiDrawing)
        {
            // Usamos um HashSet para garantir que cada INSTÂNCIA física (GUID único) 
            // seja contada apenas uma vez, mesmo que apareça em várias vistas (Frontal, Topo, etc)
            HashSet<Identifier> pecasUnicasNoDesenho = new HashSet<Identifier>();

            // 1. Acessar as vistas do Multi-drawing
            var views = multiDrawing.GetSheet().GetAllViews().GetEnumerator();
            while (views.MoveNext())
            {

                var view = views.Current as TSD.View;
                if (view == null) continue;

                // 2. Pegar todas as partes gráficas nesta vista
                DrawingObjectEnumerator drawingParts = view.GetObjects(new[] { typeof(TSD.Part) });
                while (drawingParts.MoveNext())
                {
                    TSD.Part drwPart = drawingParts.Current as TSD.Part;
                    if (drwPart != null)
                    {
                        pecasUnicasNoDesenho.Add(drwPart.ModelIdentifier);
                    }
                }
            }

            return pecasUnicasNoDesenho;
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
