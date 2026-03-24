using Tekla.Structures.DrawingInternal;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class DesenhoDgt
    {
        private ListaMateriaisDtg _listaMateriais;
        private QuadroAplicacaoDgt _quadroAplicacao;
        private ElementosFixacaoDgt _elementosFixacao;
        private RevisaoDgt _revisao;
        private CamposFormatoDgt _camposFormato;

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

        public ListaMateriaisDtg ListaMateriais => _listaMateriais;
        public ElementosFixacaoDgt ElementosFixacao => _elementosFixacao;
        public QuadroAplicacaoDgt QuadroAplicacao => _quadroAplicacao;
        public RevisaoDgt Revisao => _revisao;

        public DesenhoDgt(TSD.MultiDrawing multiDrawing, TSM.Model model)
        {
            _camposFormato = new CamposFormatoDgt(multiDrawing);
            _listaMateriais = new ListaMateriaisDtg(model, multiDrawing);
            _quadroAplicacao = new QuadroAplicacaoDgt(multiDrawing);
            _elementosFixacao = new ElementosFixacaoDgt(model, multiDrawing);
            _revisao = new RevisaoDgt(multiDrawing);
        }
    }
}
