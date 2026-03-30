using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using Tekla.Structures.DrawingInternal;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class RevisaoDgt
    {
        private string _revisionLastMark;
        private string _revisionLastDescription;
        private string _revisionLastCreatedBy;
        private string _revisionLastDateCreated;
        private string _revisionLastCheckedBy;
        private string _revisionLastDateChecked;
        private string _revisionLastApprovedBy;
        private string _revisionLastDateApproved;

        public string RevisionLastMark => _revisionLastMark;
        public string RevisionLastDescription => _revisionLastDescription;
        public string RevisionLastCreatedBy => _revisionLastCreatedBy;
        public string RevisionLastDateCreated => _revisionLastDateCreated;
        public string RevisionLastCheckedBy => _revisionLastCheckedBy;
        public string RevisionLastDateChecked => _revisionLastDateChecked;
        public string RevisionLastApprovedBy => _revisionLastApprovedBy;
        public string RevisionLastDateApproved => _revisionLastDateApproved;


        public RevisaoDgt(Drawing drawing)
        {
            // Acessa o Identifier interno do Drawing via Reflection
            PropertyInfo propInfo = drawing.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
            object value = propInfo.GetValue(drawing, null);
            Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

            // Cria um objeto ModelObject temporário com o mesmo Identifier
            Beam tempBeam = new Beam();
            tempBeam.Identifier = identifier;

            tempBeam.GetReportProperty("REVISION.LAST_MARK", ref _revisionLastMark);
            tempBeam.GetReportProperty("REVISION.LAST_DESCRIPTION", ref _revisionLastDescription);
            tempBeam.GetReportProperty("REVISION.LAST_CREATED_BY", ref _revisionLastCreatedBy);
            tempBeam.GetReportProperty("REVISION.LAST_DATE_CREATE", ref _revisionLastDateCreated);
            tempBeam.GetReportProperty("REVISION.LAST_CHECKED_BY", ref _revisionLastCheckedBy);
            tempBeam.GetReportProperty("REVISION.LAST_DATE_CHECKED", ref _revisionLastDateChecked);
            tempBeam.GetReportProperty("REVISION.LAST_APPROVED_BY", ref _revisionLastApprovedBy);
            tempBeam.GetReportProperty("REVISION.LAST_DATE_APPROVED", ref _revisionLastDateApproved);

                _revisionLastDateCreated = string.IsNullOrEmpty(_revisionLastDateCreated) ? DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") : Convert.ToDateTime(_revisionLastDateCreated).ToString("dd/MM/yyyy HH:mm:ss");
                _revisionLastDateChecked = string.IsNullOrEmpty(_revisionLastDateChecked) ? DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") : Convert.ToDateTime(_revisionLastDateChecked).ToString("dd/MM/yyyy HH:mm:ss");
                _revisionLastDateApproved = string.IsNullOrEmpty(_revisionLastDateApproved) ? DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") : Convert.ToDateTime(_revisionLastDateApproved).ToString("dd/MM/yyyy HH:mm:ss");

        }
    }
}
