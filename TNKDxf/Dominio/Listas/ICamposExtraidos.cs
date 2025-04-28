using System.Collections.Generic;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Listas
{
    public interface ICamposExtraidos : IDataExtraido
    {
        LimitesGeometricosRetangulo LimiteFormato { get; }
        Formato Formato { get; }

        bool AddTexto(Texto texto);
        List<string[]> ObterCampos();
        string ObterValorAtributo(string nomeAtributo);

    }
}
