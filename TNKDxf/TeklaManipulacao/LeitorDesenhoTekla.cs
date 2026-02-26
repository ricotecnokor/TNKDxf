using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using TSD = Tekla.Structures.Drawing;

namespace TNKDxf.TeklaManipulacao
{
    public class LeitorDesenhoTekla
    {
        private TSD.DrawingHandler _drawingHandler;

        public LeitorDesenhoTekla()
        {
            _drawingHandler = new TSD.DrawingHandler();
        }

        public void LerDesenhos()
        {


            string nomeRelatorio = Guid.NewGuid().ToString("N").Substring(0, 8);


            string caminhoSalvarTemplate = string.Empty;

            

            var arquivoRelatorio = new ArquivoRelatorioMultiDesenhos("TNKDxfListaMULTI.rpt");
            arquivoRelatorio.CriarArquivoRelatorio();

            LeitorRelatorio leitorRelatorio = new LeitorRelatorio(
                new LeitorRelatorioParams(
                    //@"C:\APP\TNKDxf  Lista de multi desenhos.rpt",
                    arquivoRelatorio.Caminho,
                    @"C:\APP\" + nomeRelatorio + ".xsr"));

            var relatorio = leitorRelatorio.Ler();

            arquivoRelatorio.DeleteArquivo("TNKDxfListaMULTI.rpt");


            //var desenhos = _drawingHandler.GetDrawings();


            //while (desenhos.MoveNext())
            //{
            //        var desenho = desenhos.Current as TSD.Drawing;

            //      var tipo =  desenho.GetType(); 

            //    if(tipo == typeof(TSD.MultiDrawing))
            //    {
            //        //pegar propriedades do desenho
            //        var multi = desenho as TSD.MultiDrawing;
            //        var title1 = multi.Title1;
            //        var title2 = multi.Title2;
            //        var title3 = multi.Title3;
            //        var nome = multi.Name;

            //        //pegar propriedades do layout
            //        var layout = multi.Layout;

            //        LayoutAttributes layoutAttributes = layout.LoadAttributes();



            //    }


            //}



        }
    }
}
