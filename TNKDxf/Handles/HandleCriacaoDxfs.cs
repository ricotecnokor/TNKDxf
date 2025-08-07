using System;
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

        private HandleCriacaoDxfs(IExtratorDXFs extrator, AvaliadorDesenhos avaliadorDesenhos) //string projeto, string username, string exportPath)
        {
            _extrator = extrator;   
            _avaliadorDesenhos = avaliadorDesenhos;
            //_model = new TSM.Model();
        }

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
            _instancia = new HandleCriacaoDxfs(extrator, avaliadorDesenhos);
        }

        public async Task Manipular()
        {
            
            if (!_extrator.ForamExtraidos)
            {
                _extrator.Extrair();

                _contadorProcessados = _extrator.Extraidos.ToList().Count();

                foreach (string desenho in _extrator.Extraidos)
                {
                    await _avaliadorDesenhos.Avaliar(desenho);
                    _contadorProcessados--;
                }

            }
        }

        public async Task Download(string arquivo)
        {
           await _avaliadorDesenhos.Download(arquivo, _extrator.Xsplot + @"\Download");
        }

        public async Task Manipular(string nome)
        {
            if (!_extrator.ForamExtraidos)
            {
                _extrator.Extrair();
                foreach (string desenho in _extrator.Extraidos)
                {
                    await _avaliadorDesenhos.Avaliar(desenho);

                }

            }
        }


        public IEnumerable<string> ObterExtraidos()
        {
            return _extrator.Extraidos;
        }

        //public List<CommandResult> ObterResultados()
        //{
        //   return _avaliadorDesenhos.ObterResultados();
        //}

        public bool VerificarSePossuiErro(string desenho)
        {
            return _avaliadorDesenhos.VerificarSePossuiErro(desenho); 
        }

        public CommandResult ObterResult(string nome)
        {
            return _avaliadorDesenhos.ObterResult(nome);
        }
    }
}
