using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using static Tekla.Structures.Model.ReferenceModel;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Desenho
    {
        TSM.Model _model;
        TSD.MultiDrawing _multiDrawing;
        private CamposFormato _camposFormato;
        private Revisao _revisao;
        private QuadroAplicacaoDgt _quadroAplicacao;
        private ListaMateriais _listaMateriais;
        private ElementosFixacao _elementosFixacao;
        //private string _nomeModelo;
        private DateTime _data;
        //private string _numeroProjeto;

        public string Title1 => _multiDrawing.Title1;

        public Revisao QuadroRevisao => _revisao;
        public string RevisaoFormato => _camposFormato.Revisao;
        public string RevisaoFormatoCliente => _camposFormato.RevisaoCliente;
        public QuadroAplicacaoDgt QuadroAplicacao => _quadroAplicacao;
        //public PropriedadesDesenho PropriedadesFormato => ;

        public ListaMateriais ListaMateriais => _listaMateriais;
        public ElementosFixacao ElementosFixacao => _elementosFixacao;

        public string NomeModelo => _camposFormato.NomeModelo;
        public string Data => _data.ToString();
        public object NumeroProjeto => _camposFormato.NumeroProjeto;
        public object NumeroContratada => _camposFormato.NumeroContratada;
        public object NumeroCliente => _camposFormato.NumeroCliente;
        public object DescricaoProjeto => _camposFormato.DescricaoProjeto;
        public object RevisaoCliente => _camposFormato.RevisaoCliente;
        public object Escala => _camposFormato.Escala;

        public Desenho(TSD.MultiDrawing multiDrawing, TSM.Model model)
        {
            //_nomeArquivo = nomeArquivo;
            _multiDrawing = multiDrawing;
            _model = model;
            _camposFormato = new CamposFormato(multiDrawing);
            _revisao = new Revisao(multiDrawing);
            _quadroAplicacao = new QuadroAplicacaoDgt(multiDrawing);
            _listaMateriais = new ListaMateriais(model, multiDrawing);
            _elementosFixacao = new ElementosFixacao(model, multiDrawing);

            //LeitorRlatorioDesenhosTekla leitor = new LeitorRlatorioDesenhosTekla("multiTemp.rpt");
            //var relatorio = leitor.Ler();
            //_momeModelo = relatorio.NomeModelo;
            //_data = relatorio.Data;
            //_numeroProjeto = relatorio.NumeroProjeto;
            //_propriedadesDesenho = relatorio.PegaPropriedades(_nomeArquivo);
        }

        //public void AddPeca(TSM.Part part)
        //{
        //    var assy = part.GetAssembly();
        //    if (assy == null)
        //    {
        //        return;
        //    }

             

        //    string assemblyPos = assy.ObterPropriedade("ASSEMBLY_POS").ToString();
        //    if (assemblyPos == string.Empty)
        //    {
        //        Console.WriteLine("A posição do conjunto não pode ser nula ou vazia.");
        //        return;
        //    }
            

        //    //string posicaoItemPrincipal = assy.ObterPosicaoItemPrincipal();

        //    if (_listaMateriais.Any(conjunto => conjunto.Posicao == assemblyPos))
        //    {
        //        Conjunto conjuntoExistente = _listaMateriais.FirstOrDefault(c => c.Posicao == assemblyPos);
        //        conjuntoExistente.AddItem(part);
        //        return;
        //    }

        //    var marca = new Conjunto(part);
        //    _listaMateriais.Add(marca);
        //}

    }
}
