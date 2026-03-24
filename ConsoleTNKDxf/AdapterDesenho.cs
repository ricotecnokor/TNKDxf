using ConsoleTNKDxf.Dgts;
using netDxf;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Drawing;
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


        public RespostaModelo ColetarArquivos()
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



            while (dg.MoveNext())
            {
                var drawing = dg.Current;
                if (drawing == null) break;

                var tipo = drawing.GetType();

                if (tipo == typeof(TSD.MultiDrawing))
                {

                    var multiDrawing = drawing as TSD.MultiDrawing;


                    if (_arquivosExistentes.Any(a => a.Split('\\').Last().StartsWith(multiDrawing.Title1)))
                    {
                        var desenhoDgt = new DesenhoDgt(multiDrawing, _model);
                        string nomeArquivo = _arquivosExistentes.First(a => a.Split('\\').Last().StartsWith(multiDrawing.Title1));
                        var dxf = DxfDocument.Load(nomeArquivo);
                        XDadosFormato xDadosFormato = new XDadosFormato(dxf, desenhoDgt);
                        xDadosFormato.InserirInformacoes();
                        salvarDados(nomeArquivo, dxf);
                        dxf = null;
                    }
                    return new RespostaModelo(true, _model, "Processamento concluído com sucesso.");

                }
            }

            return new RespostaModelo(true, null, "Informações coletatas.");


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
