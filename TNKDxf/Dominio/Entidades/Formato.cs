using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Construtores;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Enumeracoes;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.ObjetosValor;
using TNKDxf.Infra;

namespace TNKDxf.Dominio.Entidades
{
    public class Formato
    {
        protected double _fatorEscala;
        protected LimitesGeometricosRetangulo _limiteLM;
        protected LimitesGeometricosRetangulo _limiteFormato;
        protected FormatoType _tipo;
        protected Formatacao _formatacao;
        protected Ponto2d _deslocamento;

        public static Formato Criar(ArquivoDxf arquivoDxf)
        {
            var servico = new ServicoFormatacao();
            var formatacaoDTO = servico.ObterFormatacao(arquivoDxf.ObterProjeto());
            var formatacao = formatacaoDTO.Converter();

            DxfSingleton.Load(arquivoDxf.ObterNomeCompleto());

            Ponto2d extMax = DxfSingleton.DxfDocument.Extmax();

            var formatoDATABuilder = new FormatoDATABuilder(extMax.X, Ponto2d.CriarSemEscala(0.0, 0.0), formatacao);
            return formatoDATABuilder.Build();
        }

        public Formato(double fatorEscala, FormatoType tipo, Formatacao formatacaoContexto, Ponto2d deslocamento)
        {
            _fatorEscala = fatorEscala;
            _tipo = tipo;

            _deslocamento = deslocamento;

            _formatacao = formatacaoContexto;

            var limiteBaseLM = new LimitesGeometricosRetangulo(
                    Ponto2d.CriarComEscala(
                        formatacaoContexto.CantoInferiorEsquerdoPrimeiraLinhaLM.X,
                    formatacaoContexto.CantoInferiorEsquerdoPrimeiraLinhaLM.Y,
                    fatorEscala),
                    Ponto2d.CriarComEscala(
                        formatacaoContexto.CantoSuperiorDireitoTituloCabecalhoLM.X,
                        formatacaoContexto.CantoSuperiorDireitoTituloCabecalhoLM.Y,
                        fatorEscala));
            _limiteLM = deslocaLimite(deslocamento, limiteBaseLM);


            var limiteBaseFormato = setLimiteFormato(tipo, fatorEscala);
            _limiteFormato = deslocaLimite(deslocamento, limiteBaseFormato);

        }

        public Ponto2d PontoInsercaoCabecalho
        {
            get
            {
                var pontoMaxFormato = Tipo.PontoMaximo();
                return Ponto2d.CriarComEscala(pontoMaxFormato.X - 10, pontoMaxFormato.Y - 10, FatorEscala);
            }
        }

        

        public Ponto2d PontoInsercaoLm
        {
            get
            {
                var x = CantoSuperiorDireitoPrimeiraLinhaLM.X;
                var y = CantoInferiorEsquerdoPrimeiraLinhaLM.Y;
                return Ponto2d.CriarComEscala(x, y, FatorEscala);
            }
        }



        public double FatorEscala { get => _fatorEscala; protected set => _fatorEscala = value; }
        public LimitesGeometricosRetangulo LimiteLM { get => _limiteLM; protected set => _limiteLM = value; }
        public FormatoType Tipo { get => _tipo; protected set => _tipo = value; }
        public LimitesGeometricosRetangulo LimiteFormato { get => _limiteFormato; set => _limiteFormato = value; }

        public List<CampoTexto> ListaCamposLM
        {
            get
            {
                var lista = new List<CampoTexto>();
                foreach (var campo in _formatacao.LinhaLM)
                {
                    lista.Add(ajustaCampo(campo));
                }
                return lista;
            }
        }

        public double EspacoLinhasLM
        {
            get
            {
                return _formatacao.EspacoLinhasLM * _fatorEscala;
            }
        }

        public bool ListaDescendente
        {
            get
            {
                switch (_formatacao.DirecaoLM)
                {
                    case "Down":
                        return true;
                    case "Up":
                        return false;
                    default:
                        return true;
                }

            }
        }

        public double YLimiteAbaixoCabecalho
        {
            get
            {
                return ajustaPonto(_formatacao.CantoInferiorEsquerdoPrimeiraLinhaLM).Y;
            }
        }

        public string DirecaoLM
        {
            get
            {
                return _formatacao.DirecaoLM;
            }
        }

