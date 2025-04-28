using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.Colecoes;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Listas
{
    public interface ILmExtraida : IListaExtraida
    {

        //bool ColetaLinha(Linha linha);

        bool NaoEncontrouLinhasDaLM();

        bool AddTexto(Texto texto);

        //double TamanhoLista { get; }
        //double PesoTotal { get; }

        //ColecaoConjuntos ColecaoConjuntos { get; }

        //string DirecaoLM { get; }
        //double FatorEscala { get; }
        //Ponto2d PontoInsercaoLm { get; }

        void AddLinhaHorizontal(Linha linha);

        void AddLinhaVertical(Linha linha);

        bool ContemLinhaVertical(double xPontoInicial);
        bool ContemLinhaHorizontal(double xPontoInicial);
        //List<Texto> ObterLinhaPorPontosJanela(Ponto2d ptInferiorEsq, Ponto2d ptSuperiorDir);
    }
}
