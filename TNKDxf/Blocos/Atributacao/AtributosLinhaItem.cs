using netDxf.Entities;
using System.Collections;
using TNKDxf.Dominio.Abstracoes;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Blocos
{
    public class AtributosLinhaItem : Atributos
    {

        TopicoLM _topicoLM;

        public AtributosLinhaItem(ArquivoDxf arquivoDxf, TopicoLM topicoLM) : base(arquivoDxf)
        {
            _topicoLM = topicoLM;
        }

        public override void Atributar(Insert inserido)
        {
            Hashtable hashtableItem = new Hashtable();
            hashtableItem.Add("ITEM", _topicoLM.Item != null ? _topicoLM.Item.Valor : string.Empty);
            hashtableItem.Add("QT", _topicoLM.Quantidade != null ? _topicoLM.Quantidade.Valor : 0);
            hashtableItem.Add("DESCRIÇÃO", _topicoLM.Descricao != null ? _topicoLM.Descricao.Valor : string.Empty);
            hashtableItem.Add("OBSERVAÇÃO", _topicoLM.Observacao != null ? _topicoLM.Observacao.Valor : string.Empty);
            hashtableItem.Add("MATERIAL", _topicoLM.Material != null ? _topicoLM.Material.Valor : string.Empty);
            hashtableItem.Add("PESO", _topicoLM.Peso != null ? _topicoLM.Peso.Valor : 0.0);
            hashtableItem.Add("CODIGO", _topicoLM.Codigo != null ? _topicoLM.Codigo.Valor : string.Empty);
            hashtableItem.Add("MARCA_REF", _topicoLM.MarcaReferencia != null ? _topicoLM.MarcaReferencia.Trim() : string.Empty);
            hashtableItem.Add("DESC_NORMAL", _topicoLM.DescricaoNormal != null ? _topicoLM.DescricaoNormal.Trim() : string.Empty);
            hashtableItem.Add("ESPESSURA", _topicoLM.Espesssura != null ? _topicoLM.Espesssura : 0.0);
            hashtableItem.Add("LARGURA", _topicoLM.Largura != null ? _topicoLM.Largura : 0.0);
            hashtableItem.Add("COMPRIMENTO", _topicoLM.Comprimento != null ? _topicoLM.Comprimento : 0.0);
            hashtableItem.Add("MAT_NORMAL", _topicoLM.MaterialNormal != null ? _topicoLM.MaterialNormal.Trim() : string.Empty);
            hashtableItem.Add("IT_ORDER", _topicoLM.ItemOrdem != null ? _topicoLM.ItemOrdem : 0);
            hashtableItem.Add("TIPO", _topicoLM.Tipo != null ? _topicoLM.Tipo.Trim() : string.Empty);
            hashtableItem.Add("ESPESSURAVALUE", _topicoLM.EspessuraValue != null ? _topicoLM.EspessuraValue.Trim() : string.Empty);
            hashtableItem.Add("SOMAITEMESPECIAL", _topicoLM.SomaItemEspecial != null ? _topicoLM.SomaItemEspecial : 0.0);

            preencherAtributos(inserido, hashtableItem);
        }

    }
}
