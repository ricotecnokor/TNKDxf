using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNKDxf.Infra
{
    public interface IServicoEnvioDesenhos
    {
        void UploadAsync(string file, string softwareOrigem, string usuario, string padrao);
    }
}
