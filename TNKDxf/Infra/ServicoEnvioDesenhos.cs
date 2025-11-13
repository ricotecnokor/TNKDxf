using Dynamic.Tekla.Structures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using TNKDxf.Handles;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf.Infra
{
    public class ServicoEnvioDesenhos : IServicoEnvioDesenhos
    {
        private readonly string _uri;

        public ServicoEnvioDesenhos(CfgEngAPI tecnokorAPI)
        {
            _uri = $"{tecnokorAPI.URI}/api";
        }

        public async Task<CommandResult> UploadAsync(string file, string softwareOrigem, string usuario, string padrao)
        {
            var url = $"{_uri}/Dxf";
            CommandResult respostas = new CommandResult();
            try
            {
                var client = new HttpClient();

                using (var formContent = new MultipartFormDataContent())
                {
                    using (var fileStream = File.OpenRead(file))
                    {
                        formContent.Add(new StreamContent(fileStream), "File", file);
                        formContent.Add(new StringContent(softwareOrigem), "SoftwareOrigem");
                        formContent.Add(new StringContent(usuario), "Usuario");
                        formContent.Add(new StringContent(padrao), "Padrao");

                        // Fix: Remove assignment to a variable since PostAsync().Wait() returns void
                      var response = await client.PostAsync(url, formContent);
                      if(response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            respostas = JsonConvert.DeserializeObject<CommandResult>(content); // Fix: Use JsonConvert.DeserializeObject
                        }
                        else
                        {
                            respostas = new CommandResult
                            {
                                Success = false,
                                Message = $"Erro ao processar o arquivo: {response.StatusCode}",
                                Notifications = new List<Notification>()
                            };
                        }
                        return respostas;
                    }
                }
            }
            catch (Exception ex)
            {
                respostas = new CommandResult
                {
                    Success = false,
                    Message = $"Não processado",
                    Notifications = new List<Notification>()
                };
            }

            return respostas;
        }



        public List<string> ListaProcessadosAsync(string usuario, string padrao)
        {
            var url = $"{_uri}/GetListaProcessados?Usuario={usuario}";
            try
            {
                var http = new HttpClient();

                var response = http.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var listaProcessados = JsonConvert.DeserializeObject<List<string>>(content); // Fix: Use JsonConvert.DeserializeObject
                    return listaProcessados;
                }
                else
                {
                    Console.WriteLine($"Erro ao obter a lista de processados: {response.StatusCode}");
                }
            }
            catch (Exception)
            {
                throw;
            }

            return new List<string>();
        }

        public async Task DownloadFile(string usuario, string padrao, string aplicativo, string fileName)
        {
            TSM.Model model = new TSM.Model();
            string modelPath = model.GetInfo().ModelPath;

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            var dir = $"{modelPath}{xsplot.Replace(".", "")}";

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];

            

            var lista = Directory.EnumerateFiles(dir).ToList();

            var nomeCompletoDoArquivo = lista.FirstOrDefault(x => x.Contains(fileName)).Split('\\').Last().Replace(".dxf","");

           

            var fileURL = $"{_uri}/GetDownloadDxf?Usuario={usuario}&Arquivo={nomeCompletoDoArquivo}";

            
           
            HttpClient httpClient = new HttpClient();

            var response = await httpClient.GetAsync(fileURL);

            
            var diretorioSalvamento = dir + @"\Download";

            
            if (!Directory.Exists(diretorioSalvamento))
            {
                Directory.CreateDirectory(diretorioSalvamento);
            }



            var filePath = Path.Combine(dir, $"{nomeCompletoDoArquivo}.dxf");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var arquivoSalvamento = Path.Combine(diretorioSalvamento, $"{nomeCompletoDoArquivo}.dxf");
            if (File.Exists(arquivoSalvamento))
            {
                File.Delete(arquivoSalvamento);
            }



            using (var fs = new FileStream(arquivoSalvamento, FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
                MessageBox.Show($"Arquivo {fileName} salvo em {diretorioSalvamento}", "Download Concluído", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }



   
}
