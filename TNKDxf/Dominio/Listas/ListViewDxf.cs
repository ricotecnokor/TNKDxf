using System.Collections.ObjectModel;
using TNKDxf.Dominio.Dxfs;

namespace TNKDxf.Dominio.Listas
{
    public class ListViewDxf
    {
        private readonly ColecaoDxfs _colecaoDwgs;

        public ListViewDxf(ColecaoDxfs colecaoDwgs)
        {
            _colecaoDwgs = colecaoDwgs;
        }

        public ObservableCollection<ArquivoItem> CarregaArquivosItem()
        {
            var lista = new ObservableCollection<ArquivoItem>();

            var enviar = _colecaoDwgs.ObterArquivos(); //.ObterArquivosCertos();
            foreach (var dxf in enviar)
            {
                ArquivoItem arquivo = new ArquivoItem
                {
                    Nome = dxf.Nome,
                    Errado = false,
                    Aberto = false,
                    Enviado = false,
                    PodeConverter = true,
                    PodeBaixar = false
                };
                lista.Add(arquivo);
            }
            return lista;
        }

        public int ObterIndice(ArquivoItem arquivo)
        {
            return _colecaoDwgs.ObterIndice(arquivo.Nome);
        }
    }
}
