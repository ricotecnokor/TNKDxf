using System.Collections.Generic;

namespace ConsoleAppLista.Infra.Dtos
{
    public class ConjuntoPadraoDTO
    {
        public string PartNumber { get; set; }
        public string Descricao { get; set; }
        public List<string> Produtos { get; set; } // Lista de materiais utilizados no conjunto

        public List<string> Pecas { get; set; } 

        public void AddPeca(string peca)
        {
            if (Pecas == null)
            {
                Pecas = new List<string>();
            }

            Pecas.Add(peca);
        }

        public void AddProduto(string produto)
        {
            if (Produtos == null)
            {
                Produtos = new List<string>();
            }

            Produtos.Add(produto);
        }
    }
}
