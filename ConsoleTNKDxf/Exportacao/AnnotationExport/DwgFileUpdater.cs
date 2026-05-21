using System;
using System.Collections.Generic;
using System.IO;
using Tekla.Structures.Drawing;

namespace ConsoleTNKDxf.Exportacao.AnnotationExport
{
    internal class DwgFileUpdater
    {
        internal static bool AreFilesWritable(string outputDirectory, IEnumerable<string> exportNames)
        {
            throw new NotImplementedException();
        }

        internal static void ExportDwgToFile(Drawing drawing, IProgressWrapper exportProgress, object value, string v1, FileInfo fileInfo1, FileInfo fileInfo2, bool v2, DwgVersion dwgVersion, SaveType saveType, bool v3, string v4, bool v5, bool v6, bool v7, bool? v8, View view, bool v9, IDwgUpdateEvents dwgUpdateEvents)
        {
            throw new NotImplementedException();
        }

        internal static Stream GetPreviewBitmap(IProgressWrapper progressWrapper, Drawing drawing, ObjectMappingRules rules, FileInfo templateFilePath, FileInfo fileInfo, bool? v, Size size, Size val, SettingsBasket val2, View view)
        {
            throw new NotImplementedException();
        }
    }
}