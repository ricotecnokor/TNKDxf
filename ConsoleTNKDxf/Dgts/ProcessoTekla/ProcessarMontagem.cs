using netDxf;
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
            var desenhoDgt = new DesenhoMontagemDgt(_gaDrawing, _model, camposFormato, coletorLm);
            var xDadosFormato = new XDadosFormato<ConjuntoMontagemDgt>(dxf, desenhoDgt);
            xDadosFormato.InserirInformacoes(versaoTsep, _tipo);
        }
    }
}
