using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class CamposFormatoDgt
    {
        private string _title;
        private string _title1;
        private string _title3;
        private string _title2;
        private string _projectObject;
        private string _revisionMark;
        private string _projectModel;
        private string _projectNumber;
        private string _scale1;
        private string _scale2;
        private string _scale3;
        private string _scale4;
        private string _scale5;

        public string Title => _title;
        public string Title1 => _title1;
        public string Title2 => _title2;
        public string Title3 => _title3;
        public string ProjectObject => _projectObject;
        public string RevisionMark => _revisionMark;
        public string ProjectModel => _projectModel;
        public string ProjectNumber => _projectNumber;
        public string Scale1 => _scale1;
        public string Scale2 => _scale2;
        public string Scale3 => _scale3;
        public string Scale4 => _scale4;
        public string Scale5 => _scale5;

        public CamposFormatoDgt(MultiDrawing drawing)
        {

            PropertyInfo propInfo = drawing.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
            object value = propInfo.GetValue(drawing, null);
            Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

            Beam tempBeam = new Beam();
            tempBeam.Identifier = identifier;

            tempBeam.GetReportProperty("TITLE", ref _title);
            tempBeam.GetReportProperty("TITLE1", ref _title1);
            tempBeam.GetReportProperty("TITLE2", ref _title2);
            tempBeam.GetReportProperty("TITLE3", ref _title3);
            tempBeam.GetReportProperty("PROJECT.OBJECT", ref _projectObject);
            tempBeam.GetReportProperty("REVISION.MARK", ref _revisionMark);
            tempBeam.GetReportProperty("PROJECT.MODEL", ref _projectModel);
            tempBeam.GetReportProperty("PROJECT.NUMBER", ref _projectNumber);
            tempBeam.GetReportProperty("SCALE1", ref _scale1);
            tempBeam.GetReportProperty("SCALE2", ref _scale2);
            tempBeam.GetReportProperty("SCALE3", ref _scale3);
            tempBeam.GetReportProperty("SCALE4", ref _scale4);
            tempBeam.GetReportProperty("SCALE5", ref _scale5);


        }
    }
}
