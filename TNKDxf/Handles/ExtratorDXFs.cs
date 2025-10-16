using Dynamic.Tekla.Structures;
using Dynamic.Tekla.Structures.Model.Operations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TSD = Dynamic.Tekla.Structures.Drawing;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf.Handles
{
    public class ExtratorDXFs 
    {
        List<string> _desenhos;
        bool _foramExtraidos = false;

        private static ExtratorDXFs _instance;

        private ExtratorDXFs()
        {
            _desenhos = new List<string>();
        }

        public static ExtratorDXFs GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ExtratorDXFs();
            }
            return _instance;
        }

  
        public IEnumerable<object> Desenhos { get; internal set; }
        public IEnumerable<string> Extraidos => _desenhos;

    
        public void Extrair()
        {
            if (_foramExtraidos)
                return;

            var appFolder = TeklaStructuresInfo.GetLocalAppDataFolder();

            var versao = appFolder.Split('\\').Last();

            TSD.DrawingHandler dh = new TSD.DrawingHandler();

            TSM.Model model = new TSM.Model();
            string modelPath = model.GetInfo().ModelPath;

        
            var dg = dh.GetDrawingSelector().GetSelected();

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            var caminhoArquivos = modelPath + xsplot.Replace(".", "");
            var _xsplot = modelPath + xsplot.Replace(".", "");

            var files = Directory.GetFiles(_xsplot); //Directory.GetFiles(caminhoArquivos);

            foreach ( var file in files )
            {
                if(!file.EndsWith("Thumbs.db"))
                File.Delete(file);
            }

            int count = 0;
            while (dg.MoveNext())
            {
                var drawing = dg.Current;

                if (drawing == null)
                    break;

                var tipo = drawing.GetType();

                if (tipo == typeof(TSD.SinglePartDrawing))
                {
                    var spd = drawing as TSD.SinglePartDrawing;

                    var marca = spd.Mark.Replace("[", "").Replace("]", "");
                    spd.SetUserProperty("TCNM_N_KOCH", marca);
                    _desenhos.Add(marca);
                }

                if (tipo == typeof(TSD.AssemblyDrawing))
                {
                    var assd = drawing as TSD.AssemblyDrawing;

                    var marca = assd.Mark.Replace("[", "").Replace("]", "");
                    assd.SetUserProperty("TCNM_N_KOCH", marca);

                    _desenhos.Add(marca);

                }

                if (tipo == typeof(TSD.GADrawing))
                {
                    var gad = drawing as TSD.GADrawing;

                    var marca = gad.Title1;
                    _desenhos.Add(marca);

                }

                if (tipo == typeof(TSD.MultiDrawing))
                {
                    var multid = drawing as TSD.MultiDrawing;

                    var marca = multid.Title1;
                    multid.SetUserProperty("TCNM_N_KOCH", marca);
                    _desenhos.Add(marca);
                }

            }

            if (versao == "2024.0")
            {
                Operation.RunMacro(@"C:\ProgramData\Trimble\Tekla Structures\2024.0\Environments\common\macros\modeling\ExportaDxf.cs");
                _foramExtraidos = true;
            }

            _foramExtraidos = true;

        }
    }
}
