using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Revisao
    {
        private  string _revisionMark;
        private  string _descricao;
        private  string _criadoPor;
        private  string _dataCriacao;
        private  string _checadoPor;
        private  string _dataChecado;
        private  string _aprovadoPor;
        private  string _dataAprovacao;

        public Revisao(Drawing drawing)
        {
            // Acessa o Identifier interno do Drawing via Reflection
            PropertyInfo propInfo = drawing.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
            object value = propInfo.GetValue(drawing, null);
            Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

            // Cria um objeto ModelObject temporário com o mesmo Identifier
            Beam tempBeam = new Beam();
            tempBeam.Identifier = identifier;



            tempBeam.GetReportProperty("REVISION.LAST_MARK", ref _revisionMark);
            tempBeam.GetReportProperty("REVISION.LAST_DESCRIPTION", ref _descricao);
            tempBeam.GetReportProperty("REVISION.LAST_CREATED_BY", ref _criadoPor);
            tempBeam.GetReportProperty("REVISION.LAST_DATE_CREATE", ref _dataCriacao);
            tempBeam.GetReportProperty("REVISION.LAST_CHECKED_BY", ref _checadoPor);
            tempBeam.GetReportProperty("REVISION.LAST_DATE_CHECKED", ref _dataChecado);
            tempBeam.GetReportProperty("REVISION.LAST_APPROVED_BY", ref _aprovadoPor);
            tempBeam.GetReportProperty("REVISION.LAST_DATE_APPROVED", ref _dataAprovacao);
        }

        public string RevisionMark => _revisionMark == null ? string.Empty : _revisionMark;
        public string Descricao => _descricao == null ? string.Empty : _descricao;
        public string CriadoPor => _criadoPor == null ? string.Empty : _criadoPor;
        public string DataCriacao => _dataCriacao == null ? string.Empty : _dataCriacao;
        public string ChecadoPor => _checadoPor == null ? string.Empty : _checadoPor;
        public string DataChecado => _dataChecado == null ? string.Empty : _dataChecado;
        public string AprovadoPor => _aprovadoPor == null ? string.Empty : _aprovadoPor;
        public string DataAprovacao => _dataAprovacao == null ? string.Empty : _dataAprovacao;
    }
}
