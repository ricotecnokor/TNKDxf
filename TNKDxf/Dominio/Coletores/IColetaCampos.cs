using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Listas;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Coletores
{
    public interface IColetaCampos
    {
        Formato Formato { get; }

        void ApagarSelecao();
        void Coletar(CampoExtraido campos);
        string[] ObterCampo(Texto texto);
    }
}
