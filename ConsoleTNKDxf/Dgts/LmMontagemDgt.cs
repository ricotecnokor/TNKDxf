using System;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class LmMontagemDgt : LmAbs<ConjuntoMontagemDgt>
    {
        public LmMontagemDgt(Model model) : base(model)
        {
        }

        public override void Coletar(Drawing multiDrawing)
        {
            HashSet<Identifier> pecasUnicasNoDesenho = obterPecasUnicasDesenho(multiDrawing);

            foreach (Identifier partId in pecasUnicasNoDesenho)
            {
                var modelObj = _model.SelectModelObject(partId);

                if (modelObj is TSM.Part modelPart)
                {
                    addPeca(modelPart);
                }
            }
        }

        private HashSet<Identifier> obterPecasUnicasDesenho(Drawing multiDrawing)
        {
            HashSet<Identifier> pecasUnicasNoDesenho = new HashSet<Identifier>();

            var views = multiDrawing.GetSheet().GetAllViews().GetEnumerator();
            while (views.MoveNext())
            {
                var view = views.Current as TSD.View;
                if (view == null) continue;

                DrawingObjectEnumerator drawingParts = view.GetObjects(new[] { typeof(TSD.Part) });
                while (drawingParts.MoveNext())
                {
                    TSD.Part drwPart = drawingParts.Current as TSD.Part;
                    if (drwPart != null)
                    {
                        var tipoLinhasVisiveis = drwPart.Attributes.VisibleLines.Type;
                        string tipoLinhasVisiveisStr = tipoLinhasVisiveis.ToString();
                        if (tipoLinhasVisiveisStr == "SolidLine")
                        {
                            pecasUnicasNoDesenho.Add(drwPart.ModelIdentifier);
                        }
                    }
                }
            }

            return pecasUnicasNoDesenho;
        }

        public void addPeca(TSM.Part part)
        {
            var assy = part.GetAssembly();
            if (assy == null)
            {
                return;
            }

            string assemblyPos = assy.ObterPropriedade("ASSEMBLY_POS").ToString();

            if (assemblyPos == string.Empty)
            {
                Console.WriteLine("A posição do conjunto não pode ser nula ou vazia.");
                return;
            }

            if (_conjuntos.Any(conjunto => conjunto.AssemblyPos == assemblyPos))
            {
                ConjuntoMontagemDgt conjuntoExistente = _conjuntos.FirstOrDefault(c => c.AssemblyPos == assemblyPos);

                conjuntoExistente.AddItem(part, assemblyPos);
                var pecaAdiconal = new PecaDgt(assy.GetMainPart() as TSM.Part);
                conjuntoExistente.AddPeso(pecaAdiconal.WeightNet);
                return;
            }

            var pecaPrincipal = new PecaDgt(assy.GetMainPart() as TSM.Part);
            var novoConjunto = new ConjuntoMontagemDgt(part);
            novoConjunto.AddPeso(pecaPrincipal.WeightNet);
            _conjuntos.Add(novoConjunto);
        }
    }
}
