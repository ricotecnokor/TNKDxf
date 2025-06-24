using System;
using System.Collections.Generic;
using System.Linq;
using TNKDxf.Dominio.ObjetosValor;
using TNKDxf.Infra;
using TNKDxf.ViewModel;

namespace TNKDxf.Dominio.Dwgs
{
    public class ArquivoDwg
    {
        private string _projeto;
        private string _nomeCompleto;
        private string _nome;
        private List<CampoErroWpf> _erros = new List<CampoErroWpf>();
        public ArquivoDwg(string nomeCompleto, string projeto)
        {
            _nomeCompleto = nomeCompleto;
            _nome = nomeCompleto.Split('\\').Last();
            _projeto = projeto;
        }

        public string Nome { get => _nome; private set => _nome = value; }

        public bool TemErro()
        {
            return _erros.Count > 0;
        }

        public void Converter(string userName)
        {
            
            throw new NotImplementedException();
        }
    }
}
