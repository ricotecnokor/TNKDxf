using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using TNKDxf.Infra.Dtos;

namespace TNKDxf.Infra
{
    public class ServicoEnvioDesenhos : IServicoEnvioDesenhos
    {
        private readonly string _uri;

        public ServicoEnvioDesenhos(CfgEngAPI tecnokorAPI)
        {
            _uri = $"{tecnokorAPI.URI}/api";
        }

        public void UploadAsync(string file, string softwareOrigem, string usuario, string padrao)
        {
            var url = $"{_uri}/Dxf";
            ResultConvertDxf respostas = new ResultConvertDxf();
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

                        client.PostAsync(url, formContent).Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                respostas.Aprovado = false;
                respostas.Respostas.Add(softwareOrigem, "Não processada.");
            }
        }

        public List<ArquivoDTO> ListaProcessadosAsync(string usuario, string padrao)
        {
            var url = $"{_uri}/GetListaProcessados?Usuario={usuario}";
            try
            {
                var http = new HttpClient();

                var response = http.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    var listaProcessados = JsonConvert.DeserializeObject<List<ArquivoDTO>>(content); // Fix: Use JsonConvert.DeserializeObject
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

            return new List<ArquivoDTO>();
        }
    }



    public class ResultConvertDxf
    {
        public bool Aprovado { get; set; }
        public Dictionary<string, string> Respostas { get; set; }
    }
}
