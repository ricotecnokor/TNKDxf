using System.Collections.Generic;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class DesenhoMontagemDgt : DesenhoDgtAbs<ConjuntoMontagemDgt>
    {
        public DesenhoMontagemDgt(MultiDrawing multiDrawing, Model model, CamposFormatoDgt camposFormatoDgt, LmMontagemDgt coletorLm) : base(multiDrawing, model, camposFormatoDgt, coletorLm)
        {
            List<string> strings = coletorLm.ObterPrefixosConjuntos();
            _elementosFixacao = new ElementosFixacaoDgt(model, multiDrawing, strings);
            setListarElementosObra(multiDrawing);
        }

       
    }
}
