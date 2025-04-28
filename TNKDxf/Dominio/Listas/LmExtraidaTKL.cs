using System.Linq;
using TNKDxf.Dominio.Colecoes;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Listas
{
    public class LmExtraidaTKL : LmExtraida
    {

        //public LmExtraidaTKL(IColetaLista coletaLmTNK, IColetaErros coletaErros) : base(coletaLmTNK)
        //{
        //    _colecaoConjuntos = new ColecaoConjuntos(_coletaLista, this, coletaErros);
        //}

        //public LmExtraidaTKL() : base(coletaLmTNK)
        //{
        //    _colecaoConjuntos = new ColecaoConjuntos(_coletaLista, this, coletaErros);
        //}

        //public override double TamanhoLista => _colecaoConjuntos.TamanhoLista;

        //public override double PesoTotal => _colecaoConjuntos.PesoTotal;

        //public override ColecaoConjuntos ColecaoConjuntos => _colecaoConjuntos;

        //public override string DirecaoLM => _coletaLista.Formato.DirecaoLM;

        //public override double FatorEscala => _coletaLista.Formato.FatorEscala;

        //public Ponto2d PontoInsercaoCabecalho
        //{
        //    get
        //    {
        //        var pontoMaxFormato = _coletaLista.Formato.Tipo.PontoMaximo();
        //        return Ponto2d.CriarComEscala(pontoMaxFormato.X - 10, pontoMaxFormato.Y - 10, _coletaLista.Formato.FatorEscala);
        //    }
        //}

        //public Ponto2d PontoInsercaoLm
        //{
        //    get
        //    {
        //        return Ponto2d.CriarComEscala(_coletaLista.Formato.CantoSuperiorDireitoPrimeiraLinhaLM.X,
        //            _coletaLista.Formato.CantoSuperiorDireitoPrimeiraLinhaLM.Y, _coletaLista.Formato.FatorEscala);
        //    }
        //}





        //public override bool ColetaLinha(Linha linha)
        //{
        //    if (linha.PontoInicial.X.IgualComTolerancia(linha.PontoFinal.X, 0.5, _coletaLista.Formato.FatorEscala))
        //    {
        //        var linhaEncontrada = _linhasVerticais.FirstOrDefault(l => l.PontoInicial.X.IgualComTolerancia(linha.PontoInicial.X, 0.5, _coletaLista.Formato.FatorEscala));
        //        if (linhaEncontrada == null)
        //        {
        //            _linhasVerticais.Add(linha);
        //            return true;
        //        }

        //    }

        //    if (linha.PontoInicial.Y.IgualComTolerancia(linha.PontoFinal.Y, 0.5, _coletaLista.Formato.FatorEscala))
        //    {
        //        var linhaEncontrada = _linhasHorizontais.FirstOrDefault(l => l.PontoInicial.Y.IgualComTolerancia(linha.PontoInicial.Y, 0.5, _coletaLista.Formato.FatorEscala));
        //        if (linhaEncontrada == null)
        //        {
        //            _linhasHorizontais.Add(linha);
        //            return true;
        //        }

        //    }

        //    return false;
        //}



        //protected override void criarConjuntosLM()
        //{

        //    var textosDaLinha = _textos.Where(t =>
        //    t.PontoInsercao.MaiorOuIgual(_coletaLista.Formato.CantoInferiorEsquerdoPrimeiraLinhaLM)
        //    && t.PontoInsercao.MenorOuIgual(_coletaLista.Formato.CantoSuperiorDireitoPrimeiraLinhaLM)).ToList();

        //    int indice = 0;

        //    _colecaoConjuntos.LerLinhas(textosDaLinha, ref indice);

        //    var yBase = _colecaoConjuntos.MoverNaLM();

        //    textosDaLinha = _textos.Where(t =>
        //                    t.PontoInsercao.MaiorOuIgual(Ponto2d.CriarSemEscala(_coletaLista.Formato.CantoInferiorEsquerdoPrimeiraLinhaLM.X, yBase))
        //                    && t.PontoInsercao.MenorOuIgual(Ponto2d.CriarSemEscala(_coletaLista.Formato.CantoSuperiorDireitoPrimeiraLinhaLM.X, yBase + _coletaLista.Formato.EspacoLinhasLM)))
        //                    .ToList();


        //    var textosElementoObra = textosDaLinha.Where(t => t.Valor == "ELEMENTOS DE OBRAS").ToList();

        //    if (textosElementoObra.Any())
        //    {
        //        _colecaoConjuntos.LerLinhas(textosDaLinha, ref indice);
        //    }


        //}




        public override bool AddTexto(Texto texto)
        {
            _textos.Add(texto);
            return true;
        }


    }
}
