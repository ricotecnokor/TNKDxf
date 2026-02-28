using netDxf;
using netDxf.Entities;
using netDxf.Tables;
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

        string _pastaSaida;


        public AdapterDesenho()
        {

            TSM.Model model = new TSM.Model();
            string modelPath = model.GetInfo().ModelPath;

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            _pastaSaida = modelPath + xsplot.Replace(".", "");
        }


        public void ColetarArquivos()
        {
            var arquivosExistentes = new List<string>();

            if (Directory.Exists(_pastaSaida))
            {
                arquivosExistentes = Directory.GetFiles(_pastaSaida, "*.dxf").ToList();
            }

            if (arquivosExistentes.Count < 1)
            {
                return;
            }

            LeitorRlatorioDesenhosTekla leitor = new LeitorRlatorioDesenhosTekla("multiTemp.rpt");
            var relatorio = leitor.Ler();


            TSD.DrawingHandler dh = new TSD.DrawingHandler();

            var dg = dh.GetDrawingSelector().GetSelected();



            while (dg.MoveNext())
            {
                var drawing = dg.Current;
                if (drawing == null) break;

                var tipo = drawing.GetType();

                if (tipo == typeof(TSD.MultiDrawing))
                {
                    var multid = drawing as TSD.MultiDrawing;
                    var marca = multid.Title1;
                 
                    if (arquivosExistentes.Any(a => a.Split('\\').Last().StartsWith(marca)))
                    {
                        multid.SetUserProperty("TCNM_N_KOCH", marca);
                        inserirInformacoesDesenho(arquivosExistentes.First(a => a.Split('\\').Last().StartsWith(marca)), relatorio);
                        Console.WriteLine(@"Desenho {0} processado com sucesso!", marca);
                    }
                }
            }


        }

        private void inserirInformacoesDesenho(string nomeArquivo, RelatorioMultiDesenhos relatorio)
        {
            var dxf = DxfDocument.Load(nomeArquivo);

            PropriedadesDesenho propriedades = relatorio.PegaPropriedades(nomeArquivo);

            dxf = DxfDocument.Load(nomeArquivo);
            var blocoLista = dxf.Entities.Inserts.FirstOrDefault(x => x.Block.Name.StartsWith("PORTO_SUDESTE_DET_A1"));
            var linhasHorizontais = blocoLista.Block.Entities.OfType<Line>().Where(x => x.StartPoint.Y == x.EndPoint.Y).ToList();
            var linhaHrizontalMaisAlta = linhasHorizontais.OrderByDescending(x => x.StartPoint.Y).FirstOrDefault();

            string appName = "TeklaExport";
            ApplicationRegistry appReg;
            if (!dxf.ApplicationRegistries.Contains(appName))
            {
                appReg = new ApplicationRegistry(appName);
                dxf.ApplicationRegistries.Add(appReg);


                XData xdata = new XData(appReg);

                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];

                xdata.XDataRecord.Add(new XDataRecord(XDataCode.String, userName));
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

                if(Directory.Exists(nomeArquivo.Replace(nomeArquivo.Split('\\').Last(), $"Enviar")) == false)
                {
                    Directory.CreateDirectory(nomeArquivo.Replace(nomeArquivo.Split('\\').Last(), $"Enviar"));
                }

                var caminhoSalvar = nomeArquivo.Replace(nomeArquivo.Split('\\').Last(), $"Enviar\\{nomeArquivo.Split('\\').Last()}");

                if(File.Exists(caminhoSalvar)) File.Delete(caminhoSalvar);
                
                dxf.Save(caminhoSalvar);

                File.Delete(nomeArquivo);
            }
            else
            {
                appReg = dxf.ApplicationRegistries[appName];
            }




        }

    }
}
