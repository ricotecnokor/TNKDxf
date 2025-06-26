using System.Collections.Generic;
using TNKDxf.ViewModel;

namespace TNKDxf.Infra.Dtos
{
    public class ArquivoDTO
    {

        public List<CampoErroWpf> Erros { get; set; } = new List<CampoErroWpf>();
        public string Nome { get; set; }
    }

   
}
