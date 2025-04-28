using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Listas;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Coletores
{
    public abstract class ColetaLista : AbsColeta //, IColetaLista
    {
        protected List<Linha> _linhasHorizontais = new List<Linha>();
        protected List<Linha> _linhasVerticais = new List<Linha>();
        protected ColetaLista(Formato formato) : base(formato)
        {
        }

        public int QtdConjuntos => throw new NotImplementedException();

        //public abstract void Coletar(ILmExtraida lm);
    }
}
