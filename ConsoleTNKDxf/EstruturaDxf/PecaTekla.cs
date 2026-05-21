using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTNKDxf.EstruturaDxf
{
    public class PecaTekla
    {
        List<Linha> _linhas = new List<Linha>();
        public List<Linha> Linhas => _linhas;

        public void AddLinha(Linha linha)
        {
            _linhas.Add(linha);
        }

        public void AddArestasFormatadas(List<Linha> polilinhaFormato)
        {
            _linhas.AddRange(polilinhaFormato);
        }
    }
}
