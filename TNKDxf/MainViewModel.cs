using Dynamic.Tekla.Structures;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TNKDxf.Dominio.Dwgs;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Handles;
using TNKDxf.Infra;
using TNKDxf.Infra.Dtos;
using TNKDxf.ViewModel;
using TSD = Dynamic.Tekla.Structures.Drawing;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf
{
    public class MainViewModel : ViewModelBase
    {
        protected string _userName;
        protected Formato _formato;

        TSM.Model _model;
        TSD.DrawingHandler _dh;

        private ColecaoDwgs _colecaoDwgs;
        private ListViewDwgs _listViewDwgs;

        IServicoEnvioDesenhos _servicoEnvioDesenhos; 

        public ObservableCollection<ArquivoItem> Arquivos { get; } = new ObservableCollection<ArquivoItem>();
        public ObservableCollection<TabItem> Tabs { get; } = new ObservableCollection<TabItem>();

        public ICommand ToggleAbrirCommand { get; }
        public ICommand EnviarCorretosCommand { get; }

        public MainViewModel()
        {

            _servicoEnvioDesenhos = new ServicoEnvioDesenhos(new CfgEngAPI());

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            _model = new TSM.Model();

            string projeto = _model.GetProjectInfo().ProjectNumber;

            var exportPath = System.IO.Path.Combine(_model.GetInfo().ModelPath, xsplot.Substring(2, xsplot.Length - 2));

            _userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];

         
            HandleCriacaoDxfs handleCriacaoDxfs = new HandleCriacaoDxfs();
            
            var desenhos = handleCriacaoDxfs.CriarDxfs(); 
            

            //gambiarra para pegar os desenhos
            //var desenhos = new List<string>
            //{
            //    "PRJ00011-D-00652"
            //};

            DirectoryInfo diretorioRecebidos = new DirectoryInfo(exportPath);
            FileInfo[] arquivosProcessar = diretorioRecebidos.GetFiles("*.dxf");
            foreach (var desenho in desenhos)
            {
                var arquivo = arquivosProcessar.FirstOrDefault(a => a.Name.Contains(desenho));
                _servicoEnvioDesenhos.UploadAsync(arquivo.FullName, "Tekla Structures", _userName, projeto);

            }

            ToggleAbrirCommand = new RelayCommand<ArquivoItem>(ToggleAbrirArquivo);
            EnviarCorretosCommand = new RelayCommand(EnviarArquivosCorretos);

            List<ArquivoDTO> arquivosProcessados = _servicoEnvioDesenhos.ListaProcessadosAsync(_userName, projeto);
            _colecaoDwgs = new ColecaoDwgs(arquivosProcessados, projeto);
            _listViewDwgs = new ListViewDwgs(_colecaoDwgs);


            Arquivos = _listViewDwgs.CarregaArquivosItem();

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
            if (arquivo.Aberto)
            {
                var tabToRemove = Tabs.FirstOrDefault(t => t.Header.ToString() == arquivo.Nome);
                if (tabToRemove != null)
                {
                    Tabs.Remove(tabToRemove);
                }
            }
            else
            {
                var newTab = new TabItem
                {
                    Header = arquivo.Nome,
                    Content = new TextBlock
                    {
                        Text = $"Conteúdo do arquivo {arquivo.Nome}",
                        Margin = new Thickness(10)
                    }
                };
                Tabs.Add(newTab);
            }

            arquivo.Aberto = !arquivo.Aberto;
        }
    }
}
