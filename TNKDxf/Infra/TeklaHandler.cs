using Dynamic.Tekla.Structures;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf.Infra
{
    public class TeklaHandler
    {
        TSM.Model _model;
        private string _exportPath;
        private string _projeto;
        private string _userName;

        public string ExportPath { get => _exportPath; private set => _exportPath = value; }
        public string Projeto { get => _projeto; private set => _projeto = value; }
        public string UserName { get => _userName; private set => _userName = value; }

        public void Iniciar()
        {
            _model = new TSM.Model();
            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);
            _exportPath = System.IO.Path.Combine(_model.GetInfo().ModelPath, xsplot.Substring(2, xsplot.Length - 2));
            _userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            _projeto = _model.GetProjectInfo().ProjectNumber;
        }
    }
}
