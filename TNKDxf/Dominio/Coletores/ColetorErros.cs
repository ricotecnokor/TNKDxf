using System.Collections.Generic;
using System.Linq;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Coletores
{
    public class ColetaErros : IColetaErros
    {

        List<ErroColetado> _erroColetados = new List<ErroColetado>();
        public void InserirErro(ErroColetado erro)
        {
            _erroColetados.Add(erro);
        }

        public List<ErroColetado> ObterErros()
        {
            return _erroColetados;
        }

        public bool PossuiErros()
        {
            return _erroColetados.Any();
        }

    }
}
