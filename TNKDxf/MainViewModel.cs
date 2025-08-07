using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TNKDxf.Dominio.Dwgs;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Handles;
using TNKDxf.ViewModel;


namespace TNKDxf
{
    public class MainViewModel : ViewModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _resultado = "Carregando...";
        private string _projeto;
        private string _userName;
        protected Formato _formato;
        private object _conteudoSelecionado;    
        public bool _processado = false;
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
        public ObservableCollection<TabViewModel> Tabs { get; } = new ObservableCollection<TabViewModel>();
        public TabViewModel SelectedTab { get; set; }
        public ICommand ToggleAbrirCommand { get; set; }
        public ICommand EnviarCorretosCommand { get; set; }

        //private Visibility _coluna0Visibilidade = Visibility.Collapsed;
        //public Visibility Coluna0Visibilidade
        //{
        //    get => _coluna0Visibilidade;
        //    set
        //    {
        //        _coluna0Visibilidade = value;
        //        OnPropertyChanged(nameof(Coluna0Visibilidade));
        //    }
        //}

        public MainViewModel()
        {
            //Coluna0Visibilidade = Visibility.Collapsed;



            var extraidos = HandleCriacaoDxfs.Instancia.ObterExtraidos();


            _colecaoDwgs = new ColecaoDwgs(extraidos, _projeto);
            _listViewDwgs = new ListViewDwgs(_colecaoDwgs);

            if (_listViewDwgs != null)
            {
                Arquivos = _listViewDwgs.CarregaArquivosItem();
            }

            ToggleAbrirCommand = new RelayCommand<ArquivoItem>(ToggleAbrirArquivo);
            EnviarCorretosCommand = new RelayCommand(EnviarArquivosCorretos);

            _ = InitializeAsync();
           /*Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await InitializeAsync();
            });*/
        }

      


        private async Task InitializeAsync()
        {
            //Coluna0Visibilidade = Visibility.Collapsed; // Oculta antes de iniciar
            
            await HandleCriacaoDxfs.Instancia.Manipular();

            /*if (HandleCriacaoDxfs.Instancia.ContadorProcessados == 0)
            {
                Coluna0Visibilidade = Visibility.Visible;
            }*/
            //  // Exibe após finalizar
        }

        private async void EnviarArquivosCorretos()
        {
            await HandleCriacaoDxfs.Instancia.Download(_arquivoSelecionado);
        }

        private void ToggleAbrirArquivo(ArquivoItem arquivo)
        {


            if (arquivo == null)
                return;

            _arquivoSelecionado = arquivo.Nome;

            var resultadoApi = HandleCriacaoDxfs.Instancia.ObterResult(arquivo.Nome);

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
