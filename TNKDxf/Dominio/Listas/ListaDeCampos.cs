using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.ObjetosValor;
using TNKDxf.Infra.Dtos;

namespace TNKDxf.Dominio.Listas
{
    public static class ListaDeCampos
    {
        public static List<CampoTexto> Converter(List<CampoFormatoDOC> entrada)
        {
            List<CampoTexto> saida = new List<CampoTexto>();
            foreach (var campo in entrada)
            {
                saida.Add(campo.Converter());
            }
            return saida;
        }
    }
}
