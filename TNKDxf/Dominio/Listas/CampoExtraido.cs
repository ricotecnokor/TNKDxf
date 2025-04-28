using System;
using System.Collections.Generic;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Listas
{
    public class CampoExtraido : ICamposExtraidos
    {
        public LimitesGeometricosRetangulo LimiteFormato => throw new NotImplementedException();

        public Formato Formato => _coletaCampos.Formato;

        private IColetaCampos _coletaCampos;
        private List<string[]> _textos = new List<string[]>();
        public CampoExtraido(IColetaCampos coletaCampos)
        {
            _coletaCampos = coletaCampos;
        }

        public bool AddTexto(Texto texto)
        {
            var valores = _coletaCampos.ObterCampo(texto);

            if (valores != null)
            {
                _textos.Add(valores);
                return true;
            }
            return false;
        }

        public void ApagarSelecao()
        {
            _coletaCampos.ApagarSelecao();
        }

        public List<string[]> ObterCampos()
        {
            return _textos;
        }

        public string ObterValorAtributo(string nomeAtributo)
        {
            throw new NotImplementedException();
        }

        public bool PodeInserirBlocos()
        {
            return _textos.Count > 0;
        }

        public void Processar()
        {
            _coletaCampos.Coletar(this);
        }


    }
}
