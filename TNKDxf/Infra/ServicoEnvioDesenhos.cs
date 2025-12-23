using Dynamic.Tekla.Structures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows;
using TNKDxf.Handles;
using static System.Net.WebRequestMethods;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf.Infra
{
    public class ServicoEnvioDesenhos : IServicoEnvioDesenhos
    {
        private readonly string _uri;
        HttpClient _http;
        public ServicoEnvioDesenhos(CfgEngAPI tecnokorAPI)
        {
            _uri = $"{tecnokorAPI.URI}/api";
            _http = new HttpClient();
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
                    using (var fileStream = System.IO.File.OpenRead(file))
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

        public async Task<string> DownloadFile(string usuario, string padrao, string aplicativo, string fileName)
        {
            

            TSM.Model model = new TSM.Model();
            string modelPath = model.GetInfo().ModelPath;

            string xsplot = "";
            TeklaStructuresSettings.GetAdvancedOption("XS_DRAWING_PLOT_FILE_DIRECTORY", ref xsplot);

            var dir = $"{modelPath}{xsplot.Replace(".", "")}";

            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];

            

            var lista = Directory.EnumerateFiles(dir).Where(x => x.EndsWith("dxf")).ToList();

            var nomeCompletoDoArquivo = lista.FirstOrDefault(x => x.Contains(fileName)).Split('\\').Last().Replace(".dxf","");
            var revisao = nomeCompletoDoArquivo.Split(' ').Last().Replace("rev","");

            /////////////////////////
            ///
            var baseApi = _uri ?? string.Empty;
            if (baseApi.EndsWith("/api/Dxf", StringComparison.OrdinalIgnoreCase))
            {
                baseApi = baseApi.Replace("/api/Dxf", "/api");
            }

            var fileURL = $"{baseApi.TrimEnd('/')}/GetDownloadDxf?Usuario={Uri.EscapeDataString(usuario)}&Arquivo={Uri.EscapeDataString(fileName)}&Revisao={Uri.EscapeDataString(revisao)}";

            // Use the injected HttpClient instead of creating a new one
            var response = await _http.GetAsync(fileURL);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // file not found on server - nothing to save
                return null;
            }


            /////////////////



            //var fileURL = $"{_uri}/Dxf/Download?Usuario={usuario}&Arquivo={Uri.EscapeDataString(nomeCompletoDoArquivo)}";

            
           
            //HttpClient httpClient = new HttpClient();

            //var response = await httpClient.GetAsync(fileURL);

            
            var diretorioSalvamento = dir + @"\Download";

            
            if (!Directory.Exists(diretorioSalvamento))
            {
                Directory.CreateDirectory(diretorioSalvamento);
            }



            var filePath = Path.Combine(dir, $"{nomeCompletoDoArquivo}.dxf");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            var arquivoSalvamento = Path.Combine(diretorioSalvamento, $"{nomeCompletoDoArquivo}.dxf");
            if (System.IO.File.Exists(arquivoSalvamento))
            {
                System.IO.File.Delete(arquivoSalvamento);
            }



            using (var fs = new FileStream(arquivoSalvamento, FileMode.CreateNew))
            {
                await response.Content.CopyToAsync(fs);
            }

            return arquivoSalvamento;
        }
    }



   
}
