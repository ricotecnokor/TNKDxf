using System.Collections.Generic;

namespace ConsoleTNKDxf.EstruturaDxf
{
    public class Marca
    {
        public List<Linha> LinhasLider { get; set; } = new List<Linha>();

        public void AddLinhaLider(Linha linha)
        {
            LinhasLider.Add(linha);
        }
    }
}
