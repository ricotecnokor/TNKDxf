using System.Collections.Generic;
using System.Threading.Tasks;
using TNKDxf.Handles;
using TNKDxf.Infra.Dtos;

namespace TNKDxf.Infra
{
    public interface IServicoEnvioDesenhos
    {
        Task<CommandResult> UploadAsync(string file, string softwareOrigem, string usuario, string padrao);

        Task DownloadFile(string usuario, string padrao, string aplicativo, string fileName);
       List<string> ListaProcessadosAsync(string usuario, string padrao);
    }
}
