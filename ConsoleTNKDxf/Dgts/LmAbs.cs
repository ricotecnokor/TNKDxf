using ConsoleTNKDxf.Abstracoes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public abstract class LmAbs<T> : IEnumerable<T> where T : ConjuntoAbstrato
    {

        protected TSM.Model _model;
        protected List<T> _conjuntos;

        public List<T> Conjuntos => _conjuntos;

        public LmAbs(TSM.Model model)
        {
            _model = model;
            _conjuntos = new List<T>();
        }

        public List<string> ObterPrefixosConjuntos()
        {
            List<string> prefixos = new List<string>(); 
            foreach (var cj in _conjuntos)
            {
                string prefixo = cj.AssemblyPos.TeklaSubstring(0,5) + "-";
                if(!prefixos.Contains(prefixo)) prefixos.Add(prefixo);
            }

            return prefixos;
        }

        public abstract void Coletar(MultiDrawing multiDrawing);

        public IEnumerator<T> GetEnumerator()
        {
            return _conjuntos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
