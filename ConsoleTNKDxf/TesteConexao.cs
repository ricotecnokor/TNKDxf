using System;
using System.Configuration;
using System.Net.Http;

namespace ConsoleTNKDxf
{
    /// <summary>
    /// Testa a conectividade até uma URL (ex.: https://app.edsbim.com/) e informa se o
    /// aplicativo conseguiu alcançá-la. Qualquer resposta HTTP (mesmo 401/404) significa
    /// que a rede chegou ao servidor.
    /// </summary>
    public static class TesteConexao
    {
        public static string UrlPadrao => "https://app.edsbim.com/";

        public static bool Testar(out string mensagem)
        {
            string url = ConfigurationManager.AppSettings["UrlTesteConexao"];
            if (string.IsNullOrWhiteSpace(url))
            {
                url = UrlPadrao;
            }

            int timeout = 15;
            int.TryParse(ConfigurationManager.AppSettings["UrlTesteTimeoutSegundos"], out timeout);
            if (timeout <= 0) timeout = 15;

            try
            {
                using (var http = new HttpClient())
                {
                    http.Timeout = TimeSpan.FromSeconds(timeout);

                    using (var resposta = http.GetAsync(url).Result)
                    {
                        mensagem = $"Conexão com {url} OK (HTTP {(int)resposta.StatusCode}).";
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                mensagem = $"Não foi possível conectar em {url}: {ex.Message}";
                return false;
            }
        }
    }
}
