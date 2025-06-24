using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.Dwgs;
using TNKDxf.Dxfs;

namespace TNKDxf.ViewModel
{
    public class ListViewDwgs
    {
        private readonly ColecaoDwgs _colecaoDwgs;

        public ListViewDwgs(ColecaoDwgs colecaoDwgs)
        {
            _colecaoDwgs = colecaoDwgs;
        }

        public ObservableCollection<ArquivoItem> CarregaArquivosItem()
        {
            var lista = new ObservableCollection<ArquivoItem>();

            var enviar = _colecaoDwgs.ObterArquivosCertos();
            foreach (var dxf in enviar)
            {
                lista.Add(new ArquivoItem { Nome = dxf.Nome, Errado = dxf.TemErro() });
            }
            return lista;
        }
    }
}
