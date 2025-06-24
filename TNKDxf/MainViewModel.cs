using Dynamic.Tekla.Structures;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TNKDxf.Dominio.Dwgs;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Infra;
using TNKDxf.ViewModel;
using TSD = Dynamic.Tekla.Structures.Drawing;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf
{
    public class MainViewModel : ViewModelBase
    {
        protected string _userName;
        protected Formato _formato;
        //protected ColetaErros _coletorDeErros;

        TSM.Model _model;
        TSD.DrawingHandler _dh;
        //private ColecaoDxfs _colecaoDxfs;
        //private ListViewDxfs _listViewDxfs;
        private ColecaoDwgs _colecaoDwgs;
        private ListViewDwgs _listViewDwgs;

        IServicoEnvioDesenhos _servicoEnvioDesenhos; //= new ServicoEnvioDesenhos(new CfgEngAPI());

        public ObservableCollection<ArquivoItem> Arquivos { get; } = new ObservableCollection<ArquivoItem>();
        public ObservableCollection<TabItem> Tabs { get; } = new ObservableCollection<TabItem>();

        public ICommand ToggleAbrirCommand { get; }
        public ICommand EnviarCorretosCommand { get; }

        public MainViewModel()
        {

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);


            //NcParaDxf.Run();

            _servicoEnvioDesenhos = new ServicoEnvioDesenhos(new CfgEngAPI());

            //exportar desenhos tekla structures
            /*
            _dh = new TSD.DrawingHandler();


            var dg = _dh.GetDrawingSelector().GetSelected();

            while (dg.MoveNext())
            {
                var drawing = dg.Current;

                if (drawing == null)
                    break;

                var tipo = drawing.GetType();

                if (tipo == typeof(TSD.SinglePartDrawing))
                {
                    var spd = drawing as TSD.SinglePartDrawing;

                    var marca = spd.Mark.Replace("[", "").Replace("]", "");
                    spd.SetUserProperty("TCNM_N_KOCH", marca);
                    
                }

                if (tipo == typeof(TSD.AssemblyDrawing))
                {
                    var assd = drawing as TSD.AssemblyDrawing;

                    var marca = assd.Mark.Replace("[", "").Replace("]", "");
                    assd.SetUserProperty("TCNM_N_KOCH", marca);

                }

                if (tipo == typeof(TSD.GADrawing))
                {
                    var gad = drawing as TSD.GADrawing;

                    var marca = gad.Title1;
                    //gad.SetUserProperty("TCNM_N_KOCH", marca);

                }

                if (tipo == typeof(TSD.MultiDrawing))
                {
                    var multid = drawing as TSD.MultiDrawing;

                    var marca = multid.Title1;
                    multid.SetUserProperty("TCNM_N_KOCH", marca);

                }

            }

            
            ExportadoraDxf.Run(xsplot);*/

            //var dg = _dh.GetActiveDrawing();



            //if (tipo == typeof(TSD.SinglePartDrawing))
            //{

            //var views = spd.GetSheet().GetViews();
            //while(views.MoveNext())
            //{
            //    var view = views.Current;
            //   var objetos = view.GetRelatedObjects();
            //    while (objetos.MoveNext())
            //    {
            //        var objeto = objetos.Current;
            //        if (objeto is TSM.ModelObject)
            //        {
            //           var tipoObjeto = objeto.GetType();
            //            if(tipoObjeto == typeof(TSM.Part))
            //            {
            //                var part = objeto.Attributes;
            //                // Aqui você pode fazer algo com o objeto, como coletar informações
            //                // Exemplo: Console.WriteLine(modelObject.Identifier.ID);
            //            }

            //            // Aqui você pode fazer algo com o objeto, como coletar informações
            //            // Exemplo: Console.WriteLine(modelObject.Identifier.ID);
            //        }
            //    }
            //}
            //}

            //dg.SetUserProperty("TCNM_N_KOCH", "RRP_00");





            ToggleAbrirCommand = new RelayCommand<ArquivoItem>(ToggleAbrirArquivo);
            EnviarCorretosCommand = new RelayCommand(EnviarArquivosCorretos);


            _userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            _model = new TSM.Model();
            //_dh = new TSD.DrawingHandler();

           

            string proj = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_PROJECT", ref proj);


            

            //dg.SetUserProperty("DXFExport", "true");


            //while (SelectedDrawings.MoveNext())
            //{
            //    var dg = SelectedDrawings.Current;
            //}

            //var array = _model.GetInfo().ModelPath.Split('\\');
            //var descricaoProjeto = array[2];


            //string projeto = proj.Split('\\').Last(); //descricaoProjeto.Split('(')[1].Split(')')[0];
            string projeto = _model.GetProjectInfo().ProjectNumber; // Obter o número do projeto diretamente

            //_colecaoDxfs = new ColecaoDxfs(_model.GetInfo().ModelPath + xsplot, projeto);
            //_listViewDxfs = new ListViewDxfs(_colecaoDxfs);

            _colecaoDwgs = new ColecaoDwgs(_model.GetInfo().ModelPath + xsplot, projeto);
            _listViewDwgs = new ListViewDwgs(_colecaoDwgs);

            //Arquivos =  _listViewDxfs.CarregaArquivosItem();
            Arquivos = _listViewDwgs.CarregaArquivosItem();

            // Dados de exemplo
            //Arquivos.Add(new ArquivoItem { Nome = "Arquivo1.txt", Errado = false });
            //Arquivos.Add(new ArquivoItem { Nome = "Arquivo2.txt", Errado = true });
            //Arquivos.Add(new ArquivoItem { Nome = "Arquivo3.txt", Errado = false });
            //Arquivos.Add(new ArquivoItem { Nome = "Arquivo4.txt", Errado = true });



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
                    //ArquivoDxf arquivoDxf = _colecaoDxfs.ObterArquivoDxf(certo.Nome);//new ArquivoDxf(certo.Nome, _model.GetInfo().ModelPath);
                    ArquivoDwg arquivoDwg = _colecaoDwgs.ObterArquivoDwg(certo.Nome);//new ArquivoDxf(certo.Nome, _model.GetInfo().ModelPath);
                    arquivoDwg.Converter(_userName);
                }
            }
        }

        private void ToggleAbrirArquivo(ArquivoItem arquivo)
        {
            if (arquivo.Aberto)
            {
                // Fechar - remover a TabItem
                var tabToRemove = Tabs.FirstOrDefault(t => t.Header.ToString() == arquivo.Nome);
                if (tabToRemove != null)
                {
                    Tabs.Remove(tabToRemove);
                }
            }
            else
            {
                // Abrir - criar nova TabItem
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
