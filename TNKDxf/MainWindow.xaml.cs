using System.Windows;
using TNKDxf.Dominio.Entidades;
using TNKDxf.TestesViabilidade;
using System;
using System.Linq;

namespace TNKDxf
{
   
    public partial class MainWindow : Window
    {
 
        protected Formato _formato;

        public MainWindow()
        {
            InitializeComponent();

            // Se o ícone estiver definido no XAML, não é necessário carregar o recurso vetorial
            // Mantemos o carregamento do vetor como fallback se desejar comentar a linha Icon no XAML
            try
            {
                if (this.Icon == null)
                {
                    var dict = new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/Assets/Icons/FlowDrawingIcon.xaml", UriKind.Absolute)
                    };
                    Application.Current.Resources.MergedDictionaries.Add(dict);

                    if (Application.Current.Resources.Contains("FlowDrawingIcon"))
                    {
                        var img = Application.Current.Resources["FlowDrawingIcon"] as System.Windows.Media.ImageSource;
                        if (img != null)
                        {
                            this.Icon = img;
                        }
                    }
                }
            }
            catch { }
           
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

     
    }
}
