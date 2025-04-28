using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Entidades
{
    public class TopicoLM
    {

        public TopicoLM(int indice)
        {
            Indice = indice;
        }


        public void PrencherCampo(Texto textoInput, string nome, int indice)
        {
            switch (nome)
            {
                case "ITEM":
                    Item = textoInput != null || textoInput?.Valor != string.Empty ? new Item(textoInput.Valor) : Item.Default();
                    break;
                case "QUANT.":
                    Quantidade = textoInput != null || textoInput?.Valor != string.Empty ? new Quantidade(textoInput.Valor) : Quantidade.Default();
                    break;
                case "DESCRIÃ‡ÃƒO":
                    Descricao = textoInput != null || textoInput?.Valor != string.Empty ? new Descricao(textoInput.Valor) : Descricao.Default();
                    break;
                case "OBSERVAÃ‡ÃƒO":
                    Observacao = textoInput != null || textoInput?.Valor != string.Empty ? new Observacao(textoInput.Valor) : Observacao.Default();
                    break;
                case "MATERIAL":
                    Material = textoInput != null || textoInput?.Valor != string.Empty ? new Material(textoInput.Valor) : Material.Default();
                    break;
                case "PESO":
                    Peso = textoInput != null || textoInput?.Valor != string.Empty ? new Peso(textoInput.Valor) : Peso.Default();
                    break;
                default:
                    break;
            }
        }


        public int Indice { get; }
        public Item Item { get; set; }
        public Quantidade Quantidade { get; set; }
        public Descricao Descricao { get; set; }
        public Observacao Observacao { get; set; }
        public Material Material { get; set; }
        public Peso Peso { get; set; }
        public Codigo Codigo { get; set; }

        //PARA ITEM
        public string MarcaReferencia { get; set; } = string.Empty;
        public string DescricaoNormal { get; set; } = string.Empty;
        public double Espesssura { get; set; }
        public double Largura { get; set; }
        public double Comprimento { get; set; }
        public string MaterialNormal { get; set; } = string.Empty;
        public int ItemOrdem { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string EspessuraValue { get; set; } = string.Empty;
        public string CodigoPadrao { get; set; } = string.Empty;
        public double SomaItemEspecial { get; set; }

        //PARA PADRAO

        public string PartNumber { get; set; } = string.Empty;

        public string Unidade { get; set; } = string.Empty;
    }
}
