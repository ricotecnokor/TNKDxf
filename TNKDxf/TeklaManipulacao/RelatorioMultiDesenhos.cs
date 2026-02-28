using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

        public RelatorioMultiDesenhos(string line)//string nomeModelo, string numeroProjeto, string dia, string hora)
        {
            

            var linha = line.Split(';');

            _nomeModelo = linha[1].Trim();
            _numeroProjeto = linha[2].Trim();
            _data = new DateTime(
                Convert.ToInt32(linha[3].Trim().Split('.')[2]),
                Convert.ToInt32(linha[3].Trim().Split('.')[1]),
                Convert.ToInt32(linha[3].Trim().Split('.')[0]),
                Convert.ToInt32(linha[4].Trim().Split(':')[0]),
                Convert.ToInt32(linha[4].Trim().Split(':')[1]),
                Convert.ToInt32(linha[4].Trim().Split(':')[2])
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

        public void AdicionarPropriedadesDesenho(string line)
        {
          
            var linha = line.Split(';');
            _propriedadesDesenhos.Add(new PropriedadesDesenho(
                                        linha[1].Trim(),
                                        linha[2].Trim(),
                                        linha[3].Trim(),
                                        linha[4].Trim(),
                                        linha[5].Trim(),
                                        linha[6].Trim(),
                                        linha[7].Trim(),
                                        linha[8].Trim(),
                                        linha[9].Trim()));
        }

        public PropriedadesDesenho PegaPropriedades(string nomeArquivo)
        {
            string nome = nomeArquivo.Split('\\').Last().Split(' ').First();
            return _propriedadesDesenhos.Find(x => x.NumeroContratada.StartsWith(nome));
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
        private readonly string _revisaoCliente = string.Empty;

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
