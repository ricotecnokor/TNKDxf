using System;
using System.Collections.Generic;
using System.Linq;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Listas
{
    public class RevisaoExtraida : IRevisoesExtraidas
    {

        protected List<Texto> _textos = new List<Texto>();
        private List<Dictionary<string, string>> _revisoes = new List<Dictionary<string, string>>();
        private IColetaRevisoes _coletaRevisoes;
        protected IColetaErros _coletaErros;

        public RevisaoExtraida(IColetaRevisoes coletaRevisoes, IColetaErros coletaErros)
        {
            _coletaRevisoes = coletaRevisoes;
            _coletaErros = coletaErros;
        }

        public bool AddTexto(Texto texto)
        {
            if (texto.PontoInsercao.MaiorOuIgual(_coletaRevisoes.Formato.LimiteTabelaRevisao.PontoInicial)
                && texto.PontoInsercao.MenorOuIgual(_coletaRevisoes.Formato.LimiteTabelaRevisao.PontoFinal))
            {
                _textos.Add(texto);
                return true;
            }
            return false;
        }
        public void Processar()
        {
            _coletaRevisoes.Coletar(this);
            criarRevisoes();

        }

        public void criarRevisoes()
        {
            if (_textos is null || _textos.Count == 0)
                return;

            var listaYs = _textos.Select(l => Math.Round(l.PontoInsercao.Y, 0)).Distinct().ToList();

            foreach (var y in listaYs)
            {
                var dic = new Dictionary<string, string>();

                var linha = _textos.Where(l => l.PontoInsercao.Y.IgualComTolerancia(y, 1.0, _coletaRevisoes.Formato.FatorEscala)).ToList();

                foreach (var topico in _coletaRevisoes.Formato.LinhaRevisao)
                {
                    var texto = linha.FirstOrDefault(t =>
                            t.PontoInsercao.X >= topico.Janela.CantoInferiorEsquerdo.X * _coletaRevisoes.Formato.FatorEscala
                            && t.PontoInsercao.X <= topico.Janela.CantoSuperiorDireito.X * _coletaRevisoes.Formato.FatorEscala);

                    if (texto != null)
                    {
                        if (textoErrado(texto.Atributo, texto.Valor))
                        {
                            _coletaErros.InserirErro(new ErroColetado(texto.PontoInsercao, texto.Atributo, texto.Valor));
                        }

                        dic.Add(topico.Texto.Valor, texto.Valor);
                    }
                }

                _revisoes.Add(dic);

            }

        }

        private bool textoErrado(string atributo, string valor)
        {
            return valor.Equals("0");
        }

        public bool PodeInserirBlocos()
        {
            return _revisoes.Count > 0;
        }

        public void ApagarSelecao()
        {
            _coletaRevisoes.ApagarSelecao();
        }

        public List<Dictionary<string, string>> ObterRevisoes()
        {
            return _revisoes;
        }
    }
}
