using System.Collections.Generic;

namespace TNKDxf.ViewModel.Abstracoes
{
    public interface IExtratorDXFs
    {
        void Extrair();
        bool ForamExtraidos { get; }
        IEnumerable<string> Extraidos { get; }

        string Xsplot { get; }
    }
}
