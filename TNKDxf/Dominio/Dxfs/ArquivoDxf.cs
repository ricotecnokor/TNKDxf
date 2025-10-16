using Dynamic.Tekla.Structures;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TNKDxf.Blocos;
using TNKDxf.Blocos.Atributacao;
using TNKDxf.Coletas;
using TNKDxf.Dominio.Abstracoes;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.ObjetosValor;
using TNKDxf.ViewModel;

namespace TNKDxf.Dominio.Dxfs
{
    public class ArquivoDxf 
    {
        protected const string ARQUIVO_ORIGEM = @"C:\BlocosTecnoedCSN\BLOCOS.dxf";
        protected const string BLOCO_CABECALHO = @"CABECALHO_LISTA";

        protected const string BLOCO_FORMATO_VALE = @"A1_VALE";

        private string _projeto;
        private string _nomeCompleto;
        private string _nome;
        private Formato _formato;

        IColetorDeDadosDxf _coletorDeDadosDxf = new ColetorDeDadosDxf();
        private List<CampoErroWpf> _erros = new List<CampoErroWpf>();

       
        public ArquivoDxf(string nomeCompleto, string projeto)
        {
            _nomeCompleto = nomeCompleto;
            _nome = nomeCompleto.Split('\\').Last();
            _projeto = projeto;
            _formato = Formato.Criar(this);
        }

        public string Nome { get => _nome; private set => _nome = value; }

        public bool TemErro()
        {
            return _erros.Count > 0;
        }

        public Formato ObterFormato()
        {
            return _formato;
        }

        public void Validar()
        {
           var valido = preValidar();
            if(valido)
            {
                validarInternamente();
            }
        }

        internal void AcrescentarErros(List<ErroColetado> erroColetados)
        {
            foreach (var erro in erroColetados)
            {
                _erros.Add(new CampoErroWpf(erro.Campo, erro.Descricao));
            }
        }

        internal string ObterNomeCompleto()
        {
            return _nomeCompleto;
        }

        internal string ObterProjeto()
        {
            return _projeto;
        }

        public double ObterTamanhoDaLista()
        {
            return _coletorDeDadosDxf.ObterTamanhoDaLista();
        }

        private bool preValidar()
        {
            var nome = _nomeCompleto.Split('\\').Last();
            string pattern = @"^PRJ(?<codigo>\d{5})-(?<tipo>[A-Z])-(?<numero>\d{5})\srev(?<revisao>\d+|[A-Za-z]+)\.dxf$";
            Match match = Regex.Match(nome, pattern);

            if (!match.Success)
            {
                _erros.Add(new CampoErroWpf("lbl01", "Nome do arquivo não está no padrão esperado"));
                return false;
            }

            var split = nome.Split('-');
            if (split[0] != _projeto)
            {
                _erros.Add(new CampoErroWpf("lbl01", "Número do projeto está errado"));
                return false;
            }

            return true;
        }

        private void validarInternamente()
        {

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            _coletorDeDadosDxf.ColetarDados(this);
            
        }

        public IEnumerable<ConjuntoLM> ObterConjuntos()
        {
            return _coletorDeDadosDxf.ObterConjuntos();
        }

        public double ObterPesoTotalDaLM()
        {
            return _coletorDeDadosDxf.ObterPesoTotalDaLM();
        }

        public List<string[]> ObterCamposDoFormato()
        {
            return _coletorDeDadosDxf.ObterCamposDoFormato();
        }

        public List<Dictionary<string, string>> ObterRevisoes()
        {
            return _coletorDeDadosDxf.ObterRevisoes();
        }

        public void ApagarSelecao()
        {
            _coletorDeDadosDxf.ApagarSelecao();
        }

        public void Converter(string usuario)
        {

            var insercaoCabecalho =
                new InsercaoCabecalho(ARQUIVO_ORIGEM, BLOCO_CABECALHO, _formato, this);
            insercaoCabecalho.Inserir();

            var insercaoFormato = new InsercaoFormato(ARQUIVO_ORIGEM, BLOCO_FORMATO_VALE, _formato, this);
            var insertFormato = insercaoFormato.Inserir();



            var atributosCampos = new AtributosCampos(this);
            atributosCampos.Atributar(insertFormato);



            var atributosRevisoes = new AtributosRevisoes(this); 
            atributosRevisoes.Atributar(insertFormato);



            this.ApagarSelecao();

            Encaminhamento encaminhamento = new Encaminhamento(this, usuario);

            var caminhoSalvamento = encaminhamento.Encaminhar(@"C:\GitCAD");

            if (File.Exists(caminhoSalvamento))
            {
                File.Delete(caminhoSalvamento);
            }

            DxfSingleton.DxfDocument.Save(caminhoSalvamento);
        }
    }

}
