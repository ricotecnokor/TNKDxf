using System;
using System.Collections;
using System.Collections.Generic;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Entidades
{
    public class ConjuntoLM : Entidade
    {


        TopicoLM _linhaConjunto;
        List<TopicoLM> _linhasItem;

        public ConjuntoLM(TopicoLM linhaConjunto)
        {
            _linhaConjunto = linhaConjunto;
            _linhasItem = new List<TopicoLM>();
        }

        public string Nivel { get; set; }

        public int QtdItens => _linhasItem.Count;

        public List<TopicoLM> LinhasItem { get => _linhasItem; private set => _linhasItem = value; }
        public TopicoLM LinhaConjunto { get => _linhaConjunto; }

        public void AddLinhaItem(TopicoLM linha)
        {
            _linhasItem.Add(linha);
        }

        public void AddLinhaItemADV(TopicoLM linha)
        {
            var valor = linha.Item.Valor.Split('-')[1].Trim();
            linha.Item = new Item(valor);
            _linhasItem.Add(linha);


            var peso = linha.Peso.Valor * _linhaConjunto.Quantidade.Valor;
            _linhaConjunto.Peso.AddPeso(Math.Round(peso, 1));
            _linhaConjunto.Peso = new Peso(Math.Round(_linhaConjunto.Peso.Valor, 1));
        }

        public Hashtable ObterHashTableLinhas()
        {
            Hashtable hashtableMarca = new Hashtable();
            hashtableMarca.Add("MARCA", LinhaConjunto.Item.Valor);
            hashtableMarca.Add("QT", LinhaConjunto.Quantidade.Valor);
            hashtableMarca.Add("DESCRIÇÃO", LinhaConjunto.Descricao != null ? LinhaConjunto.Descricao.Valor : string.Empty);
            hashtableMarca.Add("PESO", LinhaConjunto.Peso.Valor);
            hashtableMarca.Add("CODIGO", LinhaConjunto.Codigo != null ? LinhaConjunto.Codigo.Valor : string.Empty);
            return hashtableMarca;
        }
    }
}
