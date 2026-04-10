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
        private string _prefixoConjunto;

        public ListaMateriaisDtg(TSM.Model model, TSD.MultiDrawing drawing, string prefixoConjunto)
        {
            _model = model;
            _prefixoConjunto = prefixoConjunto;
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

            string assemblyPos = assy.ObterPropriedade("ASSEMBLY_POS").ToString();

            if (!assemblyPos.Contains(_prefixoConjunto))
            {
                return;
            }



            var partPos = part.ObterPropriedade("PART_POS").ToString();

            if (assemblyPos == string.Empty)
            {
                Console.WriteLine("A posição do conjunto não pode ser nula ou vazia.");
                return;
            }

            if (_conjuntos.Any(conjunto => conjunto.AssemblyPos == assemblyPos))
            {

                ConjuntoDgt conjuntoExistente = _conjuntos.FirstOrDefault(c => c.AssemblyPos == assemblyPos);
                
                
                conjuntoExistente.AddItem(part, assemblyPos);
                return;
            }

            //assy.GetSecondaries

            var novoConjunto = new ConjuntoDgt(part, new PecaDgt(assy.GetMainPart() as TSM.Part));
            _conjuntos.Add(novoConjunto);
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
