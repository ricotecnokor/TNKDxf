using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class TipoDesenho
    {
        private string _tipo;
        private string _listarElementosObra;
        private string _criarLM;

        public string Tipo => _tipo;
        public string ListarElementosObra => _listarElementosObra;
                public string CriarLM => _criarLM;
        public TipoDesenho(MultiDrawing drawing)
        {
            PropertyInfo propInfo = drawing.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
            object value = propInfo.GetValue(drawing, null);
            Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

            Beam tempBeam = new Beam();
            tempBeam.Identifier = identifier;

            tempBeam.GetReportProperty("TCNM_TIPO_DESENHO", ref _tipo);
            tempBeam.GetReportProperty("TCNM_LISTAR_PARAF", ref _listarElementosObra);
            tempBeam.GetReportProperty("TCNM_CRIAR_LM", ref _criarLM);
        }
    }
}
