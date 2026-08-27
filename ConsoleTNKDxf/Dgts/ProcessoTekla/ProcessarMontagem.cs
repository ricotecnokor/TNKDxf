using netDxf;
using System.Collections.Generic;
using Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts.ProcessoTekla
{
    public class ProcessarMontagem : AbsProcessador
    {
        GADrawing _gaDrawing;

        public ProcessarMontagem(TSM.Model model, Drawing drawing, string criarLM, string listarElementosObra) : base(model, drawing, criarLM, listarElementosObra)
        {
            _tipo = "MONTAGEM";
            _gaDrawing = _drawing as GADrawing;
        }

        public override void Processar(string versaoTsep, DxfDocument dxf)
        {
            var camposFormato = new CamposFormatoDgt(_gaDrawing);
            var coletorLm = new LmMontagemDgt(_model);
            coletorLm.Coletar(_gaDrawing);
            //var desenhoDgt = new DesenhoMontagemDgt(_gaDrawing, _model, camposFormato, coletorLm);
            setListarElementosObra(_gaDrawing);
            var elementosFixacao = new ElementosFixacaoDgt(_model);
            elementosFixacao.Coletar(_gaDrawing, new List<string> { int.Parse(camposFormato.Title1.Split('-')[3]).ToString() }, coletorLm);
            var quadroAplicacao = new QuadroAplicacaoDgt(_gaDrawing);
            var xDadosFormato = new XDadosFormato<ConjuntoMontagemDgt>(dxf, _criarLM, _listarElementosObra, camposFormato, elementosFixacao, coletorLm, quadroAplicacao);
            xDadosFormato.InserirInformacoes(versaoTsep, _tipo);
        }
    }
}
