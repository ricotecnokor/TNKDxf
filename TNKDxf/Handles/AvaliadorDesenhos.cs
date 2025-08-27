using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Infra;

namespace TNKDxf.Handles
{
    public class AvaliadorDesenhos
    {

        IServicoEnvioDesenhos _servicoEnvioDesenhos;
        //FileInfo[] _arquivosProcessar = null;
        private List<CommandResult> _resultados;
        protected string _userName;
        protected string _projeto;
        protected string _exportPath;
        public AvaliadorDesenhos(string exportPath, string projeto, string username)
        {
            _exportPath = exportPath;
            _projeto = projeto;
            _userName = username;
            _resultados = new List<CommandResult>();
            
            _servicoEnvioDesenhos = new ServicoEnvioDesenhos(new CfgEngAPI());
        }

        public async Task<CommandResult> Avaliar(string desenho)
        {
            DirectoryInfo diretorioRecebidos = new DirectoryInfo(_exportPath);
            FileInfo[] _arquivosProcessar = diretorioRecebidos.GetFiles("*.dxf");
            //var arquivo = Path.Combine(_exportPath, desenho + ".dxf");
            var arquivo = _arquivosProcessar.FirstOrDefault(a => a.Name.Contains(desenho));
            var resp = await _servicoEnvioDesenhos.UploadAsync(arquivo.FullName, "Tekla Structures", _userName, _projeto);
           
            if (resp != null)
            {
                //_resultados.Add(resp);
                return resp;
            }

            return new CommandResult
            {
                Success = false,
                Message = "Erro ao processar o arquivo API não encontrada.",
                Resultado = desenho,
                Notifications = new List<Notification>()
            };

        }

        public async Task Download(string fileName, string diretorioSalvar)
        {
            await _servicoEnvioDesenhos.DownloadFile(_userName, _projeto, "Tekla Structures", fileName, diretorioSalvar);
        }

        //public List<CommandResult> ObterResultados()
        //{
        //    return _resultados; 
        //}

        public bool VerificarSePossuiErro(string desenho)
        {
          return  _resultados.All(r => r.Resultado != desenho || r.Success == false);
        }

        public CommandResult ObterResult(string nome)
        {
            return _resultados.FirstOrDefault(r => r.Resultado == nome);
        }

        public void IncluirResultado(CommandResult resultado)
        {
            _resultados.Add(resultado);
        }   

    }
}
