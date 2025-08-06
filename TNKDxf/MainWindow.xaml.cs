using System.Windows;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dxfs;
using TNKDxf.Handles;
using TNKDxf.Infra;
using TNKDxf.TestesViabilidade;
using TNKDxf.ViewModel;



namespace TNKDxf
{
   
    public partial class MainWindow : Window
    {
 
        protected Formato _formato;

        public MainWindow()
        {

            //TODO: Tirar o mock quando for para produção
            //var teklaHandler = new TeklaHandler();
            //var extrator = new ExtratorDXFs();
            var teklaHandler = new MockTeklaHandler();
            var extrator = new MockExtratorDXFs();


            teklaHandler.Iniciar();
            //extrator.Extrair();
            var avaliador = new AvaliadorDesenhos(teklaHandler.ExportPath, teklaHandler.Projeto, teklaHandler.UserName);
            

            HandleCriacaoDxfs.CriarManipulapor(extrator, avaliador);

            InitializeComponent();
            Loaded += MainWindow_Loaded;

           
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           


            // Remove o handler para evitar chamadas múltiplas
            Loaded -= MainWindow_Loaded;

            // Cria a ViewModel (parâmetros seriam injetados via DI na prática)
            var viewModel = new MainViewModel();

            DataContext = viewModel;
        }

        private void btnTeste_Click(object sender, RoutedEventArgs e)
        {
            TesteA.SelectSinglePartDrawingForSelectedPartInDocumentManagerOrDrawingList();
        }

        private void btnEnviar_Click(object sender, RoutedEventArgs e)
        {
            //var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            ////var conversorDxf = new ConversorDxf();
            //var certos = _listViewDxfs.CarregaArquivosItem().Where(x => x.Errado = false);
            //foreach ( var certo in certos )
            //{
            //    ArquivoDxf arquivoDxf = new ArquivoDxf(certo.Nome, _model.GetInfo().ModelPath);
            //    arquivoDxf.Converter(userName);
            //    //conversorDxf.Converter(certo, _userName);
            //}
            ////conversorDxf.Converter(_colecaoDxfs, "TESTE");
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
