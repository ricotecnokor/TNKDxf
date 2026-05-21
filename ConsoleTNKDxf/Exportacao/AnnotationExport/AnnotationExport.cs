using netDxf.Header;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.DrawingInternal;
using Tekla.Structures.DrawingPresentationModel;
using Tekla.Structures.Geometry3d;

namespace ConsoleTNKDxf.Exportacao.AnnotationExport
{
    public static class AnnotationExport
    {
        public static void EntryPoint(string outputDirectory, string indexFileName, TextWriter annotationStream)
        {
            ExportacaoDxf.Export(new CommandOptions
            {
                OutputDirectory = outputDirectory,
                IndexFileName = indexFileName,
                Export3dAnnotationsAsSeparateViews = true,
                SettingsFile = "PublishAnnotations"
            }, annotationStream);
        }

        public static void ExportViewsIn3D(Drawing drawing, RulesViewModel rulesViewModel, ExportViewModel exportViewModel, string template, AnnotationExportData annotationExportData)
        {
            //IL_00ae: Unknown result type (might be due to invalid IL or missing references)
            //IL_00b3: Unknown result type (might be due to invalid IL or missing references)
            //IL_0274: Unknown result type (might be due to invalid IL or missing references)
            //IL_027b: Expected O, but got Unknown
            annotationExportData.DrawingNumber++;
            if (drawing == null)
            {
                return;
            }
            DwgUpdateEvents dwgUpdateEvents = new DwgUpdateEvents();
            dwgUpdateEvents.Log += delegate (string message)
            {
                annotationExportData.DwgUpdateEvents.OnLog(message);
            };
            string drawingName = drawing.GetPlotFileNameExt((IncludeRevisionMarkEnum)2);
            try
            {
                if (!DrawingChecks.AreAnnotationsExportable(drawing))
                {
                    annotationExportData.DwgUpdateEvents.LogErrorFormat("Drawing cannot be exported: {0} ({1})", ((object)drawing.UpToDateStatus/*cast due to constrained. prefix*/).ToString(), drawingName);
                    return;
                }
                drawingName = new Regex("[<>:\"/\\\\|?*]").Replace(drawingName, "_");
           
                if (!DrawingData.TryGetDrawingTypeFolderName(drawing, out var typeFolderName))
                {
                    dwgUpdateEvents.LogFormat("Unable to get drawing type folder name for drawing Type: {0}, Mark: {1}, Name: {2}", ((object)drawing).GetType(), drawing.Mark, drawingName);
                    return;
                }
                FileInfo fileInfo = new FileInfo(Path.Combine(exportViewModel.OutputDirectory, typeFolderName, $"{drawingName}.dwg"));
                Dictionary<int, List<int>> viewIdToDrawingObjectIdsMap = new Dictionary<int, List<int>>();
                Dictionary<Identifier, HashSet<int>> modelObjectIdToDrawingObjectIdMap = new Dictionary<Identifier, HashSet<int>>();
                Dictionary<int, string> viewIdToViewLabelTextMap = new Dictionary<int, string>();
                dwgUpdateEvents.DrawingObjectExported += delegate (int drawingObjectId, int viewId)
                {
                    if (viewIdToDrawingObjectIdsMap.TryGetValue(viewId, out var value))
                    {
                        value.Add(drawingObjectId);
                    }
                    else
                    {
                        viewIdToDrawingObjectIdsMap[viewId] = new List<int>(new int[1] { drawingObjectId });
                    }
                };
                dwgUpdateEvents.ModelObjectToDrawingObjectsEntry += delegate (Identifier modelObjectId, int drawingObjectId)
                {
                    if (!modelObjectIdToDrawingObjectIdMap.TryGetValue(modelObjectId, out var value))
                    {
                        value = new HashSet<int>();
                        modelObjectIdToDrawingObjectIdMap[modelObjectId] = value;
                    }
                    value.Add(drawingObjectId);
                };
                DrawingData drawingData = null;
                dwgUpdateEvents.BeforeDwgSave += delegate (DwgUpdateEvents.BeforeDwgSaveEventArgs args)
                {
                    if (annotationExportData.IsCancelled())
                    {
                        args.SaveDwg = false;
                    }
                    else
                    {
                        drawingData = annotationExportData.DrawingIndexFileFactory.GetDrawingData(drawing, fileInfo, dwgUpdateEvents.LogError, dwgUpdateEvents.OnLog);
                        if (drawingData == null)
                        {
                            dwgUpdateEvents.LogErrorFormat("Unabled to load drawing data for {0}", fileInfo.Name);
                            args.SaveDwg = false;
                        }
                        else
                        {
                            args.SaveDwg = annotationExportData.DrawingIndexFileFactory.ProcessDrawingDataForExport(drawingData, viewIdToDrawingObjectIdsMap, modelObjectIdToDrawingObjectIdMap, viewIdToViewLabelTextMap);
                            if (!args.SaveDwg)
                            {
                                dwgUpdateEvents.LogFormat("No annotations, skipping drawing {0}", fileInfo.Name);
                            }
                        }
                    }
                };
                dwgUpdateEvents.DrawingSaved += delegate
                {
                    if (drawingData == null)
                    {
                        dwgUpdateEvents.LogErrorFormat("Drawing data was null for {0}", fileInfo.Name);
                    }
                    else
                    {
                        annotationExportData.DrawingIndexFileFactory.AddDrawingData(drawingData);
                    }
                };
                HashSet<int> viewIds = new HashSet<int>();
                ContainerView sheet = drawing.GetSheet();
                DrawingObjectEnumerator allViews = sheet.GetAllViews();
                ((DrawingEnumeratorBase)allViews).SelectInstances = false;
                foreach (ViewBase item in (DrawingEnumeratorBase)allViews)
                {
                    ViewBase val = item;
                    viewIds.Add(DatabaseObjectExtensions.GetIdentifier((DatabaseObject)(object)val).ID);
                }
                Dictionary<int, bool> viewInSheetMap = new Dictionary<int, bool>();
                dwgUpdateEvents.BeforeExportSegment += delegate (DwgUpdateEvents.BeforeExportSegmentEventArgs args)
                {
                    //IL_0118: Unknown result type (might be due to invalid IL or missing references)
                    //IL_0122: Expected O, but got Unknown
                    if (args.Segment.ObjectType == 260)
                    {
                        List<string> values = (from x in GetChildren((PrimitiveGroup)(object)args.Segment).OfType<TextPrimitive>()
                                               select x.Text).ToList();
                        string value = string.Join(" ", values);
                        ViewBase view = args.View;
                        int? num = ((view != null) ? new int?(DatabaseObjectExtensions.GetIdentifier((DatabaseObject)(object)view).ID) : ((int?)null));
                        if (!string.IsNullOrWhiteSpace(value) && num.HasValue)
                        {
                            viewIdToViewLabelTextMap[num.Value] = value;
                        }
                    }
                    bool flag = viewIds.Contains(args.ViewId);
                    if (flag && !viewInSheetMap.ContainsKey(args.ViewId))
                    {
                        ViewBase view2 = args.View;
                        Tekla.Structures.Drawing.View val2 = (Tekla.Structures.Drawing.View)(object)((view2 is Tekla.Structures.Drawing.View) ? view2 : null);
                        bool value2 = true;
                        if (val2 != null)
                        {
                            Point val3 = ((ViewBase)val2).Origin + (Point)(object)((ViewBase)val2).FrameOrigin;
                            Point val4 = val3 + (Point)new Vector(((ViewBase)val2).Width, ((ViewBase)val2).Height, 0.0);
                            value2 = val3.X < ((ViewBase)sheet).Width - 0.001 && val3.Y < ((ViewBase)sheet).Height - 0.001 && val4.X > 0.001 && val4.Y > 0.001;
                        }
                        viewInSheetMap[args.ViewId] = value2;
                    }
                    args.ExportSegment = flag && viewInSheetMap[args.ViewId];
                };
                ExportProgress exportProgress = new ExportProgress();
                exportProgress.IsCancelled = annotationExportData.IsCancelled;
                exportProgress.ReportEvent += delegate (double value, bool cancellable)
                {
                    annotationExportData?.AnnotationWriter?.WriteLine(string.Format(CultureInfo.InvariantCulture, "PROGRESS:{0}:{1}:{2}:{3}:\"{4}\"", annotationExportData.DrawingNumber, annotationExportData.DrawingsToExport, value, cancellable, drawingName));
                };
                DwgFileUpdater.ExportDwgToFile(
                    drawing, 
                    (IProgressWrapper)(object)exportProgress, 
                    rulesViewModel.GetRules(), 
                    "Model", 
                    string.IsNullOrWhiteSpace(template) ? null : new FileInfo(template), 
                    fileInfo, 
                    false, 
                    (DwgVersion)29, 
                    (SaveType)0, 
                    false, "1.0", true, false, true, (bool?)true, null, true, (IDwgUpdateEvents)(object)dwgUpdateEvents);
            }
            catch (IOException ex)
            {
                dwgUpdateEvents.LogError(ex.ToString());
            }
            catch (CannotPerformOperationDrawingNotUpToDateException)
            {
                dwgUpdateEvents.LogErrorFormat("Drawing is not up to date and cannot be exported. Check both the drawing state and the drawing's numbering state. ({0})", drawingName);
            }
        }

        private static IEnumerable<PrimitiveBase> GetChildren(PrimitiveGroup group)
        {
            foreach (PrimitiveBase primitive in group.Primitives)
            {
                if (primitive is PrimitiveGroup)
                {
                    foreach (PrimitiveBase child in GetChildren((PrimitiveGroup)primitive))
                    {
                        yield return child;
                    }
                }
                else
                {
                    yield return primitive;
                }
            }
        }
    }
}
