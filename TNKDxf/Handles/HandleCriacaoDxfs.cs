using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNKDxf.ViewModel.Abstracoes;

namespace TNKDxf.Handles
{
    public class HandleCriacaoDxfs
    {
        
        private readonly IExtratorDXFs _extrator;
        private readonly AvaliadorDesenhos _avaliadorDesenhos;
        private int _contadorProcessados = 1;
        public static HandleCriacaoDxfs _instancia;

        private HandleCriacaoDxfs(IExtratorDXFs extrator, AvaliadorDesenhos avaliadorDesenhos) 
        {
            _extrator = extrator;   
            _avaliadorDesenhos = avaliadorDesenhos;
        }

        public int Aprocessar => _extrator.Extraidos.ToList().Count();

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

        public static void CriarManipulapor(IExtratorDXFs extrator, AvaliadorDesenhos avaliadorDesenhos)
        {
            if (_instancia == null)
            {
                 _instancia = new HandleCriacaoDxfs(extrator, avaliadorDesenhos);
            }
               
        }

        public async Task Download(string arquivo)
        {
           await _avaliadorDesenhos.Download(arquivo, _extrator.Xsplot + @"\Download");
        }

       
        public IEnumerable<string> ObterExtraidos()
        {
            return _extrator.Extraidos;
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
            var aProcessar = _extrator.Extraidos.ToList().Count();
            return _contadorProcessados < aProcessar;
        }
    }
}
