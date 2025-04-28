using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNKDxf.Dominio.ObjetosValor
{
    public class ErroColetado
    {
        private Ponto2d _ponto2D;
        private string _descricao;
        private string _campo;

        public ErroColetado(Ponto2d ponto2D, string campo, string _valor)
        {
            _campo = campo;
            _ponto2D = ponto2D;
            _descricao = $"O campo {_campo}  está com o valor {_valor} inadequado";
        }

        public Ponto2d Ponto2D { get => _ponto2D; private set => _ponto2D = value; }
        public string Descricao { get => _descricao; private set => _descricao = value; }
        public string Campo { get => _campo; private set => _campo = value; }
    }
}
