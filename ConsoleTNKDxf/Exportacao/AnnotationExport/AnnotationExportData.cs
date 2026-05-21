using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTNKDxf.Exportacao.AnnotationExport
{
    public class AnnotationExportData
    {
        public DrawingsIndexFileFactory DrawingIndexFileFactory { get; set; }

        public DwgUpdateEvents DwgUpdateEvents { get; private set; }

        public int DrawingsToExport { get; set; }

        public int DrawingNumber { get; set; }

        public TextWriter AnnotationWriter { get; private set; }

        public Func<bool> IsCancelled { get; private set; }

        public void Setup(string pipeId, string outputDirectory, TextWriter annotationWriter = null)
        {
            DwgUpdateEvents = new DwgUpdateEvents();
            string cancelPath = Path.Combine(outputDirectory, "AnnotationCancel.txt");
            IsCancelled = () => File.Exists(cancelPath);
            if (annotationWriter != null)
            {
                AnnotationWriter = annotationWriter;
            }
            else if (pipeId != null)
            {
                PipeStream stream = new AnonymousPipeClientStream(PipeDirection.Out, pipeId);
                AnnotationWriter = new StreamWriter(stream)
                {
                    AutoFlush = true
                };
                DwgUpdateEvents.Log += delegate (string message)
                {
                    AnnotationWriter?.WriteLine("LOG:" + message.Replace(Environment.NewLine, "<NEWLINE>"));
                };
            }
        }
    }
}
