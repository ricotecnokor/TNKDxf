using System;
using System.Collections;
using System.Collections.Generic;

namespace TNKDxf.TeklaManipulacao
{
    public class RelatorioMultiDesenhos : IEnumerable<PropriedadesDesenho>
    {

        private readonly string _nomeModelo = string.Empty;
        private readonly string _numeroProjeto = string.Empty;
        private readonly DateTime _data = DateTime.Now;

        private readonly List<PropriedadesDesenho> _propriedadesDesenhos = new List<PropriedadesDesenho>();

        public string NomeModelo => _nomeModelo;

        public string NumeroProjeto => _numeroProjeto;

        public DateTime Data => _data;

        public RelatorioMultiDesenhos(string nomeModelo, string numeroProjeto, string dia, string hora)
        {
            _nomeModelo = nomeModelo;
            _numeroProjeto = numeroProjeto;
            _data = new DateTime(
                Convert.ToInt32(dia.Split('.')[2]),
                Convert.ToInt32(dia.Split('.')[1]),
                Convert.ToInt32(dia.Split('.')[0]),
                Convert.ToInt32(hora.Split(':')[0]),
                Convert.ToInt32(hora.Split(':')[1]),
                Convert.ToInt32(hora.Split(':')[2])
            );
        }

        public IEnumerator<PropriedadesDesenho> GetEnumerator()
        {
            return _propriedadesDesenhos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AdicionarPropriedadesDesenho(PropriedadesDesenho propriedades)
        {
            _propriedadesDesenhos.Add(propriedades);
        }

    }

    public class PropriedadesDesenho
    {
        private readonly string _numeroContratada = string.Empty;
        private readonly string _numeroCliente = string.Empty;
        private readonly string _descricaoProjeto = string.Empty;
        private readonly string _areaSubarea = string.Empty;
        private readonly string _tituloDesenho = string.Empty;
        private readonly string _subtitulo1Desenho = string.Empty;
        private readonly string _subtitulo2Desenho = string.Empty;
        private readonly string _revisao = string.Empty;
        private readonly string _revisaoCliente  = string.Empty;

        public PropriedadesDesenho(string numeroContratada, string numeroCliente, string descricaoProjeto, string areaSubarea, string tituloDesenho, string subtitulo1Desenho, string subtitulo2Desenho, string revisao, string revisaoCliente)
        {
            _numeroContratada = numeroContratada;
            _numeroCliente = numeroCliente;
            _descricaoProjeto = descricaoProjeto;
            _areaSubarea = areaSubarea;
            _tituloDesenho = tituloDesenho;
            _subtitulo1Desenho = subtitulo1Desenho;
            _subtitulo2Desenho = subtitulo2Desenho;
            _revisao = revisao;
            _revisaoCliente = revisaoCliente;
        }

        public string NumeroContratada => _numeroContratada;

        public string NumeroCliente => _numeroCliente;

        public string DescricaoProjeto => _descricaoProjeto;

        public string AreaSubarea => _areaSubarea;

        public string TituloDesenho => _tituloDesenho;

        public string Subtitulo1Desenho => _subtitulo1Desenho;

        public string Subtitulo2Desenho => _subtitulo2Desenho;

        public string Revisao => _revisao;

        public string RevisaoCliente => _revisaoCliente;
    }

}
