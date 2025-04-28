using Newtonsoft.Json;
using System.Net;
using TNKDxf.Infra.Dtos;

namespace TNKDxf.Infra
{
    public class ServicoFormatacao
    {
        private WebClient _webClient;
        private CfgEngAPI _cfg;
        public ServicoFormatacao()
        {
            _cfg = new CfgEngAPI();
            _webClient = new WebClient();
        }

        public FormatacaoDTO ObterFormatacao(string projeto)
        {
          



            var text = _webClient.DownloadString($"{_cfg.URI}/api/formatacao/{projeto}");
            return JsonConvert.DeserializeObject<FormatacaoDTO>(text);
        }
     }
}