        public Ponto2d CantoInferiorEsquerdoPrimeiraLinhaLM
        {
            get
            {
                var pt = _formatacao.CantoInferiorEsquerdoPrimeiraLinhaLM;
                return Ponto2d.CriarComEscala(pt.X, pt.Y, _fatorEscala);
            }
        }
        public Ponto2d CantoSuperiorDireitoPrimeiraLinhaLM
        {
            get
            {
                var pt = _formatacao.CantoSuperiorDireitoPrimeiraLinhaLM;
                return Ponto2d.CriarComEscala(pt.X, pt.Y, _fatorEscala);
            }
        }

        public List<CampoTexto> CamposFormato
        {
            get
            {
                return _formatacao.CamposFormato;
            }
        }

        public List<CampoTexto> LinhaRevisao
        {
            get
            {
                return _formatacao.LinhaRevisao;
            }
        }

        public string PadraoFormato
        {
            get
            {
                return _formatacao.Formato;
            }
        }

        public LimitesGeometricosRetangulo LimiteTabelaRevisao
        {
            get
            {
                var xMin = _formatacao.LinhaRevisao.Min(l => l.Janela.CantoInferiorEsquerdo.X);
                var yMin = _formatacao.LinhaRevisao.Min(l => l.Janela.CantoInferiorEsquerdo.Y);
                var cantoInferiorDireitoPrimeiraLinha = Ponto2d.CriarComEscala(xMin, yMin, _fatorEscala);
                var xMax = _formatacao.LinhaRevisao.Max(l => l.Janela.CantoSuperiorDireito.X);
                var yMax = _formatacao.LinhaRevisao.Max(l => l.Janela.CantoSuperiorDireito.Y) + _formatacao.EspacoLinhasRevisao * (_formatacao.QtdLinhasRevisao - 2);
                var cantoSuperiorDireitoPrimeiraLinha = Ponto2d.CriarComEscala(xMax, yMax, _fatorEscala);

                var limite = new LimitesGeometricosRetangulo(cantoInferiorDireitoPrimeiraLinha, cantoSuperiorDireitoPrimeiraLinha);
                return limite;
            }
        }

        private CampoTexto ajustaCampo(CampoTexto campo)
        {
            var campoResultado = new CampoTexto
            (
                new Texto(campo.Texto.Valor, campo.Texto.Valor, campo.Texto.Valor, ajustaPonto(campo.Texto.PontoInsercao), campo.Texto.IndiceCor),
                new Janela(ajustaPonto(campo.Janela.CantoInferiorEsquerdo), ajustaPonto(campo.Janela.CantoSuperiorDireito))
            );

            return campoResultado;
        }

        private Ponto2d ajustaPonto(Ponto2d pontoInsercao)
        {
            var pt = Ponto2d.CriarComEscala(pontoInsercao.X, pontoInsercao.Y, _fatorEscala);

            return somaPontos(pt, _deslocamento);

        }

        public Ponto2d ObterPontoInferiorEsquerdoTabelaRevisao(double fatorEscala)
        {
            Ponto2d pontoMenor = Ponto2d.CriarComEscala(_formatacao.LinhaRevisao.First().Janela.CantoInferiorEsquerdo.X,
                                             _formatacao.LinhaRevisao.First().Janela.CantoInferiorEsquerdo.Y,
                                             fatorEscala);
            foreach (var campo in _formatacao.LinhaRevisao)
            {
                var pontoAvaliado = Ponto2d.CriarComEscala(campo.Janela.CantoInferiorEsquerdo.X,
                                                campo.Janela.CantoInferiorEsquerdo.Y,
                                                fatorEscala);
                pontoMenor = pontoAvaliado.MenorOuIgual(pontoMenor) ? pontoAvaliado : pontoMenor;
            }

            return pontoMenor;
        }

        public Ponto2d ObterPontoSuperiorDireitoTabelaRevisao(double fatorEscala)
        {
            Ponto2d pontoMaior = Ponto2d.CriarComEscala(_formatacao.LinhaRevisao.First().Janela.CantoSuperiorDireito.X,
                                             _formatacao.LinhaRevisao.First().Janela.CantoSuperiorDireito.Y,
                                             fatorEscala);
            foreach (var campo in _formatacao.LinhaRevisao)
            {
                var pontoAvaliado = Ponto2d.CriarComEscala(campo.Janela.CantoSuperiorDireito.X, campo.Janela.CantoSuperiorDireito.Y, fatorEscala);
                pontoMaior = pontoAvaliado.MaiorOuIgual(pontoMaior) ? pontoAvaliado : pontoMaior;
            }

            return Ponto2d.CriarComEscala(pontoMaior.X, pontoMaior.Y * _formatacao.QtdLinhasRevisao, fatorEscala);
        }

