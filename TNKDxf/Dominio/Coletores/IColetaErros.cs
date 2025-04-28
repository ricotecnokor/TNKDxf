using System.Collections.Generic;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Coletores
{
    public interface IColetaErros
    {
        List<ErroColetado> ObterErros();

        void InserirErro(ErroColetado erro);

        bool PossuiErros();
    }
}
