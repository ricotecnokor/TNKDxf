using System.Collections.Generic;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Enumeracoes;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Construtores
{
    public class FormatoDATABuilder
    {

        List<double> _fatoresEscala;
        Formatacao _formatacaoContexto;
        Ponto2d _deslocamento;
        double _xMax;
        public FormatoDATABuilder(double xMax, Ponto2d deslocamento, Formatacao formatacaoContexto)
        {
            _xMax = xMax;
            _formatacaoContexto = formatacaoContexto;
            _deslocamento = deslocamento;
            _fatoresEscala = new List<double>
            {
                 1, 2, 4, 5, 7.5, 10, 15, 20, 25, 33.3, 50, 75, 100, 150,
                 0.5, 0.25, 0.2
            };



        }

        public Formato Build()
        {

            foreach (double fator in _fatoresEscala)
            {
                var comp = _xMax / fator;

                if (comp.IgualComTolerancia(1051.0, 2.0, fator))
                {
                    return new Formato(fator, FormatoType.DEZA4, _formatacaoContexto, _deslocamento);
                }

                if (comp.IgualComTolerancia(841.0, 2.0, fator))
                {
                    return new Formato(fator, FormatoType.A1, _formatacaoContexto, _deslocamento);
                }

                if (comp.IgualComTolerancia(594.0, 2.0, fator))
                {
                    return new Formato(fator, FormatoType.A2, _formatacaoContexto, _deslocamento);
                }

                if (comp.IgualComTolerancia(421.0, 2.0, fator))
                {
                    return new Formato(fator, FormatoType.A3, _formatacaoContexto, _deslocamento);
                }

                if (comp.IgualComTolerancia(210.0, 2.0, fator))
                {
                    return new Formato(fator, FormatoType.A4, _formatacaoContexto, _deslocamento);
                }


            }

            return null;

        }
    }
}