        private static LimitesGeometricosRetangulo deslocaLimite(Ponto2d deslocamento, LimitesGeometricosRetangulo limiteBase)
        {
            return new LimitesGeometricosRetangulo(
                somaPontos(limiteBase.PontoInicial, deslocamento),
                somaPontos(limiteBase.PontoFinal, deslocamento)
                );
        }

        private static Ponto2d somaPontos(Ponto2d ptBase, Ponto2d deslocamento)
        {
            return Ponto2d.CriarSemEscala(ptBase.X + deslocamento.X, ptBase.Y + deslocamento.Y);
        }

        public LimitesGeometricosRetangulo setLimiteFormato(FormatoType tipo, double fatorEscala)
        {
            switch (tipo.Name)
            {
                case "A1":
                    return new LimitesGeometricosRetangulo(Ponto2d.CriarComEscala(0.0, 0.0, fatorEscala), Ponto2d.CriarComEscala(841.0, 594.0, fatorEscala));
                case "A2":
                    return new LimitesGeometricosRetangulo(Ponto2d.CriarComEscala(0.0, 0.0, fatorEscala), Ponto2d.CriarComEscala(594.0, 420.0, fatorEscala));
                case "A3":
                    return new LimitesGeometricosRetangulo(Ponto2d.CriarComEscala(0.0, 0.0, fatorEscala), Ponto2d.CriarComEscala(420.0, 297.0, fatorEscala));
                case "A4":
                    return new LimitesGeometricosRetangulo(Ponto2d.CriarComEscala(0.0, 0.0, fatorEscala), Ponto2d.CriarComEscala(297.0, 210.0, fatorEscala));
                case "10A4":
                    return new LimitesGeometricosRetangulo(Ponto2d.CriarComEscala(0.0, 0.0, fatorEscala), Ponto2d.CriarComEscala(1141.0, 594.0, fatorEscala));
                default:
                    return new LimitesGeometricosRetangulo(Ponto2d.CriarComEscala(0.0, 0.0, fatorEscala), Ponto2d.CriarComEscala(841.0, 594.0, fatorEscala));
            }
        }

        public bool ehLinhaDeItem(int? indiceCor)
        {
            return indiceCor != null ? _formatacao.LinhaLM.First().Texto.IndiceCor == indiceCor : false;
        }

        public bool ehLinhaDeConjunto(int? indiceCor)
        {
            return indiceCor != null ? _formatacao.LinhaLM.First().Texto.IndiceCor != indiceCor : false;
        }

        //public string[] ObterCampo(Texto texto)
        //{
        //    var possivelValorCampo = texto.Valor;
        //    foreach (var c in _formatacao.CamposFormato)
        //    {
        //        var cantoInferiorEsquerdoRef = c.Janela.CantoInferiorEsquerdo;
        //        var cantoSuperiorDireitoRef = c.Janela.CantoSuperiorDireito;

        //        var pontoInsercaoRef_X = c.Texto.PontoInsercao.X;
        //        var pontoInsercaoRef_Y = c.Texto.PontoInsercao.Y;
        //        var pontoTexto_X = texto.PontoInsercao.X;
        //        var pontoTexto_Y = texto.PontoInsercao.Y;

        //        var nomeCampo = c.Texto.Valor;


        //        if (cantoInferiorEsquerdoRef.MenorOuIgual(texto.PontoInsercao))
        //        {
        //            if (cantoSuperiorDireitoRef.MaiorOuIgual(texto.PontoInsercao))
        //            {
        //                var deu = texto.Valor;
        //                if (c.Texto.PontoInsercao.IgualComTolerancia(texto.PontoInsercao, 3.0))
        //                {
        //                    ColetorErros.Instance.AddErro(new Erro($"Erro no campo {nomeCampo} com valor {possivelValorCampo}", texto.PontoInsercao));
        //                    return new string[] { nomeCampo, possivelValorCampo };


        //                }
        //            }
        //        }


        //    }

        //    return null;

        //}


    }
}
