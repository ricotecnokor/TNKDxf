using System.Collections.Generic;
using TNKDxf.ViewModel.Abstracoes;

namespace TNKDxf.Handles
{
    public class MockExtratorDXFs : IExtratorDXFs
    {
    
        List<string> _desenhos;
        bool _foramExtraidos = false;


        public string Xsplot => @"C:\MockPastaPlotagem";

        public MockExtratorDXFs()
        {
            _desenhos = new List<string>();

        }

        public bool ForamExtraidos { get => _foramExtraidos; private set => _foramExtraidos = value; }
        public IEnumerable<object> Desenhos { get; internal set; }
        public IEnumerable<string> Extraidos => _desenhos;

        public void Extrair()
        {

            _desenhos = new List<string>
            {
                "PRJ00200-D-00000 rev0.dxf",
                "PRJ00200-D-00000 rev1.dxf",
                "PRJ00200-D-00022 rev0.dxf"
            };

            _foramExtraidos = true;

        }
    }
}
