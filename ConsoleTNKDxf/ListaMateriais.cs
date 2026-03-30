using netDxf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class ListaMateriais : IEnumerable<Conjunto>
    {
        TSM.Model _model;
        private List<Conjunto> _conjuntos = new List<Conjunto>();
        public List<Conjunto> Conjuntos => _conjuntos;

        public ListaMateriais(TSM.Model model, MultiDrawing drawing)
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
            if (assemblyPos == string.Empty)
            {
                Console.WriteLine("A posição do conjunto não pode ser nula ou vazia.");
                return;
            }


            //string posicaoItemPrincipal = assy.ObterPosicaoItemPrincipal();

            if (_conjuntos.Any(conjunto => conjunto.Posicao == assemblyPos))
            {
                Conjunto conjuntoExistente = _conjuntos.FirstOrDefault(c => c.Posicao == assemblyPos);
                conjuntoExistente.AddItem(part);
                return;
            }

            var marca = new Conjunto(part);
            _conjuntos.Add(marca);
        }

        public IEnumerator<Conjunto> GetEnumerator()
        {
            return _conjuntos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
