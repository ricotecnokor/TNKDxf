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
                lista.Add(new ArquivoItem { Nome = dxf.Nome, Errado = dxf.TemErro() });
            }
            return lista;
        }

    }

    public class ArquivoItem
    {
        
        public string Nome { get; set; }
        public bool Selecionado { get; set; }
        public bool Errado { get; set; }
        public bool Aberto { get; set; } 
        public bool Enviado { get; set; }
    }
}
