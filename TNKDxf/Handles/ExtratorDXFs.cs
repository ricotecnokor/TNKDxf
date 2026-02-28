using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Model.Operations;
using TNKDxf.TeklaManipulacao;
using TNKDxf.TeklaManipulacao.Adapters;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace TNKDxf.Handles
{
    public class ExtratorDXFs
    {
        List<string> _desenhos;
        bool _foramExtraidos = false;
        string _pastaSaida;
        IAdapterDesenho _adapterDesenho;

        private static ExtratorDXFs _instance;
        

        private ExtratorDXFs()
        {
            _adapterDesenho = new AdapterDesenho();
            _desenhos = new List<string>();
            TSM.Model model = new TSM.Model();
            string modelPath = model.GetInfo().ModelPath;

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            _pastaSaida = modelPath + xsplot.Replace(".", "");
        }

        public static ExtratorDXFs GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ExtratorDXFs();
            }
            return _instance;
        }

        public IEnumerable<object> Desenhos { get; internal set; }
        public List<string> Extraidos => _desenhos;

        // Propriedades para UI de progresso
        public int TotalEsperado { get; private set; } = 0;
        public string PastaSaida => _pastaSaida;
        public bool EmExecucao { get; private set; } = false;

        // Limpa a pasta de saída para não contar DXFs antigos
        //public void LimparPastaSaida()
        //{
        //    //TSM.Model model = new TSM.Model();
        //    //string modelPath = model.GetInfo().ModelPath;

        //    //string xsplot = "";
        //    //TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

        //    //var destino = modelPath + xsplot.Replace(".", "");
        //    //PastaSaida = destino;

        //    TentarLimparDxfs(_pastaSaida);
        //}

        // Rotina centralizada para limpar DXFs e avisar se falhar
        private void TentarLimparDxfs()
        {
            try
            {
                if (!Directory.Exists(_pastaSaida))
                {
                    Directory.CreateDirectory(_pastaSaida);
                    return;
                }

                Exception firstEx = null;
                foreach (var file in Directory.GetFiles(_pastaSaida, "*.dxf"))
                {
                    try
                    {
                        var fi = new FileInfo(file);
                        if (fi.IsReadOnly) fi.IsReadOnly = false;
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        if (firstEx == null) firstEx = ex;
                    }
                }

                // Verificação pós-limpeza
                var restantes = Directory.GetFiles(_pastaSaida, "*.dxf").Length;
                if (restantes > 0)
                {
                    System.Windows.MessageBox.Show(
                        $"Não foi possível limpar todos os DXFs em \"{_pastaSaida}\".\n" +
                        $"Arquivos restantes: {restantes}. Verifique permissões ou se estão em uso.\n" +
                        $"Detalhes: {firstEx?.Message}",
                        "Aviso: limpeza incompleta",
                        System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Warning
                    );
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Falha ao limpar a pasta de DXFs \"{_pastaSaida}\".\nDetalhes: {ex.Message}",
                    "Erro ao limpar DXFs",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error
                );
            }
        }

        public void ColetarArquivos()
        {
            var arquivosExistentes = new List<string>();

            if (Directory.Exists(_pastaSaida))
            {
                arquivosExistentes = Directory.GetFiles(_pastaSaida, "*.dxf").ToList();
            }

            if(arquivosExistentes.Count < 1)
            {
                return;
            }

            LeitorRlatorioDesenhosTekla leitor = new LeitorRlatorioDesenhosTekla("multiTemp.rpt");
            var relatorio = leitor.Ler();




            //var appFolder = TeklaStructuresInfo.GetLocalAppDataFolder();
            //var versao = appFolder.Split('\\').Last();

            TSD.DrawingHandler dh = new TSD.DrawingHandler();

           

            var dg = dh.GetDrawingSelector().GetSelected();

            

            _desenhos.Clear();
            while (dg.MoveNext())
            {
                var drawing = dg.Current;
                if (drawing == null) break;

                var tipo = drawing.GetType();

                if (tipo == typeof(TSD.SinglePartDrawing))
                {
                    var spd = drawing as TSD.SinglePartDrawing;
                    var marca = spd.Mark.Replace("[", "").Replace("]", "");
                    if (arquivosExistentes.Any(a => a.Split('\\').Last().StartsWith(marca)))
                    {
                        spd.SetUserProperty("TCNM_N_KOCH", marca);
                        _desenhos.Add(marca);
                    }
                        
                }
                else if (tipo == typeof(TSD.AssemblyDrawing))
                {
                    var assd = drawing as TSD.AssemblyDrawing;
                    var marca = assd.Mark.Replace("[", "").Replace("]", "");
                    if (arquivosExistentes.Any(a => a.Split('\\').Last().StartsWith(marca)))
                    {
                        assd.SetUserProperty("TCNM_N_KOCH", marca);
                        _desenhos.Add(marca);
                    }
                        
                }
                else if (tipo == typeof(TSD.GADrawing))
                {
                    var gad = drawing as TSD.GADrawing;

                    var marca = gad.Title1;
                    if (arquivosExistentes.Any(a => a.Split('\\').Last().StartsWith(marca)))
                    {
                        _desenhos.Add(marca);
                    }
                }
                else if (tipo == typeof(TSD.MultiDrawing))
                {
                    var multid = drawing as TSD.MultiDrawing;
                    var marca = multid.Title1;

                    if (arquivosExistentes.Any(a => a.Split('\\').Last().StartsWith(marca)))
                    {
                        multid.SetUserProperty("TCNM_N_KOCH", marca);

                        _adapterDesenho.InserirInformacoesDesenho(arquivosExistentes.First(a => a.Split('\\').Last().StartsWith(marca)), relatorio);

                        _desenhos.Add(marca);


                    }
                }
            }

            _foramExtraidos = true;
        }

        public void Extrair()
        {
            if (_foramExtraidos)
                return;

            EmExecucao = true;
            try
            {
                var appFolder = TeklaStructuresInfo.GetLocalAppDataFolder();
                var versao = appFolder.Split('\\').Last();

                TSD.DrawingHandler dh = new TSD.DrawingHandler();

                TSM.Model model = new TSM.Model();
                string modelPath = model.GetInfo().ModelPath;

                var dg = dh.GetDrawingSelector().GetSelected();

                //string xsplot = "";
                //TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

                //var _xsplot = modelPath + xsplot.Replace(".", "");
                //PastaSaida = _xsplot;

                // Limpa apenas DXFs antes de exportar e verifica
                TentarLimparDxfs();

                _desenhos.Clear();
                while (dg.MoveNext())
                {
                    var drawing = dg.Current;
                    if (drawing == null) break;

                    var tipo = drawing.GetType();

                    if (tipo == typeof(TSD.SinglePartDrawing))
                    {
                        var spd = drawing as TSD.SinglePartDrawing;
                        var marca = spd.Mark.Replace("[", "").Replace("]", "");
                        spd.SetUserProperty("TCNM_N_KOCH", marca);
                        _desenhos.Add(marca);
                    }
                    else if (tipo == typeof(TSD.AssemblyDrawing))
                    {
                        var assd = drawing as TSD.AssemblyDrawing;
                        var marca = assd.Mark.Replace("[", "").Replace("]", "");
                        assd.SetUserProperty("TCNM_N_KOCH", marca);
                        _desenhos.Add(marca);
                    }
                    else if (tipo == typeof(TSD.GADrawing))
                    {
                        var gad = drawing as TSD.GADrawing;
                        var marca = gad.Title1;
                        _desenhos.Add(marca);
                    }
                    else if (tipo == typeof(TSD.MultiDrawing))
                    {
                        var multid = drawing as TSD.MultiDrawing;
                        var marca = multid.Title1;
                        multid.SetUserProperty("TCNM_N_KOCH", marca);
                        _desenhos.Add(marca);
                    }
                }

                TotalEsperado = _desenhos.Count;


                if (versao == "2024.0")
                {
                    ExportadoraDxf.Run();
                    _foramExtraidos = true;
                }

                _foramExtraidos = true;
            }
            finally
            {
                EmExecucao = false;
            }
        }


    }
}
