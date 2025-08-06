using System;
using TNKDxf.Handles;

namespace TNKDxf.Dominio.Dwgs
{
    public class ArquivoDwg
    {
        private string _projeto;

        private string _commandResult;// = new CommandResult();
        public ArquivoDwg(string commandResult, string projeto)
        {

            _projeto = projeto;
            _commandResult = commandResult;

        }

        public string Nome { get => _commandResult; private set => _commandResult = value; }

        public bool TemErro()
        {
            return HandleCriacaoDxfs.Instancia.VerificarSePossuiErro(_commandResult);
        }

        public void Enviar(string userName)
        {
            throw new NotImplementedException();
        }

        
    }
}
