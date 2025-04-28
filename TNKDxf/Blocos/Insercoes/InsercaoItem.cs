using netDxf;
using netDxf.Entities;
using TNKDxf.Dominio.Abstracoes;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Blocos
{
    public class InsercaoItem : Insercao
    {
        Vector3 _ptInsercao;
        double _fatorEscala;
        TopicoLM _item;
        public InsercaoItem(string arquivoOrigem, string nomeBloco, Formato formato, ArquivoDxf arquivoDxf, TopicoLM item, Vector3 ptInsercao, double fatorEscala, IAtributos atributos) : base(arquivoOrigem, nomeBloco, formato, arquivoDxf)
        {
            _item = item;
            _ptInsercao = ptInsercao;
            _fatorEscala = fatorEscala;
        }

        public override Insert Inserir()
        {
            var insert = inserir(_ptInsercao, _fatorEscala);

            var atributosLinhaItem = new AtributosLinhaItem(_arquivoDxf, _item);
            atributosLinhaItem.Atributar(insert);

            return insert;
        }
    }
}
