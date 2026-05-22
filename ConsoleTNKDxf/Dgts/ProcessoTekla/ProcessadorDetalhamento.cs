using netDxf;
using System.Reflection;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class ProcessadorDetalhamento : AbsProcessador
    {

        MultiDrawing _multiDrawing;

        public ProcessadorDetalhamento(TSM.Model model, Drawing drawing, string criarLM, string listarElementosObra) : base(model, drawing, criarLM, listarElementosObra)
        {
            _tipo = "DETALHE";
            _multiDrawing = _drawing as MultiDrawing;
        }

     
       

        //public ProcessadorDetalhamento(TSM.Model model):(model) 
        //{
           

            
        //        _tipo = "DETALHE";
            

        //    PropertyInfo propInfo = draw.GetType().GetProperty("Identifier",
        //                                BindingFlags.Instance | BindingFlags.NonPublic);
        //    object value = propInfo.GetValue(draw, null);
        //    Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

        //    Beam tempBeam = new Beam();
        //    tempBeam.Identifier = identifier;

        //    tempBeam.GetReportProperty("TCNM_LISTAR_PARAF", ref _listarElementosObra);
        //    tempBeam.GetReportProperty("TCNM_CRIAR_LM", ref _criarLM);
        //}


        public override void Processar(string versaoTsep, DxfDocument dxf)
        {
            var camposFormato = new CamposFormatoDgt(_multiDrawing);
            string prefixoConjunto = int.Parse(camposFormato.Title1.Split('-')[3]).ToString();
            var coletorLm = new LmDetalhesDtg(_model, prefixoConjunto);
            var desenhoDgt = new DesenhoDetalhesDgt(_multiDrawing, _model, camposFormato, coletorLm);
            var xDadosFormato = new XDadosFormato<ConjuntoDetalhadoDgt>(dxf, desenhoDgt);
            xDadosFormato.InserirInformacoes(versaoTsep, _tipo);
        }

        

        
    }
}
