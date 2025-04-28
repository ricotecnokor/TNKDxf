using System;
using System.Collections.Generic;
using System.Linq;
using TNKDxf.Dominio.Colecoes;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Listas
{
    public abstract class LmExtraida : ILmExtraida
    {

        //protected IColetaLista _coletaLista;
        protected List<Texto> _textos = new List<Texto>();
        protected List<Linha> _linhasHorizontais = new List<Linha>();
        protected List<Linha> _linhasVerticais = new List<Linha>();
        //protected ColecaoConjuntos _colecaoConjuntos;
        protected double _tamanhoLista;
        //protected Formato _formato;

        //public LmExtraida(Formato formato)//IColetaLista coletaLista)
        //{
        //    //_coletaLista = coletaLista;
        //}


        //public abstract double TamanhoLista { get; }

        //public abstract double PesoTotal { get; }



        //public abstract ColecaoConjuntos ColecaoConjuntos { get; }

        //public abstract string DirecaoLM { get; }

        //public abstract double FatorEscala { get; }
        //public Ponto2d PontoInsercaoLm
        //{
        //    get
        //    {
        //        var x = _formato.CantoSuperiorDireitoPrimeiraLinhaLM.X;
        //        var y = _formato.CantoInferiorEsquerdoPrimeiraLinhaLM.Y;
        //        return Ponto2d.CriarComEscala(x, y, _formato.FatorEscala);
        //    }
        //}
        public ConjuntoLM Conjuntos { get; set; }

        public void AddLinhaHorizontal(Linha linha)
        {
            throw new NotImplementedException();
        }

        public void AddLinhaVertical(Linha linha)
        {
            throw new NotImplementedException();
        }

        public abstract bool AddTexto(Texto texto);


        ////public void ApagarSelecao()
        ////{
        ////    _coletaLista.ApagarSelecao();
        ////}

        //public abstract bool ColetaLinha(Linha linha);

        //protected abstract void criarConjuntosLM();



        public bool ContemLinhaHorizontal(double xPontoInicial)
        {
            throw new NotImplementedException();
        }

        public bool ContemLinhaVertical(double xPontoInicial)
        {
            throw new NotImplementedException();
        }

        public bool NaoEncontrouLinhasDaLM()
        {
            throw new NotImplementedException();
        }

        //public bool PodeInserirBlocos()
        //{
        //    return _colecaoConjuntos.PodeInserirBlocos();
        //}

        //public void Processar()
        //{
        //    _coletaLista.Coletar(this);


        //    criarConjuntosLM();
        //}

        //public List<Texto> ObterLinhaPorPontosJanela(Ponto2d ptInferiorEsq, Ponto2d ptSuperiorDir)
        //{
        //    return _textos.Where(t => t.PontoInsercao.MaiorOuIgual(ptInferiorEsq) && t.PontoInsercao.MenorOuIgual(ptSuperiorDir)).ToList();
        //}
    }
}
