using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class CamposDesenhoINP
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
        private string _name;

        private string _tagQA;
        private int _qtdQA;
        private string _desenhoQA;
        private string _desenhoClienteQA;
        private int _familiaQA;

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
        public string Name => _name;
        public string TagQA => _tagQA;
        public int QtdQA => _qtdQA;
        public string DesenhoQA => _desenhoQA;
        public string DesenhoClienteQA => _desenhoClienteQA;
        public int FamiliaQA => _familiaQA;



        public CamposDesenhoINP(Drawing drawing)
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
            tempBeam.GetReportProperty("NAME", ref _name);

            List<string> userProperties = new List<string> { "TCNK_N_TCNK", "TCNK_N_CLIENTE", "TCNK_TAG", "TCNK_FAMILIA",  };
            Dictionary<string, string> properties = new Dictionary<string, string>();
            drawing.GetStringUserProperties(userProperties, out properties);
            _tagQA = properties.ContainsKey("TCNK_TAG") ? properties["TCNK_TAG"] : string.Empty;
            
            _desenhoQA = properties.ContainsKey("TCNK_N_TCNK") ? properties["TCNK_N_TCNK"] : string.Empty;
            _desenhoClienteQA = properties.ContainsKey("TCNK_N_CLIENTE") ? properties["TCNK_N_CLIENTE"] : string.Empty;


            List<string> userPropertiesInteger = new List<string> { "TCNK_FAMILIA", "TCNK_QTD" };
            Dictionary<string, int> propertiesInteger = new Dictionary<string, int>();
            drawing.GetIntegerUserProperties(userPropertiesInteger, out propertiesInteger);
            _familiaQA = propertiesInteger.ContainsKey("TCNK_FAMILIA") ? propertiesInteger["TCNK_FAMILIA"] : 0;
            _qtdQA = propertiesInteger.ContainsKey("TCNK_QTD") ? propertiesInteger["TCNK_QTD"] : 0;


        }
    }
}
