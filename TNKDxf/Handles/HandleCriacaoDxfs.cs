using Dynamic.Tekla.Structures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TSM = Dynamic.Tekla.Structures.Model;

namespace TNKDxf.Handles
{
    public class HandleCriacaoDxfs
    {
        
        //private readonly IExtratorDXFs _extrator;
        private readonly AvaliadorDesenhos _avaliadorDesenhos;
        private int _contadorProcessados = 1;
        public static HandleCriacaoDxfs _instancia;

        private HandleCriacaoDxfs(AvaliadorDesenhos avaliadorDesenhos) 
        {
            //_extrator = extrator;   
            _avaliadorDesenhos = avaliadorDesenhos;
        }

        public int Aprocessar => ExtratorDXFs.GetInstance().Extraidos.ToList().Count();  //_extrator.Extraidos.ToList().Count();

        public static HandleCriacaoDxfs Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    throw new System.Exception("Instância não criada. Use CriarManipulapor para inicializar.");
                }
                return _instancia;
            }
        }

        public int ContadorProcessados { get => _contadorProcessados; private set => _contadorProcessados = value; }

        public static void CriarManipulapor(AvaliadorDesenhos avaliadorDesenhos)
        {
            if (_instancia == null)
            {
                 _instancia = new HandleCriacaoDxfs(avaliadorDesenhos);
            }
               
        }

        public async Task Download(string arquivo)
        {
            await _avaliadorDesenhos.Download(arquivo);
        }

       
        public IEnumerable<string> ObterExtraidos()
        {
            return ExtratorDXFs.GetInstance().Extraidos;
        }

        public bool VerificarSePossuiErro(string desenho)
        {
            return _avaliadorDesenhos.VerificarSePossuiErro(desenho); 
        }

        public CommandResult ObterResult(string nome)
        {
            return _avaliadorDesenhos.ObterResult(nome);
        }

        public bool NaoProcessou()
        {
            var aProcessar = ExtratorDXFs.GetInstance().Extraidos.ToList().Count();
            return _contadorProcessados < aProcessar;
        }
    }
}
