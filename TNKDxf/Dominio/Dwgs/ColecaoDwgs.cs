using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TNKDxf.Dominio.Dwgs
{
    public class ColecaoDwgs
    {
        private List<ArquivoDwg> _dwgs = new List<ArquivoDwg>();
        public ColecaoDwgs(string caminho, string projeto)
        {
            DirectoryInfo di = new DirectoryInfo(caminho);

            FileInfo[] arquivos = di.GetFiles("*.dwg");


            foreach (FileInfo fi in arquivos)
            {
                ArquivoDwg arquivoDxf = new ArquivoDwg(fi.FullName, projeto);

                //arquivoDxf.Validar();
                _dwgs.Add(arquivoDxf);
            }

        }

        public IEnumerable<ArquivoDwg> ObterArquivosCertos()
        {
            return _dwgs.Where(x => !x.TemErro());
        }

        public ArquivoDwg ObterArquivoDwg(string nome)
        {
            return _dwgs.Find(x => x.Nome == nome);
        }
    }
}
