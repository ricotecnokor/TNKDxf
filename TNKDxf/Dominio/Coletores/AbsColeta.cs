using System.Collections.Generic;
using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Dominio.Coletores
{
    public abstract class AbsColeta : IColeta
    {
        protected Dictionary<string, string> _objectIdsToDelete = new Dictionary<string, string>();

        protected Formato _formato;

        public Formato Formato { get => _formato; private set => _formato = value; }

        public AbsColeta(Formato formato)
        {
            _formato = formato;
        }

        public abstract void ApagarSelecao();
        
    }
}
