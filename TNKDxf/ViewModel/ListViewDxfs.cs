using System.Collections;
using System.Collections.Generic;
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

        public List<ArquivoDxf> CarregaCertos()
        {
            var lista = new List<ArquivoDxf>();

            var enviar = _colecaoDxfs.ObterArquivosCertos();
            foreach (var dxf in enviar)
            {
                lista.Add(dxf);
            }
            return lista;
        }

        public List<ArquivoDxf> CarregaErrados()
        {
            var lista = new List<ArquivoDxf>();

            var corrigir = _colecaoDxfs.ObterArquivosErrados();
            foreach (var dxf in corrigir)
            {
                lista.Add(dxf);
            }
            return lista;
        }
    }

    public class LinhaTabelaDxfs
    {
        public string Desenho { get; set; }
        public bool Errado { get; set; }
    }
}
