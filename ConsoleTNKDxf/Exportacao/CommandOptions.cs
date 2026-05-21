using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTNKDxf.Exportacao
{
    public class CommandOptions
    {
        public string Name { get; set; }

        public string OutputDirectory { get; set; }

        public string SettingsFile { get; set; } = string.Empty;

        public bool OnlyAnnotations { get; set; }

        public bool ExportViewPictures { get; set; }

        public bool Export3dAnnotationsAsSeparateViews { get; set; }

        public string ExportViewPicturesFilenameFormat { get; set; } = "{0}.View_{1}_{2}.png";

        public Dictionary<string, HashSet<int>> TargetViewIdsPerDrawingGuid { get; set; }

        public bool UseTrbForAnnotations { get; set; }

        public string AnnotationPipe { get; set; }

        public string IndexFileName { get; set; }

        public string FileExtension { get; set; }
    }
}
