using System;
using System.Collections.Generic;

namespace ConsoleAppLista.Infra.Dtos
{
    public class ProdutoPadraoDTO
    {
        public string PartNumber { get; set; }
        public string Descricao { get; set; }
        public DateTime UltimaModificacao { get; set; }

        private List<string> Conjuntos { get; set; }

     
        public void AddConjunto(string conjunto)
        {
            if (Conjuntos == null)
            {
                Conjuntos = new List<string>();
            }
            Conjuntos.Add(conjunto);
        }
    }
}
