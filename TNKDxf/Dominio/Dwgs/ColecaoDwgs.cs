using System;
using System.Collections.Generic;
using System.Linq;
using TNKDxf.Handles;
using TNKDxf.ViewModel;

namespace TNKDxf.Dominio.Dwgs
{
    public class ColecaoDwgs
    {
        private List<ArquivoDwg> _dwgs = new List<ArquivoDwg>();
        public ColecaoDwgs(IEnumerable<string> lista, string projeto)
        {
            _dwgs = lista
                .Select(x => new ArquivoDwg(x, projeto))
                .ToList();

        }


        public ArquivoDwg ObterArquivoDwg(string nome)
        {
            return _dwgs.Find(x => x.Nome == nome);
        }

        public IEnumerable<ArquivoDwg> ObterArquivosErrados()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ArquivoItem> ObterArquivos()
        {
            return _dwgs
                .Select(x => new ArquivoItem { Nome = x.Nome, Errado = x.TemErro() })
                .ToList();
        }

        public int ObterIndice(string nome)
        {
            return _dwgs.FindIndex(x => x.Nome == nome);
        }
    }
}
