using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Infra.Dtos;

namespace TNKDxf.Infra
{
    public interface IServicoEnvioDesenhos
    {
        void UploadAsync(string file, string softwareOrigem, string usuario, string padrao);

        List<ArquivoDTO> ListaProcessadosAsync(string usuario, string padrao);
    }
}
