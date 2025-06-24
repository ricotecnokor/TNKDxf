using System.Linq;
using System.Windows;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dxfs;
using TNKDxf.Infra;
using TNKDxf.TestesViabilidade;
using TNKDxf.ViewModel;
using TSD = Dynamic.Tekla.Structures.Drawing;
using TSM = Dynamic.Tekla.Structures.Model;


namespace TNKDxf
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        //protected const string ARQUIVO_ORIGEM = @"C:\BlocosTecnoedCSN\BLOCOS.dxf";

        //protected const string BLOCO_CABECALHO = @"CABECALHO_LISTA";
        //protected const string BLOCO_MARCA = @"ITEM_LISTA0";
        //protected const string BLOCO_ITEM_LISTA1 = @"ITEM_LISTA1";

        //protected const string BLOCO_FORMATO_VALE = @"A1_VALE";

        //protected const string BLOCO_BLOCO_ERRO = @"ERRO";

        //protected const double LARGURA_LINHA = 5.0;
        //protected const double RECUO_ESTICAMENTO = 10.0;

        protected string _userName;
        protected Formato _formato;
        //protected ColetaErros _coletorDeErros;

        TSM.Model _model;
        TSD.DrawingHandler _dh = new TSD.DrawingHandler();
        TSD.Drawing _drawing;
        private ColecaoDxfs _colecaoDxfs;
        private ListViewDxfs _listViewDxfs;
        
        //private IColetorDeDadosDxf _coletorDeDadosDxf;
        //private IConversorDxf _conversorDxf;
        
        public MainWindow()
        {

           
            InitializeComponent();

            //_userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            //_model = new TSM.Model();
            //_dh = new TSD.DrawingHandler();

            //string xsplot = "";
            //TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            //var array = _model.GetInfo().ModelPath.Split('\\');
            //var descricaoProjeto = array[2];
            //string projeto = descricaoProjeto.Split('(')[1].Split(')')[0];

            ////_coletorDeDadosDxf = new ColetorDeDadosDxf();


            //_colecaoDxfs = new ColecaoDxfs(_model.GetInfo().ModelPath + xsplot, projeto);
            //_listViewDxfs = new ListViewDxfs(_colecaoDxfs);


            ////dgArquivos.ItemsSource = _listViewDxfs.CarregaCertos();
            ////dgArquivosErrados.ItemsSource = _listViewDxfs.CarregaErrados();
            ////SetaDados();
        }

        private void btnTeste_Click(object sender, RoutedEventArgs e)
        {
            TesteA.SelectSinglePartDrawingForSelectedPartInDocumentManagerOrDrawingList();
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            //var conversorDxf = new ConversorDxf();
            var certos = _listViewDxfs.CarregaArquivosItem().Where(x => x.Errado = false);
            foreach ( var certo in certos )
            {
                ArquivoDxf arquivoDxf = new ArquivoDxf(certo.Nome, _model.GetInfo().ModelPath);
                arquivoDxf.Converter(_userName);
                //conversorDxf.Converter(certo, _userName);
            }
            //conversorDxf.Converter(_colecaoDxfs, "TESTE");
        }

        //private void CarregaDetalhes(object sender, RoutedEventArgs e)
        //{
        //    var arquivoDxf = (ArquivoDxf)dgArquivos.SelectedItem;

        //    TabItem newTab = new TabItem();

        //    var nomeDesenho = arquivoDxf.Nome.Replace(".dxf", "");

        //    newTab.Header = nomeDesenho;

        //    newTab.Content = new TextBlock
        //    {
        //        Text = $"Conteúdo do desenho {nomeDesenho}.",
        //        Margin = new Thickness(10)
        //    };

        //    MainTabControl.Items.Add(newTab);

        //    //lblplot.Content = arquivoDxf.Nome;
        //}

        //private void AddTabsButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Lista de strings que serão os nomes das novas abas
        //    List<string> tabNames = new List<string>
        //    {
        //        "Segunda Aba",
        //        "Terceira Aba",
        //        "Aba Personalizada",
        //        "Última Aba"
        //    };

        //    // Adiciona cada nome da lista como uma nova TabItem
        //    foreach (string name in tabNames)
        //    {
        //        // Cria uma nova TabItem
        //        TabItem newTab = new TabItem();
        //        newTab.Header = name;

        //        // Adiciona conteúdo à TabItem (opcional)
        //        newTab.Content = new TextBlock
        //        {
        //            Text = $"Conteúdo da {name}",
        //            Margin = new Thickness(10)
        //        };

        //        // Adiciona a nova TabItem ao TabControl
        //        MainTabControl.Items.Add(newTab);
        //    }
        //}

        //private void SetaDados()
        //{





        //    //if (_dh.GetConnectionStatus())
        //    //{

        //    //    /*DrawingEnumerator SelectedDrawings = _dh.GetDrawingSelector().GetSelected();
        //    //    while (SelectedDrawings.MoveNext())
        //    //    {
        //    //        var teste = SelectedDrawings.Current;
        //    //        SelectedDrawings.Current.Delete();
        //    //    }*/

        //    //        _drawing = _dh.GetActiveDrawing();



        //    //        _fileName = _drawing.GetPlotFileName(true);

        //    //        string xsplot = "";
        //    //        TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

        //    //        var caminho = _model.GetInfo().ModelPath + xsplot + "\\" + _fileName + ".dxf";



        //    //            string versao = _drawing.ModificationDate.ToString("dd/MM/yyyy HH:mm:ss");

        //    //            // if (!File.Exists(caminho))
        //    //            //{
        //    //            lblplot.Content = $"FAVOR EXPORTAR O DESENHO";
        //    //            //}
        //    //            //else
        //    //            // {
        //    //            quadro.Title = $"FLUXO DO DESENHO: {_drawing.GetPlotFileName(true)}";

        //    //            lblplot.Content = $"ARQUIVO EXPORTADO: {_drawing.GetPlotFileName(true)}.dxf";
        //    //            lblmodificacao.Content = $"ULTIMA MODIFICACAO: {versao}";
        //    //            lblcara.Content = $"DESENHISTA: {_drawing.Title3}";



        //    //            //U:\MODELO TEKLA\VALE (PRJ00200) Transportadores Carajás\TR-1310KN-126 (2200) REV_4

        //    //            var array = _model.GetInfo().ModelPath.Split('\\');
        //    //            var drive = array[0];
        //    //            var endereco = array[1];
        //    //            var descricaoProjeto = array[2];
        //    //            var especificacao = array[3];
        //    //            _projeto = descricaoProjeto.Split('(')[1].Split(')')[0];

        //    //            coletarDados(caminho);

        //    //            var tituloProjeto = _camposFormato.ObterCampo("PROJETO");

        //    //            lblprojeto.Content = $"PROJETO: {tituloProjeto}";

        //    //            converter(caminho);

        //    //            // }
        //    //        }


        //}





        //private void coletarDados(string projeto, string filePath)
        //{

        //    var servico = new ServicoFormatacao();
        //    var formatacaoDTO = servico.ObterFormatacao(projeto);

        //    _formatacao = formatacaoDTO.Converter();

        //    DxfSingleton.Load(filePath);

        //    Ponto2d extMax = DxfSingleton.DxfDocument.Extmax();


        //    var formatoDATABuilder = new FormatoDATABuilder(extMax.X, Ponto2d.CriarSemEscala(0.0, 0.0), _formatacao);
        //    _formato = formatoDATABuilder.Build();

        //    _coletorDeErros = new ColetaErros();

        //    _coletaLista = new ColetaLmTNK(_formato);
        //    _coletaLista.Coletar();
        //    _colecaoConjuntos = new ColecaoConjuntos(_coletaLista, _coletorDeErros);
        //    _colecaoConjuntos.CriarConjuntosLM();

        //    _camposFormato = new CamposFormato(_formato, _coletorDeErros);
        //    _camposFormato.Coletar();

        //    _coletaRevisoes = new ColetaRevisoes(_formato, _coletorDeErros);
        //    _coletaRevisoes.Coletar();
        //    _coletaRevisoes.CriarRevisoes();





        //}





        //private void converter(string caminho, string filename, string usuario)
        //{
        //    _coletaLista.ApagarSelecao();
        //    var insercaoCabecalho =
        //        new InsercaoCabecalho(ARQUIVO_ORIGEM, BLOCO_CABECALHO, _formato, _coletorDeErros, _colecaoConjuntos);
        //    insercaoCabecalho.Inserir();

        //    var insercaoFormato = new InsercaoFormato(ARQUIVO_ORIGEM, BLOCO_FORMATO_VALE, _formato, _coletorDeErros);
        //    var insertFormato = insercaoFormato.Inserir();



        //    var atributosCampos = new AtributosCampos(_camposFormato);
        //    atributosCampos.Atributar(insertFormato);



        //    var atributosRevisoes = new AtributosRevisoes(_coletaRevisoes);
        //    atributosRevisoes.Atributar(insertFormato);

        //    _coletaRevisoes.ApagarSelecao();

        //    _camposFormato.ApagarSelecao();

        //    Encaminhamento encaminhamento = new Encaminhamento(caminho, usuario, filename);

        //    var caminhoSalvamento = encaminhamento.Encaminhar(@"C:\GitCAD");

        //    if (File.Exists(caminhoSalvamento))
        //    {
        //        File.Delete(caminhoSalvamento);
        //    }

        //    DxfSingleton.DxfDocument.Save(caminhoSalvamento);
        //}


    }
}
