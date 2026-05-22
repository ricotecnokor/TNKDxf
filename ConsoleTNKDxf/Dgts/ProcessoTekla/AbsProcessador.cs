using ConsoleTNKDxf.Dgts.ProcessoTekla;
using netDxf;
using System.Reflection;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public abstract class AbsProcessador
    {
        protected TSM.Model _model;
        protected string _tipo;
        protected string _listarElementosObra;
        protected string _criarLM;
        protected Drawing _drawing;


        public string Tipo => _tipo;
        public string ListarElementosObra => _listarElementosObra;
        public string CriarLM => _criarLM;

        public AbsProcessador(TSM.Model model, Drawing drawing, string criarLM, string listarElementosObra)
        {
            _model = model;
            _drawing = drawing;
            _criarLM = criarLM;
            _listarElementosObra = listarElementosObra;
        }


        public abstract void Processar(string versaoTsep, DxfDocument dxf);

        public static AbsProcessador ObterTipo(TSM.Model model, TSD.Drawing drawing)
        {
            PropertyInfo propInfo = drawing.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
            object value = propInfo.GetValue(drawing, null);
            Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

            Beam tempBeam = new Beam();
            tempBeam.Identifier = identifier;

            string nomeTipo = string.Empty;
            string listarElementosObra = string.Empty;
            string criarLM = string.Empty;

            tempBeam.GetReportProperty("TCNM_TIPO_DESENHO", ref nomeTipo);
            tempBeam.GetReportProperty("TCNM_LISTAR_PARAF", ref listarElementosObra);
            tempBeam.GetReportProperty("TCNM_CRIAR_LM", ref criarLM);


            var tipo = drawing.GetType();

            if (tipo == typeof(TSD.GADrawing))
            {
                return new ProcessarMontagem(model, drawing, criarLM, listarElementosObra);

            }
            else if (tipo == typeof(TSD.MultiDrawing))
            {
                if(nomeTipo == "MONTAGEM") return new ProcessarMontagem(model, drawing, criarLM, listarElementosObra);

                return new ProcessadorDetalhamento(model, drawing, criarLM, listarElementosObra);
            }
            return null;
        }

        public override string ToString()
        {
            return _tipo;
        }

        public static explicit operator string(AbsProcessador valor)
        {
            return valor?._tipo ?? string.Empty;
        }
    }
}
