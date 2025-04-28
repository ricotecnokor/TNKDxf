using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TNKDxf.Dominio.Dxfs;

namespace TNKDxf.Dxfs
{
    public class ColecaoDxfs : IEnumerable<ArquivoDxf>
    {
        
        private List<ArquivoDxf> _dxfs = new List<ArquivoDxf>();
        public ColecaoDxfs(string caminho, string projeto)
        {
            DirectoryInfo di = new DirectoryInfo(caminho);

            FileInfo[] arquivos = di.GetFiles("*.dxf");

            foreach (FileInfo fi in arquivos)
            {
                ArquivoDxf arquivoDxf = new ArquivoDxf(fi.FullName, projeto);
                arquivoDxf.Validar();
                _dxfs.Add(arquivoDxf);
            }

        }

        public bool ContemArquivo(string caminho)
        {
            var dxf = _dxfs.Find(x => x.Nome == caminho);
            return dxf != null;
        }

        public IEnumerator<ArquivoDxf> GetEnumerator()
        {
            return _dxfs.GetEnumerator();
        }

        public IEnumerable<ArquivoDxf> ObterArquivosErrados()
        {
          return  _dxfs.Where(x => x.TemErro());
        }

        public IEnumerable<ArquivoDxf> ObterArquivosCertos()
        {
            return _dxfs.Where(x => !x.TemErro());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        
    }
}
