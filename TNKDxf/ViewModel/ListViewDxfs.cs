using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TNKDxf.Dominio.Dxfs;
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

        //public List<ArquivoDxf> CarregaErrados()
        //{
        //    var lista = new List<ArquivoDxf>();

        //    var corrigir = _colecaoDxfs.ObterArquivosErrados();
        //    foreach (var dxf in corrigir)
        //    {
        //        lista.Add(dxf);
        //    }
        //    return lista;
        //}
    }

    public class ArquivoItem
    {
        public string Nome { get; set; }
        public bool Errado { get; set; }
        public bool Aberto { get; set; } // Para controlar se está aberto ou não
        public bool Enviado { get; set; }
    }
}
