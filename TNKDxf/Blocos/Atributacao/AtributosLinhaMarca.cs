using netDxf.Entities;
using System.Collections;
using TNKDxf.Dominio.Abstracoes;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Blocos
{
    public class AtributosLinhaMarca : Atributos
    {

        protected const double LARGURA_LINHA = 5.0;
        private ConjuntoLM _conjuntoLM;
        public AtributosLinhaMarca(ArquivoDxf arquivoDxf, ConjuntoLM conjuntoLM) : base(arquivoDxf)
        {
            _conjuntoLM = conjuntoLM;
        }

        //ConjuntoLM _conjuntoLM;
        //public AtributosLinhaMarca(ConjuntoLM conjunto)
        //{
        //    _conjuntoLM = conjunto;

        //}

        public override void Atributar(Insert inserido)
        {
            Hashtable hashtableMarca = _conjuntoLM.ObterHashTableLinhas();

            preencherAtributos(inserido, hashtableMarca);

        }
    }
}
