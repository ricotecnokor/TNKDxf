using Dynamic.Tekla.Structures;

namespace TNKDxf.Infra
{
    public class MockTeklaHandler
    {
        private string _exportPath;
        private string _projeto;
        private string _userName;

        public string ExportPath { get => _exportPath; private set => _exportPath = value; }
        public string Projeto { get => _projeto; private set => _projeto = value; }
        public string UserName { get => _userName; private set => _userName = value; }

        public void Iniciar()
        {
            _exportPath = @"C:\TesteDxf\semprocessar";
            _userName = "usuario_teste";
            _projeto = "PRJ00200";
        }
    }
}
