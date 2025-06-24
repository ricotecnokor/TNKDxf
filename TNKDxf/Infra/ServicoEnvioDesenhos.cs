using System.Collections.Generic;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TNKDxf.Infra
{
    public class ServicoEnvioDesenhos : IServicoEnvioDesenhos
    {

        private readonly string _url;

        public ServicoEnvioDesenhos(CfgEngAPI tecnokorAPI)
        {
            _url = $"{tecnokorAPI.URI}/api/Dxf";
        }

      

        public void UploadAsync(string file, string softwareOrigem, string usuario, string padrao)
        {
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

                        client.PostAsync(_url, formContent).Wait();

                    }

                }

            }
            catch (Exception ex)
            {
                respostas.Aprovado = false;
                respostas.Respostas.Add(softwareOrigem, "Não processada.");

            }

          
        }


    }

    public class ResultConvertDxf
    {
        public bool Aprovado { get; set; }
        public Dictionary<string, string> Respostas { get; set; }
    }
}
