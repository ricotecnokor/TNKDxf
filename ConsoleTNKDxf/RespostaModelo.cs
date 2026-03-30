using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class RespostaModelo
    {
        private readonly bool _sucesso;
        private readonly TSM.Model _model;
        private readonly string _mensagem;

        public RespostaModelo(bool sucesso, TSM.Model model, string mensagem)
        {
            _sucesso = sucesso;
            _model = model;
            _mensagem = mensagem;
        }

        public bool Sucesso => _sucesso;
        public TSM.Model Model => _model;
        public string Mensagem => _mensagem;
    }
}
