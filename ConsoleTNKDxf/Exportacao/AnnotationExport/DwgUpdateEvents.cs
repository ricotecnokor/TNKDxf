using System;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using static ConsoleTNKDxf.Exportacao.AnnotationExport.DwgUpdateEvents;

namespace ConsoleTNKDxf.Exportacao.AnnotationExport
{
    public class DwgUpdateEvents : IDwgUpdateEvents
    {
        public class BeforeDwgSaveEventArgs
        {
            public bool SaveDwg { get; set; }
        }

        public class BeforeExportSegmentEventArgs
        {
            public int ViewId { get; set; }

            public ViewBase View { get; set; }

            public Segment Segment { get; set; }

            public bool ExportSegment { get; set; }
        }

        public bool ExportAnnotationsForConnect => true;

        public event Action<int, int> DrawingObjectExported;

        public event Action DrawingSaved;

        public event Action<Identifier, int> ModelObjectToDrawingObjectsEntry;

        public event Action<BeforeDwgSaveEventArgs> BeforeDwgSave;

        public event Action<BeforeExportSegmentEventArgs> BeforeExportSegment;

        public event Action<string> Log;

        public void OnDrawingObjectExported(int drawingObjectId, int viewId)
        {
            this.DrawingObjectExported?.Invoke(drawingObjectId, viewId);
        }

        public void OnDrawingSaved()
        {
            this.DrawingSaved?.Invoke();
        }

        public void OnModelObjectToDrawingObjectsEntry(Identifier modelObjectId, int drawingObjectId)
        {
            this.ModelObjectToDrawingObjectsEntry?.Invoke(modelObjectId, drawingObjectId);
        }

        public void OnLog(string message)
        {
            this.Log?.Invoke(message);
        }

        public void LogError(string message)
        {
            this.Log?.Invoke("ERROR:" + message);
        }

        public void LogFormat(string message, params object[] args)
        {
            this.Log?.Invoke(string.Format(message, args));
        }

        public void LogErrorFormat(string message, params object[] args)
        {
            LogError(string.Format(message, args));
        }

        public bool ShouldSaveDwg()
        {
            BeforeDwgSaveEventArgs e = new BeforeDwgSaveEventArgs();
            this.BeforeDwgSave?.Invoke(e);
            return e.SaveDwg;
        }

        public bool ShouldExportSegment(int viewId, ViewBase view, Segment segment)
        {
            BeforeExportSegmentEventArgs e = new BeforeExportSegmentEventArgs
            {
                ViewId = viewId,
                View = view,
                Segment = segment
            };
            this.BeforeExportSegment?.Invoke(e);
            return e.ExportSegment;
        }
    }

    public interface IDwgUpdateEvents
    {
        bool ExportAnnotationsForConnect { get; }
        event Action<int, int> DrawingObjectExported;
        event Action DrawingSaved;
        event Action<Identifier, int> ModelObjectToDrawingObjectsEntry;
        event Action<BeforeDwgSaveEventArgs> BeforeDwgSave;
        event Action<BeforeExportSegmentEventArgs> BeforeExportSegment;
        event Action<string> Log;
        void OnDrawingObjectExported(int drawingObjectId, int viewId);
        void OnDrawingSaved();
        void OnModelObjectToDrawingObjectsEntry(Identifier modelObjectId, int drawingObjectId);
        void OnLog(string message);
        void LogError(string message);
        void LogFormat(string message, params object[] args);
        void LogErrorFormat(string message, params object[] args);
        bool ShouldSaveDwg();
        bool ShouldExportSegment(int viewId, ViewBase view, Segment segment);

    }
}
