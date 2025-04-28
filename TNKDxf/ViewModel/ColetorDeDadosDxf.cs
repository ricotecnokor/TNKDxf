using System.Collections.Generic;
using System.Linq;
using TNKDxf.Dominio.Abstracoes;
using TNKDxf.Dominio.Colecoes;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Construtores;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.ObjetosValor;
using TNKDxf.Infra;

namespace TNKDxf.Coletas
{
    public class ColetorDeDadosDxf : IColetorDeDadosDxf
    {
        //private Formatacao _formatacao;
        //protected Formato _formato;
        protected ColetaErros _coletorDeErros;
        private ColetaLmTNK _coletaLista;
        private ColecaoConjuntos _colecaoConjuntos;
        private CamposFormato _camposFormato;
        private ColetaRevisoes _coletaRevisoes;
        public void ColetarDados(ArquivoDxf arquivoDxf)//string projeto, string filePath)
        {
            var servico = new ServicoFormatacao();
            var formatacaoDTO = servico.ObterFormatacao(arquivoDxf.ObterProjeto());
            var formatacao = formatacaoDTO.Converter();

            DxfSingleton.Load(arquivoDxf.ObterNomeCompleto());

            Ponto2d extMax = DxfSingleton.DxfDocument.Extmax();


            var formatoDATABuilder = new FormatoDATABuilder(extMax.X, Ponto2d.CriarSemEscala(0.0, 0.0), formatacao);
            var formato = formatoDATABuilder.Build();

            _coletorDeErros = new ColetaErros();

            _coletaLista = new ColetaLmTNK(formato);
            _coletaLista.Coletar();
            _colecaoConjuntos = new ColecaoConjuntos(_coletaLista, _coletorDeErros);
            _colecaoConjuntos.CriarConjuntosLM();

            _camposFormato = new CamposFormato(formato, _coletorDeErros);
            _camposFormato.Coletar();

            _coletaRevisoes = new ColetaRevisoes(formato, _coletorDeErros);
            _coletaRevisoes.Coletar();
            _coletaRevisoes.CriarRevisoes();

            arquivoDxf.AcrescentarErros(_coletorDeErros.ObterErros());



        }

        public void ApagarSelecao()
        {
            _coletaLista.ApagarSelecao();
            _coletaRevisoes.ApagarSelecao();
            _camposFormato.ApagarSelecao();
        }

        public double ObterPesoTotalDaLM()
        {
            return _colecaoConjuntos.PesoTotal;
        }

        public double ObterTamanhoDaLista()
        {
            return _colecaoConjuntos.TamanhoLista;
        }

        public IEnumerable<ConjuntoLM> ObterConjuntos()
        {
            return _colecaoConjuntos;
        }

        public List<string[]> ObterCamposDoFormato()
        {
            var campos = _camposFormato.ObterCampos()
                                       .GroupBy(c => c[0])
                                       .Select(g => g.First());
            return campos.ToList();
        }

        public List<Dictionary<string, string>> ObterRevisoes()
        {
           return _coletaRevisoes.ObterRevisoes();
        }
    }

}
