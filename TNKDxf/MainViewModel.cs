using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private List<CommandResult> _resultados;

        public string Resultado
        {
            get => _resultado;
            set { _resultado = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Resultado))); }
        }



        private ColecaoDwgs _colecaoDwgs;
        private ListViewDwgs _listViewDwgs;


        public ObservableCollection<ArquivoItem> Arquivos { get; set; } = new ObservableCollection<ArquivoItem>();
        public ObservableCollection<AbaModel> Tabs { get; } = new ObservableCollection<AbaModel>();

        public ICommand ToggleAbrirCommand { get; set; }
        public ICommand EnviarCorretosCommand { get; set; }



        public MainViewModel()
        {
            
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

        }


        private async Task InitializeAsync()
        {
            await HandleCriacaoDxfs.Instancia.Manipular();
        }

        private void EnviarArquivosCorretos()
        {
            var arquivosCorretos = Arquivos.Where(a => !a.Errado).ToList();

            if (!arquivosCorretos.Any())
            {
                MessageBox.Show("Nenhum arquivo correto para enviar.");
                return;
            }

            var result = MessageBox.Show($"Deseja enviar {arquivosCorretos.Count} arquivo(s)?",
                                       "Confirmar envio",
                                       MessageBoxButton.YesNo);


            if (result == MessageBoxResult.Yes)
            {

                foreach (var certo in arquivosCorretos)
                {
                    ArquivoDwg arquivoDwg = _colecaoDwgs.ObterArquivoDwg(certo.Nome);
                    arquivoDwg.Enviar(_userName);
                }
            }
        }

        private void ToggleAbrirArquivo(ArquivoItem arquivo)
        {


            var texto = new StringBuilder();

            CommandResult resultadoApi = HandleCriacaoDxfs.Instancia.ObterResult(arquivo.Nome);

            texto.AppendLine($"🟢 Sucesso: {resultadoApi.Success}");
            texto.AppendLine($"📄 Mensagem: {resultadoApi.Message}");

            if (!string.IsNullOrWhiteSpace(resultadoApi.Resultado))
            {
                texto.AppendLine("\n📦 Resultado:");
                texto.AppendLine(resultadoApi.Resultado);
            }

            if (resultadoApi.Notifications != null && resultadoApi.Notifications.Count > 0)
            {
                texto.AppendLine("\n⚠️ Notificações:");
                foreach (var n in resultadoApi.Notifications)
                {
                    texto.AppendLine($" - [{n.Key}] {n.Message}");
                }
            }

            var textBlock = new TextBlock
            {
                Text = texto.ToString(),
                Margin = new Thickness(10),
                TextWrapping = TextWrapping.Wrap
            };

            var scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = textBlock
            };

            var tab = new AbaModel
            {
                Header = arquivo.Nome,
                Content = scrollViewer
            };

            Tabs.Add(tab);

            arquivo.Aberto = !arquivo.Aberto;
        }

    }
}
