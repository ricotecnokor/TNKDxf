using System;
using System.Collections.ObjectModel;
using TNKDxf.Dominio.Dwgs;

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
