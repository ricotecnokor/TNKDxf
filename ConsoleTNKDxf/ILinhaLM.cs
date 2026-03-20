using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTNKDxf
{
    public interface ILinhaLM
    {
        string Posicao { get; }
        string Quantidade { get; }
        string Descricao { get; }
        string Observacao { get; }
        string Material { get; }
        string Peso { get; }
    }
}
