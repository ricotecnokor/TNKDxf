using System;
using System.Collections.Generic;
using System.IO;
using Tekla.Structures.Drawing;

namespace ConsoleTNKDxf.Exportacao.AnnotationExport
{
    public class ExportViewModel
    {
        private DirectoryInfo directoryInfo;

        public ExportViewModel(DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
        }

        public string OutputDirectory { get; internal set; }
        public List<Drawing> ActiveDrawings { get; internal set; }
        public string FileExtension { get; internal set; }
        public string FilePrefix { get; internal set; }
        public string FileSuffix { get; internal set; }
        public string FileVersion { get; internal set; }
        public bool OpenFolderWhenDone { get; internal set; }
        public bool SnapshotToModelSpace { get; internal set; }
        public string ScaleValue { get; internal set; }
        public object EmbedImages { get; internal set; }
        public bool WithoutBlocks { get; internal set; }
        public bool Update { get; internal set; }
        public string CoordinateStringToUse { get; internal set; }

        internal IList<string> GetExportNames()
        {
            throw new NotImplementedException();
        }
    }
}