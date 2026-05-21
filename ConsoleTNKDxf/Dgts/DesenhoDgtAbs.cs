using ConsoleTNKDxf.Abstracoes;
using System;
using System.Reflection;
using Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public abstract class DesenhoDgtAbs<T> where T : ConjuntoAbstrato
    {
        protected LmAbs<T> _coletorLM;

        protected QuadroAplicacaoDgt _quadroAplicacao;
        protected ElementosFixacaoDgt _elementosFixacao;
        protected RevisaoDgt _revisao;
        protected CamposFormatoDgt _camposFormato;
        protected string _listarElementosObra;
        protected string _criarLM;


        public string Title => _camposFormato.Title;
        public string Title1 => _camposFormato.Title1;
        public string Title2 => _camposFormato.Title2;
        public string Title3 => _camposFormato.Title3;
        public string ProjectObject => _camposFormato.ProjectObject;
        public string RevisionMark => _camposFormato.RevisionMark;
        public string ProjectModel => _camposFormato.ProjectModel;
        public string ProjectNumber => _camposFormato.ProjectNumber;
        public string Scale1 => _camposFormato.Scale1;
        public string Scale2 => _camposFormato.Scale2;
        public string Scale3 => _camposFormato.Scale3;
        public string Scale4 => _camposFormato.Scale4;
        public string Scale5 => _camposFormato.Scale5;
        public string ListarElementosObra => _listarElementosObra;
        public string CriarLM => _criarLM;

        public LmAbs<T> ColetorMateriais => _coletorLM;


        public ElementosFixacaoDgt ElementosFixacao => _elementosFixacao;
        public QuadroAplicacaoDgt QuadroAplicacao => _quadroAplicacao;
        public RevisaoDgt Revisao => _revisao;

        public DesenhoDgtAbs(TSD.MultiDrawing multiDrawing, TSM.Model model, CamposFormatoDgt camposFormatoDgt, LmAbs<T> coletorLm)
        {
            _camposFormato = camposFormatoDgt;

            _coletorLM = coletorLm;




            _coletorLM.Coletar(multiDrawing);

            _quadroAplicacao = new QuadroAplicacaoDgt(multiDrawing);
           



            _revisao = new RevisaoDgt(multiDrawing);

        }

        protected void setListarElementosObra(TSD.MultiDrawing multiDrawing)
        {
            //
            // Acessa o Identifier interno do Drawing via Reflection
            PropertyInfo propInfo = multiDrawing.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
            object value = propInfo.GetValue(multiDrawing, null);
            Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

            // Cria um objeto ModelObject temporário com o mesmo Identifier
            Beam tempBeam = new Beam();
            tempBeam.Identifier = identifier;


            //string listarElementosObra = string.Empty;
            tempBeam.GetReportProperty("TCNM_LISTAR_PARAF", ref _listarElementosObra);
            tempBeam.GetReportProperty("TCNM_CRIAR_LM", ref _criarLM);



            bool isDiagrama = false;
            string currentTemplateFile = string.Empty;
            if (tempBeam.GetReportProperty("PADRÃO ARAUCO", ref currentTemplateFile))
            {
                // Verifica se o nome do arquivo contém o seu DIAGRAMA_LM
                if (!string.IsNullOrEmpty(currentTemplateFile) &&
                    currentTemplateFile.IndexOf("DIAGRAMA_LM", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    isDiagrama = true;
                }
            }

        }
    }
}
