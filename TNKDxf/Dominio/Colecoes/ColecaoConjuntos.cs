using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Listas;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Colecoes
{
    public class ColecaoConjuntos : IEnumerable<ConjuntoLM>
    {
        //protected List<Texto> _textos = new List<Texto>();
        protected List<ConjuntoLM> _conjuntos = new List<ConjuntoLM>();
        protected IEnumerable<ConjuntoLM> _ienumerableConjuntos;
        //protected ILmExtraida _lmExtraida;
        private ColetaLmTNK _coleta;
        private double _yBase;
        protected double _pesoTotal;
        private double _tamanhoLista;
        IColetaErros _coletaErros;

        public double TamanhoLista { get => _tamanhoLista; private set => _tamanhoLista = value; }
        public double PesoTotal => _pesoTotal;

        public ColecaoConjuntos(ColetaLmTNK coleta, IColetaErros coletaErros)//ILmExtraida lmExtraida, IColetaErros coletaErros)
        {
            //_lmExtraida = lmExtraida;
            _coleta = coleta;
            _yBase = setYBase();
            _coletaErros = coletaErros;
        }

        public void CriarConjuntosLM()
        {
            var textos = _coleta.ObterTextos();

            int indice = 0;

            lerLinhas(textos, ref indice);

            var yBase = moverNaLM();
            textos = _coleta.ObterTextosPorYBase(yBase);

            var textosElementoObra = textos.Where(t => t.Valor == "ELEMENTOS DE OBRAS").ToList();

            if (textosElementoObra.Any())
            {
                lerLinhas(textos, ref indice);
            }


        }

        


        private double setYBase()
        {
            double yBase = 0;
            if (_coleta.Formato.DirecaoLM == "Down")
            {
                yBase = _coleta.Formato.CantoInferiorEsquerdoPrimeiraLinhaLM.Y - _coleta.Formato.EspacoLinhasLM;
            }
            else if (_coleta.Formato.DirecaoLM == "Up")
            {
                yBase = _coleta.Formato.CantoSuperiorDireitoPrimeiraLinhaLM.Y + _coleta.Formato.EspacoLinhasLM;
            }

            return yBase;
        }

        private void lerLinhas(List<Texto> textosDaLinha, ref int indice)
        {
            List<ConjuntoLM> conjuntos = new List<ConjuntoLM>();
            while (textosDaLinha.Any())
            {
                int? corTextosLinha = 0;
                TopicoLM linhaQ = new TopicoLM(indice);
                foreach (var cp in _coleta.Formato.ListaCamposLM)
                {
                    var ptInferiorEsq = Ponto2d.CriarSemEscala(cp.Janela.CantoInferiorEsquerdo.X, _yBase);
                    var ptSuperiorDir = Ponto2d.CriarSemEscala(cp.Janela.CantoSuperiorDireito.X, _yBase + _coleta.Formato.EspacoLinhasLM);

                    string stringResultante = string.Empty;
                    var textosEncontrados = obterLinhaPorPontosJanela(ptInferiorEsq, ptSuperiorDir);
                    if (textosEncontrados.Any())
                    {
                        stringResultante = setTextoEncotrado(ptInferiorEsq, ptSuperiorDir, textosEncontrados);
                        Texto texto = new Texto(cp.Texto.Atributo, cp.Texto.Campo, stringResultante, textosEncontrados.First().PontoInsercao, textosEncontrados.First().IndiceCor);

                        if (textoErrado(texto.Atributo, texto.Valor))
                        {
                            _coletaErros.InserirErro(new ErroColetado(texto.PontoInsercao, texto.Atributo, texto.Valor));
                        }


                        corTextosLinha = texto != null ? texto.IndiceCor : 0;
                        linhaQ.PrencherCampo(texto, cp.Texto.Valor, indice);
                    }
                    else
                    {
                        Texto texto = new Texto("", "", "", cp.Texto.PontoInsercao, corTextosLinha);
                        corTextosLinha = texto != null ? texto.IndiceCor : 0;
                        linhaQ.PrencherCampo(texto, cp.Texto.Valor, indice);
                    }

                }



                bool ehLinhaDeItem = _coleta.Formato.ehLinhaDeItem(corTextosLinha);
                bool ehLinhaDeConjunto = _coleta.Formato.ehLinhaDeConjunto(corTextosLinha);


                if (ehLinhaDeConjunto)
                {
                    linhaQ.Codigo = new Codigo("M");
                    conjuntos.Add(new ConjuntoLM(linhaQ));
                    _pesoTotal = _pesoTotal + linhaQ.Peso.Valor;
                    indice++;
                }

                if (ehLinhaDeItem)
                {
                    linhaQ.Codigo = new Codigo("I");
                    var ultimoConjunto = conjuntos.Last();
                    linhaQ.MarcaReferencia = ultimoConjunto.LinhaConjunto.Item.Valor;
                    ultimoConjunto.AddLinhaItem(linhaQ);
                    indice++;
                }

                moverNaLM();

                textosDaLinha = obterLinhaPorPontosJanela(
                    Ponto2d.CriarSemEscala(_coleta.Formato.CantoInferiorEsquerdoPrimeiraLinhaLM.X, _yBase),
                    Ponto2d.CriarSemEscala(_coleta.Formato.CantoSuperiorDireitoPrimeiraLinhaLM.X, _yBase + _coleta.Formato.EspacoLinhasLM));

                indice++;
            }

            _conjuntos.AddRange(conjuntos);

            _ienumerableConjuntos = _conjuntos;

            int multiplicador = 0;

            foreach (var cj in _conjuntos)
            {
                multiplicador = multiplicador + 1;
                multiplicador = multiplicador + cj.LinhasItem.Count;
            }

            _tamanhoLista = multiplicador * _coleta.Formato.FatorEscala * _coleta.Formato.EspacoLinhasLM;

        }

        public List<Texto> obterLinhaPorPontosJanela(Ponto2d ptInferiorEsq, Ponto2d ptSuperiorDir)
        {
            var textos = _coleta.ObterTodosTextos();
            return textos.Where(t => t.PontoInsercao.MaiorOuIgual(ptInferiorEsq) && t.PontoInsercao.MenorOuIgual(ptSuperiorDir)).ToList();
        }
        private bool textoErrado(string atributo, string valor)
        {
            return valor == "05408AA" ? true : false;
        }

        private double moverNaLM()
        {
            if (_coleta.Formato.DirecaoLM == "Down")
            {

                _yBase = _yBase - _coleta.Formato.EspacoLinhasLM;
            }
            else if (_coleta.Formato.DirecaoLM == "Up")
            {
                _yBase = _yBase + _coleta.Formato.EspacoLinhasLM;
            }

            return _yBase;

        }

        private string setTextoEncotrado(Ponto2d ptInferiorEsq, Ponto2d ptSuperiorDir, List<Texto> textosEncontrados)
        {
            string stringResultante;

            stringResultante = string.Empty;
            if (textosEncontrados.Count() > 1)
            {

                foreach (var txt in textosEncontrados)
                {
                    stringResultante = stringResultante + " " + txt;
                }
            }
            else
            {
                stringResultante = textosEncontrados.First().Valor;
            }

            return stringResultante;
        }

        public bool PodeInserirBlocos()
        {
            return _conjuntos.Count() > 0;
        }

        public IEnumerator<ConjuntoLM> GetEnumerator()
        {
            return _ienumerableConjuntos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
