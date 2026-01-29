using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
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
        private string _resultado = "Carregando...";
        private string _projeto;
        protected Formato _formato;
        private object _conteudoSelecionado;
        public bool _processado = false;
        AvaliadorDesenhos _avaliadorDesenhos;
        public string Resultado
        {
            get => _resultado;
            set { _resultado = value; OnPropertyChanged(nameof(Resultado)); }
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
        private ListViewDxf _listViewDwgs;
        private string _arquivoSelecionado;

        public ObservableCollection<ArquivoItem> Arquivos { get; set; } = new ObservableCollection<ArquivoItem>();

        public ICommand ToggleAbrirCommand { get; set; }
        public ICommand EnviarCorretosCommand { get; set; }
        public ICommand DownloadArquivoCommand { get; set; }
        public ICommand ExtrairCommand { get; set; }

        // Propriedades de progresso
        private bool _isExtraindo;
        public bool IsExtraindo
        {
            get => _isExtraindo;
            set { _isExtraindo = value; OnPropertyChanged(nameof(IsExtraindo)); OnPropertyChanged(nameof(PodeExtrair)); OnPropertyChanged(nameof(ProgressoIndeterminado)); }
        }

        private int _progressoAtual;
        public int ProgressoAtual
        {
            get => _progressoAtual;
            set { _progressoAtual = value; OnPropertyChanged(nameof(ProgressoAtual)); }
        }

        private int _progressoMaximo;
        public int ProgressoMaximo
        {
            get => _progressoMaximo;
            set { _progressoMaximo = value; OnPropertyChanged(nameof(ProgressoMaximo)); OnPropertyChanged(nameof(ProgressoIndeterminado)); }
        }

        private string _statusProgresso = "Pronto";
        public string StatusProgresso
        {
            get => _statusProgresso;
            set { _statusProgresso = value; OnPropertyChanged(nameof(StatusProgresso)); }
        }

        public bool PodeExtrair => !IsExtraindo;
        public bool ProgressoIndeterminado => IsExtraindo && ProgressoMaximo <= 0;

        public MainViewModel()
        {
            var teklaHandler = new TeklaHandler();

            // Não extrai na inicialização; apenas prepara ambiente
            teklaHandler.Iniciar();

            _avaliadorDesenhos = new AvaliadorDesenhos(teklaHandler.ExportPath, teklaHandler.Projeto, teklaHandler.UserName);

            HandleCriacaoDxfs.CriarManipulapor(_avaliadorDesenhos);

            // Inicializa coleção (vazia inicialmente)
            _colecaoDwgs = new ColecaoDwgs(ExtratorDXFs.GetInstance().Extraidos, _projeto);
            _listViewDwgs = new ListViewDxf(_colecaoDwgs);

            var iniciais = _listViewDwgs.CarregaArquivosItem();
            Arquivos.Clear();
            foreach (var item in iniciais)
                Arquivos.Add(item);

            ToggleAbrirCommand = new RelayCommand<ArquivoItem>(ToggleAbrirArquivo);
            EnviarCorretosCommand = new RelayCommand(EnviarArquivosCorretos);
            DownloadArquivoCommand = new RelayCommand<ArquivoItem>(DownloadArquivo);
            ExtrairCommand = new RelayCommand(async () => await ExtrairArquivosAsync());
        }

        private async Task ExtrairArquivosAsync()
        {
            IsExtraindo = true;
            ProgressoAtual = 0;
            ProgressoMaximo = 0;
            StatusProgresso = "Preparando extração...";

            var extrator = ExtratorDXFs.GetInstance();

            // Dispara a extração em background
            var tarefaExtracao = Task.Run(() => extrator.Extrair());

            // Monitora progresso por contagem de arquivos na pasta de saída
            while (!tarefaExtracao.IsCompleted)
            {
                if (ProgressoMaximo == 0 && extrator.TotalEsperado > 0)
                {
                    // Adiciona 1 para considerar a etapa de preencher a tabela
                    ProgressoMaximo = extrator.TotalEsperado + 1;
                }

                if (!string.IsNullOrWhiteSpace(extrator.PastaSaida) && Directory.Exists(extrator.PastaSaida))
                {
                    try
                    {
                        var gerados = Directory.GetFiles(extrator.PastaSaida, "*.dxf").Length;
                        ProgressoAtual = gerados;
                        StatusProgresso = ProgressoMaximo > 0
                            ? $"Preparando remessa: {ProgressoAtual}/{ProgressoMaximo - 1}"
                            : $"Preparando remessa: {ProgressoAtual}...";
                    }
                    catch { }
                }
                else
                {
                    StatusProgresso = "Iniciando...";
                }

                await Task.Delay(500);
            }

            await tarefaExtracao;

            // Finaliza progresso da extração
            if (!string.IsNullOrWhiteSpace(extrator.PastaSaida) && Directory.Exists(extrator.PastaSaida))
            {
                var gerados = Directory.GetFiles(extrator.PastaSaida, "*.dxf").Length;
                ProgressoAtual = gerados;
            }

            // Garante que o máximo considere a etapa final de atualização da tabela
            if (ProgressoMaximo == 0 || ProgressoMaximo <= ProgressoAtual)
            {
                ProgressoMaximo = ProgressoAtual + 1;
            }

            StatusProgresso = "Atualizando lista de arquivos...";

            // Atualiza a tabela em background para não travar a UI
            var atualizados = await Task.Run(() =>
            {
                _colecaoDwgs = new ColecaoDwgs(extrator.Extraidos, _projeto);
                _listViewDwgs = new ListViewDxf(_colecaoDwgs);
                return _listViewDwgs.CarregaArquivosItem();
            });

            Arquivos.Clear();
            foreach (var item in atualizados)
            {
                var res = _avaliadorDesenhos.ObterResult(item.Nome);
                if (res != null && res.Success)
                {
                    item.PodeBaixar = true;
                }
                Arquivos.Add(item);
            }

            // Conclui o progresso
            ProgressoAtual = ProgressoMaximo;
            StatusProgresso = $"Geração concluída: {Arquivos.Count} arquivo(s).";

            // Toast de conclusão
            try
            {
                System.Windows.Forms.NotifyIcon notify = new System.Windows.Forms.NotifyIcon();
                notify.Visible = true;
                notify.Icon = System.Drawing.SystemIcons.Information;
                notify.BalloonTipTitle = "DXFs Gerados";
                notify.BalloonTipText = $"{Arquivos.Count} arquivo(s) gerado(s) com sucesso.";
                notify.ShowBalloonTip(3000);

                // Oculta depois de alguns segundos
                await Task.Delay(4000);
                notify.Dispose();
            }
            catch { }

            IsExtraindo = false;
        }

        private async void EnviarArquivosCorretos()
        {
            var resultadoApi = _avaliadorDesenhos.ObterResult(_arquivoSelecionado);

            if (resultadoApi.Success)
            {
                await HandleCriacaoDxfs.Instancia.Download(resultadoApi.Resultado);
            }
            else
            {
                MessageBox.Show($"Arquivo inválido para download: {resultadoApi.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DownloadArquivo(ArquivoItem arquivo)
        {
            if (arquivo == null)
                return;

            var resultadoApi = _avaliadorDesenhos.ObterResult(arquivo.Nome);

            if (resultadoApi == null)
            {
                resultadoApi = await _avaliadorDesenhos.Avaliar(arquivo.Nome);
                _avaliadorDesenhos.IncluirResultado(resultadoApi);
            }

            if (resultadoApi.Success)
            {
                var path = await HandleCriacaoDxfs.Instancia.Download(resultadoApi.Resultado);
                if (!string.IsNullOrEmpty(path))
                {
                    arquivo.PodeBaixar = false;
                    MessageBox.Show($"Download concluído com sucesso!\nArquivo salvo em: {path}", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erro ao baixar o arquivo. Arquivo não encontrado no servidor.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show($"Arquivo inválido para download: {resultadoApi.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ToggleAbrirArquivo(ArquivoItem arquivo)
        {
            if (arquivo == null)
                return;

            // desabilita o botão Converter desta linha imediatamente para evitar cliques repetidos
            arquivo.PodeConverter = false;

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
            if (resultadoApi == null)
            {
                resultadoApi = await _avaliadorDesenhos.Avaliar(arquivo.Nome);
                _avaliadorDesenhos.IncluirResultado(resultadoApi);
            }

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

            if (resultadoApi.Success)
            {
                arquivo.PodeBaixar = true;
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
