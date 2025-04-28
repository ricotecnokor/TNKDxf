using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNKDxf.Dominio.ObjetosValor
{
    public class Item
    {
        public Item(string input)
        {
            Valor = input.Trim();
        }

        public string Valor { get; private set; }

        public override string ToString()
        {
            return "ITEM";
        }

        public static Item Default()
        {
            return new Item("");
        }
    }
}
