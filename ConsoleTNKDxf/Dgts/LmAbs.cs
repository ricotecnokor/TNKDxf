using ConsoleTNKDxf.Abstracoes;
using System.Collections;
using System.Collections.Generic;
using Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public abstract class LmAbs<T> : IEnumerable<T> where T : ConjuntoAbstrato
    {
        protected TSM.Model _model;
        protected List<T> _conjuntos;

        public LmAbs(TSM.Model model)
        {
            _model = model;
            _conjuntos = new List<T>();
        }

        public abstract void Coletar(Drawing multiDrawing);

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
