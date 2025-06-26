using Dynamic.Tekla.Structures;
using Dynamic.Tekla.Structures.Model.Operations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TSD = Dynamic.Tekla.Structures.Drawing;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf.Handles
{
    public class HandleCriacaoDxfs
    {
        public List<string> CriarDxfs()
        {
            var desenhos = new List<string>();

            var appFolder = TeklaStructuresInfo.GetLocalAppDataFolder(); 

            var versao = appFolder.Split('\\').Last(); 

            TSD.DrawingHandler _dh = new TSD.DrawingHandler();

            TSM.Model model = new TSM.Model();
            string modelPath = model.GetInfo().ModelPath;

            string ApplicationName = "Dwg.exe";

            string TSBinaryDir = "";
            TeklaStructuresSettings.GetAdvancedOption("XSBIN", ref TSBinaryDir);

            string dwgExePath = Path.Combine(TSBinaryDir, "Applications\\Tekla\\Drawings\\DwgExport\\" + ApplicationName);

            string configPath = "\"C:\\Configs\\dwgExportConfig.xml\"";

            var dg = _dh.GetDrawingSelector().GetSelected();

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
                    desenhos.Add(marca);
                }

                if (tipo == typeof(TSD.AssemblyDrawing))
                {
                    var assd = drawing as TSD.AssemblyDrawing;

                    var marca = assd.Mark.Replace("[", "").Replace("]", "");
                    assd.SetUserProperty("TCNM_N_KOCH", marca);

                    desenhos.Add(marca);

                }

                if (tipo == typeof(TSD.GADrawing))
                {
                    var gad = drawing as TSD.GADrawing;

                    var marca = gad.Title1;
                    desenhos.Add(marca);

                }

                if (tipo == typeof(TSD.MultiDrawing))
                {
                    var multid = drawing as TSD.MultiDrawing;

                    var marca = multid.Title1;
                    multid.SetUserProperty("TCNM_N_KOCH", marca);
                    desenhos.Add(marca);
                }

            }

            if (versao == "2024.0")
            {
                Operation.RunMacro(@"C:\ProgramData\Trimble\Tekla Structures\2024.0\Environments\common\macros\modeling\ExportaDxf.cs"); ;
            }

            return desenhos;
        }

       
    }
}
