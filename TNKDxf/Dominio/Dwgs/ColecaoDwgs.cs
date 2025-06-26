using System.Collections.Generic;
using System.IO;
using System.Linq;
using TNKDxf.Infra.Dtos;

namespace TNKDxf.Dominio.Dwgs
{
    public class ColecaoDwgs
    {
        private List<ArquivoDwg> _dwgs = new List<ArquivoDwg>();
        public ColecaoDwgs(List<ArquivoDTO> lista, string projeto)
        {
            //DirectoryInfo di = new DirectoryInfo(caminho);

            //FileInfo[] arquivos = di.GetFiles("*.dwg");


            foreach (var arquivo in lista)
            {
                ArquivoDwg arquivoDxf = new ArquivoDwg(arquivo, projeto);

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
