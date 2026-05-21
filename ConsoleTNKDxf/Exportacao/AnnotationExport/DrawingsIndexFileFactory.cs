using System;
using System.Collections.Generic;
using System.IO;
using Tekla.Structures;
using Tekla.Structures.Drawing;

namespace ConsoleTNKDxf.Exportacao.AnnotationExport
{
    public class DrawingsIndexFileFactory
    {
        private string outputDirectory;
        private bool useTrbForAnnotations;
        private Action<string> value;

        public DrawingsIndexFileFactory(string outputDirectory, bool useTrbForAnnotations, Action<string> value)
        {
            this.outputDirectory = outputDirectory;
            this.useTrbForAnnotations = useTrbForAnnotations;
            this.value = value;
        }

        internal void AddDrawingData(DrawingData drawingData)
        {
            throw new NotImplementedException();
        }

        internal DrawingData GetDrawingData(Drawing drawing, FileInfo fileInfo, Action<string> logError, Action<string> onLog)
        {
            throw new NotImplementedException();
        }

        internal bool ProcessDrawingDataForExport(DrawingData drawingData, Dictionary<int, List<int>> viewIdToDrawingObjectIdsMap, Dictionary<Identifier, HashSet<int>> modelObjectIdToDrawingObjectIdMap, Dictionary<int, string> viewIdToViewLabelTextMap)
        {
            throw new NotImplementedException();
        }

        internal void PublishIndexFile(Action<string> value, string indexFileName)
        {
            throw new NotImplementedException();
        }
    }
}