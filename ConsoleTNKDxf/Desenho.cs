using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.DrawingInternal;
using Tekla.Structures.Model;
using static Tekla.Structures.Model.ReferenceModel;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Desenho
    {
        TSM.Model _model;
        TSD.MultiDrawing _multiDrawing;
        private CamposFormato _camposFormato;
        private Revisao _revisao;
        private QuadroAplicacaoDgt _quadroAplicacao;
        private ListaMateriais _listaMateriais;
        private ElementosFixacao _elementosFixacao;
        //private string _nomeModelo;
        private DateTime _data;
        //private string _numeroProjeto;
        private string _listarElementosObra;

        public string Title1 => _multiDrawing.Title1;

        public Revisao QuadroRevisao => _revisao;
        public string RevisaoFormato => _camposFormato.Revisao;
        public string RevisaoFormatoCliente => _camposFormato.RevisaoCliente;
        public QuadroAplicacaoDgt QuadroAplicacao => _quadroAplicacao;
        //public PropriedadesDesenho PropriedadesFormato => ;

        public ListaMateriais ListaMateriais => _listaMateriais;
        public ElementosFixacao ElementosFixacao => _elementosFixacao;

        public string NomeModelo => _camposFormato.NomeModelo;
        public string Data => _data.ToString();
        public object NumeroProjeto => _camposFormato.NumeroProjeto;
        public object NumeroContratada => _camposFormato.NumeroContratada;
        public object NumeroCliente => _camposFormato.NumeroCliente;
        public object DescricaoProjeto => _camposFormato.DescricaoProjeto;
        public object RevisaoCliente => _camposFormato.RevisaoCliente;
        public object Escala => _camposFormato.Escala;

        public Desenho(TSD.MultiDrawing multiDrawing, TSM.Model model)
        {
            _multiDrawing = multiDrawing;
            _model = model;
            _camposFormato = new CamposFormato(multiDrawing);
            _revisao = new Revisao(multiDrawing);
            _quadroAplicacao = new QuadroAplicacaoDgt(multiDrawing);
            _listaMateriais = new ListaMateriais(model, multiDrawing);
            _elementosFixacao = new ElementosFixacao(model, multiDrawing);
            setListarElementosObra();
        }

        private void setListarElementosObra()
        {
            //
            // Acessa o Identifier interno do Drawing via Reflection
            PropertyInfo propInfo = _multiDrawing.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
            object value = propInfo.GetValue(_multiDrawing, null);
            Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

            // Cria um objeto ModelObject temporário com o mesmo Identifier
            Beam tempBeam = new Beam();
            tempBeam.Identifier = identifier;


            //string listarElementosObra = string.Empty;
            tempBeam.GetReportProperty("TCNM_LISTAR_PARAF", ref _listarElementosObra);

        }

    }
}
