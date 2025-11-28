using System.Collections.ObjectModel;
using TNKDxf.Dxfs;

namespace TNKDxf.ViewModel
{
    public class ListViewDxfs
    {
        private readonly ColecaoDxfs _colecaoDxfs;

        public ListViewDxfs(ColecaoDxfs colecaoDxfs)
        {
            _colecaoDxfs = colecaoDxfs;
        }

        public ObservableCollection<ArquivoItem> CarregaArquivosItem()
        {
            var lista = new ObservableCollection<ArquivoItem>();

            var enviar = _colecaoDxfs.ObterArquivosCertos();
            foreach (var dxf in enviar)
            {
                lista.Add(new ArquivoItem { Nome = dxf.Nome, Errado = dxf.TemErro(), PodeConverter = true, Aberto = false, Enviado = false });
            }
            return lista;
        }

    }
}
