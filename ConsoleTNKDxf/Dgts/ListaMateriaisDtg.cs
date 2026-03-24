using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
   

    public class ListaMateriaisDtg : IEnumerable<ConjuntoDgt>
    {
        TSM.Model _model;
        private List<ConjuntoDgt> _conjuntos = new List<ConjuntoDgt>();
        public List<ConjuntoDgt> Conjuntos => _conjuntos;

        public ListaMateriaisDtg(TSM.Model model, TSD.MultiDrawing drawing)
        {
            _model = model;
            coletar(drawing);
        }

        private void coletar(MultiDrawing multiDrawing)
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

            
            foreach (var conjuntoAvulso in _conjuntos)
            {
                conjuntoAvulso.MultiplicaQtdConjuntoPorPeca();
            }
        }

        private HashSet<Identifier> obterPecasUnicasDesenho(MultiDrawing multiDrawing)
        {
            HashSet<Identifier> pecasUnicasNoDesenho = new HashSet<Identifier>();

            var views = multiDrawing.GetSheet().GetAllViews().GetEnumerator();
            while (views.MoveNext())
            {

                var view = views.Current as TSD.View;
                if (view == null) continue;

                // 2. Pegar todas as partes gráficas nesta vista
                DrawingObjectEnumerator drawingParts = view.GetObjects(new[] { typeof(TSD.Part) });
                while (drawingParts.MoveNext())
                {
                    TSD.Part drwPart = drawingParts.Current as TSD.Part;
                    if (drwPart != null)
                    {
                        pecasUnicasNoDesenho.Add(drwPart.ModelIdentifier);
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

            var mainPart = assy.GetMainPart() as TSM.Part;
            PecaDgt pecaDgtMaiPart = new PecaDgt(mainPart);

            string assemblyPos = assy.ObterPropriedade("ASSEMBLY_POS").ToString();
           


            var partPos = part.ObterPropriedade("PART_POS").ToString();
            if (partPos == "18")
            {
                var x = 0;
            }


            if (assemblyPos == string.Empty)
            {
                Console.WriteLine("A posição do conjunto não pode ser nula ou vazia.");
                return;
            }

            if (_conjuntos.Any(conjunto => conjunto.AssemblyPos == assemblyPos))
            {
                if (assemblyPos.Contains("AG"))
                {
                    var x = 0;
                }


                ConjuntoDgt conjuntoExistente = _conjuntos.FirstOrDefault(c => c.AssemblyPos == assemblyPos);
                
                
                conjuntoExistente.AddItem(part, assemblyPos);
                return;
            }

            //assy.GetSecondaries


            var marca = new ConjuntoDgt(part, pecaDgtMaiPart);
            _conjuntos.Add(marca);
        }

        public IEnumerator<ConjuntoDgt> GetEnumerator()
        {
            return _conjuntos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
