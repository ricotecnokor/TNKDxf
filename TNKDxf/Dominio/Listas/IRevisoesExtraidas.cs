using System.Collections.Generic;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Listas
{
    public interface IRevisoesExtraidas : IDataExtraido
    {
        bool AddTexto(Texto texto);

        List<Dictionary<string, string>> ObterRevisoes();

    }
}
