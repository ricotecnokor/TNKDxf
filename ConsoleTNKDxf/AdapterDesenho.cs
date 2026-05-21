using ConsoleTNKDxf.Abstracoes;
using ConsoleTNKDxf.Dgts;
using ConsoleTNKDxf.EstruturaDxf;
using netDxf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
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
                                ConsoleAnimation.RunWithSpinner(
                                    $"Processando desenho de montagem...",
                                    () => processarMontagem(versaoTsep, multiDrawing, dxf)
                                );
                                Console.WriteLine("Desenho de montagem processado.");
                            }
                            break;
                        case "DETALHE":
                            {
                                ConsoleAnimation.RunWithSpinner(
                                    $"Processando desenho de detalhes...",
                                    () => processarDetalhe(versaoTsep, multiDrawing, dxf)
                                );
                                Console.WriteLine("Desenho de detalhes processado.");

                            }
                            break;
                        default:
                            {
                                ConsoleAnimation.RunWithSpinner(
                                    $"Processando desenho de detalhes...",
                                    () => processarDetalhe(versaoTsep, multiDrawing, dxf)
                                );
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
            string json = criarJson(dxf, multiDrawing);


            var camposFormato = new CamposFormatoDgt(multiDrawing);
            string prefixoConjunto = int.Parse(camposFormato.Title1.Split('-')[3]).ToString();
            var coletorLm = new LmDetalhesDtg(_model, prefixoConjunto);
            var desenhoDgt = new DesenhoDetalhesDgt(multiDrawing, _model, camposFormato, coletorLm);
            var xDadosFormato = new XDadosFormato<ConjuntoDetalhadoDgt>(dxf, desenhoDgt);
            xDadosFormato.InserirInformacoes(versaoTsep, "DETALHE");
        }

        private string criarJson(DxfDocument dxf, MultiDrawing multiDrawing)
        {
            var viewsNoFormato = new List<TSD.View>();
            var views = multiDrawing.GetSheet().GetAllViews().GetEnumerator();

            while (views.MoveNext())
            {
                var view = views.Current as TSD.View;
                if (view != null)
                {
                    // Obtém a bounding box da view
                    var minPoint = view.Origin;
                    var maxPoint = new Tekla.Structures.Geometry3d.Point(minPoint.X + view.Width, minPoint.Y + view.Height);

                    if(minPoint.X > 0.0 && minPoint.Y > 0.0 && maxPoint.X < 840.0 && maxPoint.Y < 594.0)
                    {
                        if(!viewsNoFormato.Any(v => v.Name == view.Name))
                        {
                            viewsNoFormato.Add(view);
                        }
                    }

                }
            }

            var vistas = new List<Vista>();

            foreach (var view in viewsNoFormato)
            {
                var vista = new Vista();
                coletarLinhasView(view, vista);
                coletarCotas(view, vista);
                coletarMarcas(view, vista);
                coletarPecas(view, vista);

                vistas.Add(vista);
            }

            return string.Empty;
        }

        private void coletarCotas(TSD.View view, Vista vista)
        {
            var cotasL = view.GetObjects(new[] { typeof(TSD.StraightDimension) });
            while (cotasL.MoveNext())
            {
                var cotaTekla = cotasL.Current as TSD.StraightDimension;
                if (cotaTekla != null)
                {
                    var p1 = new Ponto2D(cotaTekla.StartPoint.X, cotaTekla.StartPoint.Y);
                    var p2 = new Ponto2D(cotaTekla.EndPoint.X, cotaTekla.EndPoint.Y);
                    vista.AddCota(new Cota(p1, p2, cotaTekla.Distance));
                }
            }

            var cotasSet = view.GetObjects(new[] { typeof(TSD.StraightDimensionSet) });
            while (cotasSet.MoveNext())
            {
                var cotaConjunto = cotasSet.Current as TSD.StraightDimensionSet;
                if(cotaConjunto != null)
                {
                    // Um StraightDimensionSet contém uma lista de dimensoes individuais.
                    // Se quiser iterar também, você pode usar cotaConjunto.GetObjects() analogamente se for para adicionar como estruturas simples. Opcional mas comum se necessário.
                }
            }
        }

        private void coletarLinhasView(TSD.View view, Vista vista)
        {


            var lines = view.GetObjects(new[] { typeof(TSD.Line) });
            while (lines.MoveNext())
            {
                var linhaTekla = lines.Current as TSD.Line;
                if (linhaTekla != null)
                {
                    var p1 = new Ponto2D(linhaTekla.StartPoint.X, linhaTekla.StartPoint.Y);
                    var p2 = new Ponto2D(linhaTekla.EndPoint.X, linhaTekla.EndPoint.Y);
                    vista.AddLinha(new Linha(p1, p2));
                }
            }
        }

        private void coletarMarcas(TSD.View view, Vista vista)
        {
            var marcas = view.GetObjects(new[] { typeof(TSD.Mark) });
            while (marcas.MoveNext())
            {
                var marcaTekla = marcas.Current as TSD.Mark;
                if (marcaTekla != null)
                {
                    var marca = new Marca();
                    var formas = marcaTekla.GetRelatedObjects();

                    while (formas.MoveNext())
                    {
                        var forma = formas.Current;
                        if (forma is TSD.LeaderLine leaderLine)
                        {
                            var p1 = new Ponto2D(leaderLine.StartPoint.X, leaderLine.StartPoint.Y);
                            var p2 = new Ponto2D(leaderLine.EndPoint.X, leaderLine.EndPoint.Y);
                            marca.AddLinhaLider(new Linha(p1, p2));
                        }
                    }

                    vista.AddMarca(marca);
                }
            }
        }

        private void coletarPecas(TSD.View view, Vista vista)
        {
            var pecas = view.GetObjects(new[] { typeof(TSD.Part) });
            while (pecas.MoveNext())
            {
                var partView = pecas.Current as TSD.Part;
                if (partView != null)
                {
                    var pc = new PecaTekla();
                    var partViewObjetos = partView.GetRelatedObjects();

                    while (partViewObjetos.MoveNext())
                    {
                        var partViewObjeto = partViewObjetos.Current;

                        if (partViewObjeto is TSD.Line linhaPeca)
                        {
                            var p1 = new Ponto2D(linhaPeca.StartPoint.X, linhaPeca.StartPoint.Y);
                            var p2 = new Ponto2D(linhaPeca.EndPoint.X, linhaPeca.EndPoint.Y);
                            pc.AddLinha(new Linha(p1, p2));
                        }
                        else if (partViewObjeto is TSD.Polyline polyPeca)
                        {
                            var pontos = new List<Ponto2D>();
                            foreach (Tekla.Structures.Geometry3d.Point p in polyPeca.Points)
                            {
                                pontos.Add(new Ponto2D(p.X, p.Y));
                            }

                            for (int i = 0; i < pontos.Count - 1; i++)
                            {
                                pc.AddLinha(new Linha(pontos[i], pontos[i + 1]));
                            }
                        }
                    }

                    vista.AddPeca(pc);
                }
            }
        }

        private void processarMontagem(string versaoTsep, MultiDrawing multiDrawing, DxfDocument dxf)
        {
            var camposFormato = new CamposFormatoDgt(multiDrawing);
            var coletorLm = new LmMontagemDgt(_model);
            var desenhoDgt = new DesenhoMontagemDgt(multiDrawing, _model, camposFormato, coletorLm);
            var xDadosFormato = new XDadosFormato<ConjuntoMontagemDgt>(dxf, desenhoDgt);
            xDadosFormato.InserirInformacoes(versaoTsep, "MONTAGEM");
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
