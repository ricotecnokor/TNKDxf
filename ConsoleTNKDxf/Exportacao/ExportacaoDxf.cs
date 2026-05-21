using ConsoleTNKDxf.Exportacao;
using ConsoleTNKDxf.Exportacao.AnnotationExport;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text.RegularExpressions;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.DrawingInternal;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;
using TSO = Tekla.Structures.Model.Operations;

namespace ConsoleTNKDxf
{
    public static class ExportacaoDxf
    {
        public static void Exportar()
        {
            TSM.Model model = new TSM.Model();
            string modelPath = model.GetInfo().ModelPath;

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            var destino = modelPath + xsplot.Replace(".", "");


            if (Directory.Exists(destino))
            {
                try
                {
                    var arquivosExistentes = Directory.GetFiles(destino, "*.dxf");
                    foreach (var arquivo in arquivosExistentes)
                    {
                        try
                        {
                            File.Delete(arquivo);
                        }
                        catch { }
                    }
                }
                catch { }
            }



            TSM.Operations.Operation.DisplayPrompt("Exporting DWG Files.");

            string TSBinaryDir = "";

            TSM.Model CurrentModel = new TSM.Model();

            TeklaStructuresSettings.GetAdvancedOption("XSBIN", ref TSBinaryDir);


            string ApplicationName = "Dwg.exe";

            string ApplicationPath = Path.Combine(TSBinaryDir, "Applications\\Tekla\\Drawings\\DwgExport\\" + ApplicationName);

            string dwgxportParams = "export outputDirectory=\"" + destino + "\"";


            Process NewProcess = new Process();


            if (File.Exists(ApplicationPath))
            {

                NewProcess.StartInfo.FileName = ApplicationPath;


                try

                {

                    NewProcess.StartInfo.Arguments = dwgxportParams;

                    NewProcess.Start();

                    NewProcess.WaitForExit();

                }

                catch

                {

                    TSO.Operation.DisplayPrompt(ApplicationName + " failed to start.");

                }

            }

            else

            {

                TSO.Operation.DisplayPrompt(ApplicationName + " not found.");

            }

            TSM.Operations.Operation.DisplayPrompt("DWG Files Exported.");

        }
    
