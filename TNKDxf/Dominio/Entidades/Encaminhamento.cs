using System.IO;
using TNKDxf.Dominio.Dxfs;

namespace TNKDxf.Dominio.Entidades
{
    public class Encaminhamento
    {
        private ArquivoDxf _arquivoDxf;
        private string _usuario;
      
       

        public Encaminhamento(ArquivoDxf arquivoDxf, string usuario)
        {
            _arquivoDxf = arquivoDxf;
            _usuario = usuario;
        }

        public string Encaminhar(string caminhoSalvamento)
        {
            if (!Directory.Exists(caminhoSalvamento))
            {
                Directory.CreateDirectory(caminhoSalvamento);
            }

            caminhoSalvamento = Path.Combine(caminhoSalvamento, _usuario);
            if (!Directory.Exists(caminhoSalvamento))
            {
                Directory.CreateDirectory(caminhoSalvamento);
            }
        
            return Path.Combine(caminhoSalvamento, _arquivoDxf.Nome);
        }
    }
}
