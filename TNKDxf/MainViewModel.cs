using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TNKDxf.Dominio.Dwgs;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Handles;
using TNKDxf.Infra;
using TNKDxf.ViewModel;


namespace TNKDxf
{
    public class MainViewModel : ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _resultado = "Carregando...";
        private string _projeto;
        protected Formato _formato;
        private object _conteudoSelecionado;
        public bool _processado = false;
        AvaliadorDesenhos _avaliadorDesenhos;
        public string Resultado
        {
            get => _resultado;
            set { _resultado = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Resultado))); }
        }

        public object ConteudoSelecionado
        {
            get => _conteudoSelecionado;
            set
            {
                _conteudoSelecionado = value;
                OnPropertyChanged(nameof(ConteudoSelecionado));
            }
        }

        private ColecaoDwgs _colecaoDwgs;
        private ListViewDwgs _listViewDwgs;
        private string _arquivoSelecionado;

        public ObservableCollection<ArquivoItem> Arquivos { get; set; } = new ObservableCollection<ArquivoItem>();

        public ICommand ToggleAbrirCommand { get; set; }
        public ICommand EnviarCorretosCommand { get; set; }

        public MainViewModel()
        {

           
            var teklaHandler = new TeklaHandler();

            var extrator = new ExtratorDXFs();


            if (!extrator.ForamExtraidos)
            {
                extrator.Extrair();
            }

            teklaHandler.Iniciar();

            _avaliadorDesenhos = new AvaliadorDesenhos(teklaHandler.ExportPath, teklaHandler.Projeto, teklaHandler.UserName);


            HandleCriacaoDxfs.CriarManipulapor(extrator, _avaliadorDesenhos);

            _colecaoDwgs = new ColecaoDwgs(extrator.Extraidos, _projeto);
            _listViewDwgs = new ListViewDwgs(_colecaoDwgs);

            if (_listViewDwgs != null)
            {
                Arquivos = _listViewDwgs.CarregaArquivosItem();
            }

            ToggleAbrirCommand = new RelayCommand<ArquivoItem>(ToggleAbrirArquivo);
            EnviarCorretosCommand = new RelayCommand(EnviarArquivosCorretos);

        }




        

        private async void EnviarArquivosCorretos()
        {
            var resultadoApi = _avaliadorDesenhos.ObterResult(_arquivoSelecionado);

            if (resultadoApi.Success)
            {
                await HandleCriacaoDxfs.Instancia.Download(_arquivoSelecionado);
            }
            else
            {
                MessageBox.Show($"Arquivo inválido para download: {resultadoApi.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        private async void ToggleAbrirArquivo(ArquivoItem arquivo)
        {
           int indice = _listViewDwgs.ObterIndice(arquivo);

     
            for (int i = 0; i < Arquivos.Count; i++)
            {
                Arquivos[i].Selecionado = false;
                if (i == indice)
                {
                    Arquivos[i].Selecionado = true;
                }
            }
              

            var resultadoApi = _avaliadorDesenhos.ObterResult(arquivo.Nome);
            if(resultadoApi == null)
            {
               resultadoApi = await _avaliadorDesenhos.Avaliar(arquivo.Nome);
                _avaliadorDesenhos.IncluirResultado(resultadoApi);
            }
           
            if (arquivo == null)
                return;

            _arquivoSelecionado = arquivo.Nome;

            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(resultadoApi.Resultado))
            {
                sb.AppendLine("📦 Arquivo:");
                sb.AppendLine(resultadoApi.Resultado);
            }


            sb.AppendLine($"\n🟢 Sucesso: {resultadoApi.Success}");
            sb.AppendLine($"📄 Mensagem: {resultadoApi.Message}");



            if (resultadoApi.Notifications != null && resultadoApi.Notifications.Count > 0)
            {
                sb.AppendLine("\n⚠️ Notificações:");
                foreach (var n in resultadoApi.Notifications)
                {
                    sb.AppendLine($" - [{n.Key}] {n.Message}");
                }
            }

            ConteudoSelecionado = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = new TextBlock
                {
                    Text = sb.ToString(),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10)
                }
            };
        }



    }
}
