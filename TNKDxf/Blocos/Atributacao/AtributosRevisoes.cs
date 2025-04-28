using netDxf.Entities;
using System.Collections;
using System.Linq;
using TNKDxf.Dominio.Abstracoes;
using TNKDxf.Dominio.Dxfs;

namespace TNKDxf.Blocos.Atributacao
{
    public class AtributosRevisoes : Atributos
    {
        public AtributosRevisoes(ArquivoDxf arquivoDxf) : base(arquivoDxf)
        {
        }

        //private ColetaRevisoes _coletaRevisoes;

        //public AtributosRevisoes(ColetaRevisoes coletaRevisoes)
        //{
        //    _coletaRevisoes = coletaRevisoes;
        //}

        public override void Atributar(Insert inserido)
        {
            Hashtable hashtableMarca = new Hashtable();

            var revisoes = _arquivoDxf.ObterRevisoes();//_coletaRevisoes.ObterRevisoes();

            for (int i = 0; i < revisoes.Count(); i++)
            {
                foreach (var campoRevisao in revisoes[i])
                {
                    var tag = $"{i + 1}_{campoRevisao.Key}";
                    var valor = campoRevisao.Value;

                    hashtableMarca.Add(tag, valor);
                }
            }

            var id = preencherAtributos(inserido, hashtableMarca);
        }
    }
}
