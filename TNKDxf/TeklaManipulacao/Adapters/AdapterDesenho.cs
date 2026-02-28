using netDxf;
using netDxf.Collections;
using netDxf.Entities;
using netDxf.Tables;
using System;
using System.IO;
using System.Linq;
using TSD = Tekla.Structures.Drawing;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class AdapterDesenho : IAdapterDesenho
    {

        private TSD.DrawingHandler _drawingHandler;
        private DxfDocument _dxf;

        public AdapterDesenho()
        {
            _drawingHandler = new TSD.DrawingHandler();
            _dxf = new DxfDocument();
            _dxf.DrawingVariables.InsUnits = netDxf.Units.DrawingUnits.Millimeters;

        }

        public void ColetarInformacoesDesenho()
        {
            var desenhos = _drawingHandler.GetDrawings();

            while (desenhos.MoveNext())
            {
                string titulo = desenhos.Current.Title1; 
                if (titulo == "PRJ-00019-D-00124")
                {
                    if (desenhos.Current is TSD.MultiDrawing multi)
                    {

                        bool encontrouConteudoRelevante = false;
                        var sheet = multi.GetSheet();
                        var views = sheet.GetAllViews();

                        var qtdVistas = views.GetSize();
                        //var listaVistas = new List<TSD.View>();
                        //while (views.MoveNext())
                        //{
                        //    if (views.Current is TSD.View view)
                        //    {
                        //        if(view.FrameOrigin.X > 0 && view.FrameOrigin.Y > 0 && view.FrameOrigin.X < 841 && view.FrameOrigin.Y < 594)
                        //        {
                        //            listaVistas.Add(view);
                        //        }

                        //    }
                        //}

                        //int qtdVistasRelevantes = listaVistas.Count;
                        //foreach (var view in listaVistas)
                        //{
                        //    if (view.GetObjects().GetSize() > 0)
                        //    {
                        //        encontrouConteudoRelevante = true;
                        //        processarVista(view);
                        //    }
                        //}

                        while (views.MoveNext())
                        {
                            if (views.Current is TSD.View view)
                            {
                                processarVista(view);

                            }
                        }


                        if (File.Exists($"C:\\APP\\{titulo}.dxf"))
                        {
                            File.Delete($"C:\\APP\\{titulo}.dxf");
                        }

                        _dxf.Save($"C:\\APP\\{titulo}.dxf");
                    }
                    break;
                }
            }
        }

        public void InserirInformacoesDesenho(string nomeArquivo, RelatorioMultiDesenhos relatorio)
        {

            PropriedadesDesenho propriedades = relatorio.PegaPropriedades(nomeArquivo);

            _dxf = DxfDocument.Load(nomeArquivo);
            var blocoLista = _dxf.Entities.Inserts.FirstOrDefault(x => x.Block.Name.StartsWith("PORTO_SUDESTE_DET_A1"));
            var linhasHorizontais = blocoLista.Block.Entities.OfType<Line>().Where(x => x.StartPoint.Y == x.EndPoint.Y).ToList();
            var linhaHrizontalMaisAlta = linhasHorizontais.OrderByDescending(x => x.StartPoint.Y).FirstOrDefault();

            string appName = "TeklaExport";
            ApplicationRegistry appReg;
            if (!_dxf.ApplicationRegistries.Contains(appName))
            {
                appReg = new ApplicationRegistry(appName);
                _dxf.ApplicationRegistries.Add(appReg);


                XData xdata = new XData(appReg);

                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, relatorio.NomeModelo));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, relatorio.NumeroProjeto));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, relatorio.Data.ToString()));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.NumeroContratada));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.NumeroCliente));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.DescricaoProjeto));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.AreaSubarea));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.TituloDesenho));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.Subtitulo1Desenho));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.Subtitulo2Desenho));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.Revisao));
                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, propriedades.RevisaoCliente));

                linhaHrizontalMaisAlta.XData.Add(xdata);

                var xdataRecuperada = linhaHrizontalMaisAlta.XData[appName];

                _dxf.Save(nomeArquivo);
            }
            else
            {
                appReg = _dxf.ApplicationRegistries[appName];
            }

            


        }

        private void processarVista(TSD.View view)
        {

            var pontoOrigem = view.Origin;

            var objetos = view.GetObjects();

            int qtdObjetos = objetos.GetSize();

            while (objetos.MoveNext())
            {
                var objeto = objetos.Current;

                if (objeto is TSD.Part peca)
                {
                    new PecaDxf(_dxf).Processar(peca, view);
                }

                if (objeto is TSD.StraightDimension stDimension)
                {
                    new CotaLinearDxf(_dxf).Processar(stDimension, view);
                }

                else if (objeto is TSD.StraightDimensionSet cotaConjunto)
                {
                    new CotaConjuntoDxf(_dxf).Processar(cotaConjunto, view);
                }

                else if (objeto is TSD.Line linha)
                {
                    new LinhaDxf(_dxf).Processar(linha, view);
                }

                else if (objeto is TSD.Polyline poly)
                {
                    new PolilinhaDxf(_dxf).Processar(poly, view);
                }

                else if (objeto is TSD.Text texto)
                {
                    new TextoDxf(_dxf).Processar(texto, view);
                }

                else if (objeto is TSD.Arc arco)
                {
                    new ArcoDxf(_dxf).Processar(arco, view);
                }

                else if (objeto is TSD.Circle circulo)
                {
                    new CirculoDxf(_dxf).Processar(circulo, view);
                }


            }
        }












    }

}




