using System.Collections.Generic;
using System.Linq;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Listas;

namespace TNKDxf.Dominio.Dxfs
{
    public class ColecaoDxfs
    {
        private List<ArquivoDxf> _dwgs = new List<ArquivoDxf>();
        public ColecaoDxfs(IEnumerable<string> lista, string projeto)
        {
            _dwgs = lista
                .Select(x => new ArquivoDxf(x, projeto))
                .ToList();

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