        public static void ExportarProgramaticamente()
        {
            try
            {
                string[] args =new string[]{"export"};



                //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                CommandOptions cmdOptions = new CommandOptions
                {
                    OutputDirectory = GetPlotDirectory(),
                    Name = "export"
                };
                if (ParseArguments(args, ref cmdOptions) && cmdOptions.Name == "export")
                {
                    Export(cmdOptions);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        private static string GetPlotDirectory()
        {
            //IL_0000: Unknown result type (might be due to invalid IL or missing references)
            string modelPath = new Model().GetInfo().ModelPath;
            string empty = string.Empty;
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref empty);
            if (Path.IsPathRooted(empty))
            {
                return empty;
            }
            return Path.GetFullPath(Path.Combine(modelPath, empty)) + "\\";
        }

        private static bool ParseArguments(string[] args, ref CommandOptions cmdOptions)
        {
            bool flag = false;
            if (cmdOptions == null)
            {
                cmdOptions = new CommandOptions();
            }
            if (args.Length != 0)
            {
                cmdOptions.Name = args[0].ToLower();
                if (cmdOptions.Name == "export")
                {
                    flag = ParseExportArguments(args, ref cmdOptions);
                }
                else if (cmdOptions.Name == "settingfiles")
                {
                    ListSettings();
                    flag = true;
                }
            }
            if (!flag)
            {
                PrintInstructions();
            }
            return flag;
        }

        private static void ListSettings()
        {
            //IL_0000: Unknown result type (might be due to invalid IL or missing references)
            foreach (string item in new Settings().SettingFiles())
            {
                Console.WriteLine(item);
            }
        }


        private static void PrintInstructions()
        {
            Console.WriteLine("\nUnknown command, supported commands are:");
            Console.WriteLine("");
            Console.WriteLine("export [option1 [option2 [... optionN]]]");
            Console.WriteLine("");
            Console.WriteLine(" Description: exports drawing which is currently open");
            Console.WriteLine(" Example : dwg export outputDirectory=. settingFile=myFile onlyAnnotations");
            Console.WriteLine(" Supported options");
            Console.WriteLine("  outputDirectory - specifies output directory");
            Console.WriteLine("  settingFile - specifies used settings");
            Console.WriteLine("  onlyAnnotations - flag to create only annotation drawings");
            Console.WriteLine("");
            Console.WriteLine("settingFiles");
            Console.WriteLine("");
            Console.WriteLine(" Description: Lists available setting files");
            Console.WriteLine(" Example : dwg settingFiles");
            Console.WriteLine(" Supported options - none");
        }

        private static bool ParseExportArguments(string[] args, ref CommandOptions cmdOptions)
        {
            bool result = true;
            for (int i = 1; i < args.Length; i++)
            {
                string text = args[i];
                if (text.StartsWith("outputDirectory="))
                {
                    cmdOptions.OutputDirectory = text.Substring("outputDirectory=".Length);
                    continue;
                }
                if (text.StartsWith("fileExtension="))
                {
                    cmdOptions.FileExtension = text.Substring("fileExtension=".Length);
                    continue;
                }
                if (text.StartsWith("settingFile="))
                {
                    cmdOptions.SettingsFile = text.Substring("settingFile=".Length);
                    continue;
                }
                switch (text)
                {
                    case "onlyAnnotations":
                        cmdOptions.OnlyAnnotations = true;
                        continue;
                    case "exportViewPictures":
                        cmdOptions.ExportViewPictures = true;
                        continue;
                    case "exportViewsSeparately3D":
                        cmdOptions.Export3dAnnotationsAsSeparateViews = true;
                        continue;
                }
                if (text.StartsWith("viewFilenameFormat="))
                {
                    cmdOptions.ExportViewPicturesFilenameFormat = text.Substring("viewFilenameFormat=".Length);
                }
                else if (text.StartsWith("viewIds="))
                {
                    string text2 = text.Substring("viewIds=".Length);
                    cmdOptions.TargetViewIdsPerDrawingGuid = new Dictionary<string, HashSet<int>>();
                    string[] array = text2.Split(',');
                    for (int j = 0; j < array.Length; j++)
                    {
                        ParseDrawingGuidAndViewId(array[j], cmdOptions.TargetViewIdsPerDrawingGuid);
                    }
                }
                else if (text.StartsWith("viewIdsFile="))
                {
                    cmdOptions.TargetViewIdsPerDrawingGuid = new Dictionary<string, HashSet<int>>();
                    string path = text.Substring("viewIdsFile=".Length);
                    if (File.Exists(path))
                    {
                        string[] array = File.ReadAllLines(path);
                        for (int j = 0; j < array.Length; j++)
                        {
                            ParseDrawingGuidAndViewId(array[j], cmdOptions.TargetViewIdsPerDrawingGuid);
                        }
                    }
                }
                else if (string.Equals(text, "useTrbForAnnotations", StringComparison.InvariantCultureIgnoreCase))
                {
                    cmdOptions.UseTrbForAnnotations = true;
                }
                else if (text.StartsWith("annotationPipe="))
                {
                    cmdOptions.AnnotationPipe = text.Substring("annotationPipe=".Length);
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }

        private static void ParseDrawingGuidAndViewId(string guidAndId, IDictionary<string, HashSet<int>> targetViewIdsPerDrawingGuid)
        {
            string[] array = guidAndId.Split(':');
            bool flag = true;
            string key = string.Empty;
            int result = 0;
            if (array.Length == 1)
            {
                flag = int.TryParse(array[0], out result);
            }
            else if (array.Length >= 2)
            {
                key = array[0];
                flag = int.TryParse(array[1], out result);
            }
            if (flag)
            {
                if (!targetViewIdsPerDrawingGuid.ContainsKey(key))
                {
                    targetViewIdsPerDrawingGuid.Add(key, new HashSet<int>());
                }
                targetViewIdsPerDrawingGuid[key].Add(result);
            }
        }

        public static void Export(CommandOptions cmdOptions, TextWriter annotationStream = null)
        {
            //IL_00a2: Unknown result type (might be due to invalid IL or missing references)
            //IL_00a9: Expected O, but got Unknown
            //IL_00b7: Unknown result type (might be due to invalid IL or missing references)
            //IL_00be: Expected O, but got Unknown
            //IL_0115: Unknown result type (might be due to invalid IL or missing references)
            //IL_011c: Expected O, but got Unknown
            string outputDirectory = cmdOptions.OutputDirectory;
            string settingsFile = cmdOptions.SettingsFile;
            bool onlyAnnotations = cmdOptions.OnlyAnnotations;
            bool exportViewPictures = cmdOptions.ExportViewPictures;
            bool export3dAnnotationsAsSeparateViews = cmdOptions.Export3dAnnotationsAsSeparateViews;
            bool useTrbForAnnotations = cmdOptions.UseTrbForAnnotations;
            string realPath = Settings.GetRealPath(outputDirectory);
            Console.Write(outputDirectory);
            Console.Write("\n" + settingsFile);
            if (onlyAnnotations || export3dAnnotationsAsSeparateViews)
            {
                AppDomain.CurrentDomain.SetData("USE_ONLY_ANNOTATIONS", true);
            }
            //DwgDatabaseUpdater.RunningInConsole = true;
            //DgnDatabaseUpdater.RunningInConsole = true;
            AnnotationExportData annotationExportData = null;
            if (export3dAnnotationsAsSeparateViews)
            {
                annotationExportData = new AnnotationExportData();
                annotationExportData.Setup(cmdOptions.AnnotationPipe, realPath, annotationStream);
            }
            RulesViewModel val = new RulesViewModel();
            val.Init();
            ExportViewModel val2 = new ExportViewModel(new DirectoryInfo(realPath));
            if (cmdOptions.TargetViewIdsPerDrawingGuid != null && cmdOptions.TargetViewIdsPerDrawingGuid.Count > 0)
            {
                HashSet<string> guids = Extensions.ToHashSet<string>((IEnumerable<string>)cmdOptions.TargetViewIdsPerDrawingGuid.Keys);
                val2.ActiveDrawings = GetDrawingByIds(guids);
            }
            val2.FileExtension = (string.IsNullOrWhiteSpace(cmdOptions.FileExtension) ? ".DWG" : cmdOptions.FileExtension);
            Settings val3 = new Settings();
            string settingsFileFromInput = GetSettingsFileFromInput(val3, settingsFile);
            bool flag;
            string text;
            if (!string.IsNullOrEmpty(settingsFileFromInput))
            {
                (flag, text) = val3.Load(settingsFileFromInput);
            }
            else
            {
                string empty = string.Empty;
                text = empty;
                flag = false;
            }
            if (flag)
            {
                annotationExportData?.DwgUpdateEvents?.LogFormat("Export to DWG using '{0}' settings file.", text);
                val2.FileExtension = val3.FileExtension;
                val2.FilePrefix = val3.FilePrefix;
                val2.FileSuffix = val3.FileSuffix;
                val2.FileVersion = val3.FileVersion;
                val2.OpenFolderWhenDone = val3.OpenFolderWhenDone;
                val2.SnapshotToModelSpace = val3.SnapshotToModelSpace;
                val2.ScaleValue = val3.SnapshotScale;
                val2.EmbedImages = val3.EmbedImages;
                val2.WithoutBlocks = val3.WithoutBlocks;
                if (outputDirectory.Length == 0)
                {
                    val2.OutputDirectory = Settings.GetRealPath(val3.OutputDirectory);
                }
                val2.Update = val3.UpdateExisting && !val3.WithoutBlocks;
                if (!onlyAnnotations)
                {
                    val2.CoordinateStringToUse = val3.CoordinateStringToUse;
                    val.SetRules(val3.Rules);
                }
            }
            else
            {
                annotationExportData?.DwgUpdateEvents?.OnLog("Export to DWG using internal default settings.");
            }
            IList<string> exportNames = val2.GetExportNames();
            if (!DwgFileUpdater.AreFilesWritable(val2.OutputDirectory, (IEnumerable<string>)exportNames))
            {
                Console.Error.WriteLine("Some file(s) cannot be opened. Close applications using them before exporting.");
                return;
            }
            string template = FindTemplate(val, val3);
            if (!exportViewPictures)
            {
                if (annotationExportData != null)
                {
                    annotationExportData.DrawingIndexFileFactory = new DrawingsIndexFileFactory(outputDirectory, useTrbForAnnotations, delegate (string msg)
                    {
                        annotationExportData.DwgUpdateEvents.OnLog(msg);
                    });
                    annotationExportData.DrawingsToExport = val2.ActiveDrawings.Count;
                }
                for (int num = 0; num < val2.ActiveDrawings.Count; num++)
                {
                    AnnotationExportData annotationExportData2 = annotationExportData;
                    if (annotationExportData2 == null || annotationExportData2.IsCancelled?.Invoke() != true)
                    {
                        ExportDrawing(val2.ActiveDrawings[num], val, val2, template, exportNames[num], onlyAnnotations ? new bool?(true) : ((bool?)null), export3dAnnotationsAsSeparateViews, annotationExportData);
                    }
                }
                AnnotationExportData annotationExportData3 = annotationExportData;
                if (annotationExportData3 == null)
                {
                    return;
                }
                Func<bool> isCancelled = annotationExportData3.IsCancelled;
                if (((isCancelled != null) ? new bool?(!isCancelled()) : ((bool?)null)) == true)
                {
                    annotationExportData?.DrawingIndexFileFactory?.PublishIndexFile(delegate (string msg)
                    {
                        annotationExportData?.DwgUpdateEvents?.LogFormat("PublishIndexFileFailed: {0}", msg);
                    }, cmdOptions.IndexFileName);
                }
            }
            else
            {
                for (int num2 = 0; num2 < val2.ActiveDrawings.Count; num2++)
                {
                    ExportViewsPictures(val2.ActiveDrawings[num2], val, val2, template, cmdOptions.TargetViewIdsPerDrawingGuid, cmdOptions.ExportViewPicturesFilenameFormat);
                }
            }
        }

        private static string FindTemplate(RulesViewModel rulesViewModel, Settings settings)
        {
            string templateFilename = rulesViewModel.TemplateFilename;
            if (!string.IsNullOrEmpty(templateFilename) && !Path.IsPathRooted(templateFilename))
            {
                foreach (string item in settings.DataFolders())
                {
                    string text = Path.Combine(item, templateFilename);
                    if (File.Exists(text))
                    {
                        return text;
                    }
                }
            }
            return templateFilename;
        }
        private static void ExportDrawing(Drawing activeDrawing, RulesViewModel rulesViewModel, ExportViewModel exportViewModel, string template, string outputName, bool? onlyAnnotations, bool export3dAnnotationsAsSeparateViews, AnnotationExportData annotationExportData)
        {
            //IL_0111: Unknown result type (might be due to invalid IL or missing references)
            //IL_0084: Unknown result type (might be due to invalid IL or missing references)
            //try
            //{
            //    if (export3dAnnotationsAsSeparateViews)
            //    {
            //        DrawingExport.Dwg.AnnotationExport.AnnotationExport.ExportViewsIn3D(activeDrawing, rulesViewModel, exportViewModel, template, annotationExportData);
            //    }
            //    else if (exportViewModel.FileExtension == ".dgn")
            //    {
            //        DgnFileUpdater.ExportDgnToFile(activeDrawing, (IProgressWrapper)null, rulesViewModel.GetRules(), exportViewModel.CoordinateStringToUse, string.IsNullOrEmpty(template) ? null : new FileInfo(template), new FileInfo(Path.Combine(exportViewModel.OutputDirectory, outputName)), exportViewModel.Update, (DwgVersion)Enum.Parse(typeof(DwgVersion), Extensions.TrimStart(exportViewModel.FileVersion, "Dwg.UI.DwgFileVersion.".Length)), exportViewModel.SnapshotToModelSpace, exportViewModel.ScaleValue, exportViewModel.EmbedImages, exportViewModel.WithoutBlocks, exportViewModel.IncludeViewsOutsideSheet, onlyAnnotations, (View)null, false, (IDwgUpdateEvents)null);
            //    }
            //    else
            //    {
            //        DwgFileUpdater.ExportDwgToFile(activeDrawing, (IProgressWrapper)null, rulesViewModel.GetRules(), exportViewModel.CoordinateStringToUse, string.IsNullOrEmpty(template) ? null : new FileInfo(template), new FileInfo(Path.Combine(exportViewModel.OutputDirectory, outputName)), exportViewModel.Update, (DwgVersion)Enum.Parse(typeof(DwgVersion), Extensions.TrimStart(exportViewModel.FileVersion, "Dwg.UI.DwgFileVersion.".Length)), (SaveType)(!(exportViewModel.FileExtension == ".dwg")), exportViewModel.SnapshotToModelSpace, exportViewModel.ScaleValue, exportViewModel.EmbedImages, exportViewModel.WithoutBlocks, exportViewModel.IncludeViewsOutsideSheet, onlyAnnotations, (View)null, false, (IDwgUpdateEvents)null);
            //    }
            //    Identifier identifier = DatabaseObjectExtensions.GetIdentifier((DatabaseObject)(object)activeDrawing);
            //    if (identifier != null)
            //    {
            //        Operation.SetDrawingPlotDateById(identifier.ID);
            //    }
            //}
            //catch (Exception)
            //{
            //    Console.Error.WriteLine("Error creating " + outputName);
            //}
        }

        private static void ExportViewsPictures(Drawing activeDrawing, RulesViewModel rulesViewModel, ExportViewModel exportViewModel, string template, IDictionary<string, HashSet<int>> targetViewIdsPerDrawingGuid, string exportViewPicturesFilenameFormat)
        {
            try
            {
                HashSet<int> targetViewIds = null;
                if (targetViewIdsPerDrawingGuid != null)
                {
                    string text = DatabaseObjectExtensions.GetIdentifier((DatabaseObject)(object)activeDrawing).GUID.ToString();
                    targetViewIds = Extensions.GetValueOrDefault<string, HashSet<int>>(targetViewIdsPerDrawingGuid, text);
                }
                WritePreviewOfViews(activeDrawing, rulesViewModel.GetRules(), exportViewModel.CoordinateStringToUse, string.IsNullOrEmpty(template) ? null : new FileInfo(template), exportViewModel.OutputDirectory, exportViewModel.Update, exportViewModel.SnapshotToModelSpace, exportViewModel.ScaleValue, targetViewIds, exportViewPicturesFilenameFormat);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error exporting views pictures");
            }
        }

        private static void WritePreviewOfViews(Drawing targetDrawing, ObjectMappingRules rules, string coordinates, FileInfo templateFilePath, string outputFilePath, bool update, bool snapshotToModelSpace, string snapshotScale, HashSet<int> targetViewIds, string exportViewPicturesFilenameFormat)
        {
            //IL_0004: Unknown result type (might be due to invalid IL or missing references)
            //IL_000a: Expected O, but got Unknown
            if (targetDrawing == null)
            {
                return;
            }
            DrawingHandler val = new DrawingHandler();
            bool flag = false;
            try
            {
                Drawing activeDrawing = val.GetActiveDrawing();
                if (activeDrawing == null)
                {
                    flag = val.SetActiveDrawing(targetDrawing, true);
                    if (!flag)
                    {
                        throw new IOException("albl_Drawing_cannot_be_read");
                    }
                    activeDrawing = val.GetActiveDrawing();
                }
                foreach (object item in (DrawingEnumeratorBase)targetDrawing.GetSheet().GetAllViews())
                {
                    View val2 = (View)((item is View) ? item : null);
                    int num = ((val2 != null) ? DatabaseObjectExtensions.GetIdentifier((DatabaseObject)(object)val2).ID : 0);
                    if (val2 != null && (targetViewIds == null || targetViewIds.Contains(num)))
                    {
                        WriteImage(activeDrawing, rules, coordinates, templateFilePath, outputFilePath, update, snapshotToModelSpace, snapshotScale, val2, num, exportViewPicturesFilenameFormat);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch (CannotPerformOperationDrawingNotUpToDateException)
            {
            }
            if (flag)
            {
                val.CloseActiveDrawing(false);
            }
        }

        private static void WriteImage(Drawing drawing, ObjectMappingRules rules, string coordinates, FileInfo templateFilePath, string outputFilePath, bool update, bool snapshotToModelSpace, string snapshotScale, View view, int viewId, string exportViewPicturesFilenameFormat)
        {
            //IL_0016: Unknown result type (might be due to invalid IL or missing references)
            //IL_001c: Expected O, but got Unknown
            //IL_0053: Unknown result type (might be due to invalid IL or missing references)
            //Size val = new Size(((ViewBase)drawing.GetSheet()).Width, ((ViewBase)drawing.GetSheet()).Height);
            //bool flag = false;
            //BitmapImage bitmapImage = new BitmapImage();
            //SettingsBasket val2 = default(SettingsBasket);
            //((SettingsBasket)(ref val2))..ctor(coordinates, false, snapshotScale, snapshotToModelSpace, update);
            //using (Stream stream = DwgFileUpdater.GetPreviewBitmap((IProgressWrapper)null, drawing, rules, templateFilePath, new FileInfo(outputFilePath), (bool?)null, default(Size), val, val2, view))
            //{
            //    if (stream != null)
            //    {
            //        bitmapImage.BeginInit();
            //        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            //        bitmapImage.StreamSource = stream;
            //        bitmapImage.EndInit();
            //        flag = true;
            //    }
            //    if (bitmapImage.CanFreeze)
            //    {
            //        bitmapImage.Freeze();
            //    }
            //}
            //if (view != null && flag)
            //{
            //    BitmapEncoder bitmapEncoder = new PngBitmapEncoder();
            //    bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            //    string plotFileNameExt = drawing.GetPlotFileNameExt((IncludeRevisionMarkEnum)0);
            //    plotFileNameExt = Regex.Replace(plotFileNameExt, "[^a-zA-Z0-9]", "_");
            //    using FileStream stream2 = new FileStream(Path.Combine(outputFilePath, string.Format(exportViewPicturesFilenameFormat, plotFileNameExt, DatabaseObjectExtensions.GetIdentifier((DatabaseObject)(object)drawing).ID, viewId, DatabaseObjectExtensions.GetIdentifier((DatabaseObject)(object)drawing).GUID.ToString())), FileMode.Create);
            //    bitmapEncoder.Save(stream2);
            //}
        }

        private static string GetSettingsFileFromInput(Settings settings, string settingsFileIn)
        {
            if (string.IsNullOrEmpty(settingsFileIn))
            {
                if (settings.SettingFiles().Contains("standard"))
                {
                    return "standard";
                }
                return string.Empty;
            }
            return settingsFileIn;
        }

        private static List<Drawing> GetDrawingByIds(HashSet<string> guids)
        {
            //IL_0006: Unknown result type (might be due to invalid IL or missing references)
            //IL_000c: Expected O, but got Unknown
            //IL_0032: Unknown result type (might be due to invalid IL or missing references)
            //IL_003c: Expected O, but got Unknown
            List<Drawing> list = new List<Drawing>();
            DrawingHandler val = new DrawingHandler();
            if (val.GetActiveDrawing() != null)
            {
                list.Add(val.GetActiveDrawing());
            }
            else
            {
                foreach (string guid in guids)
                {
                    Drawing drawing = Operation.GetDrawing(new Identifier(guid));
                    if (drawing != null)
                    {
                        list.Add(drawing);
                    }
                }
            }
            return list;
        }
    }

    internal class SettingsBasket
    {
    }
}
