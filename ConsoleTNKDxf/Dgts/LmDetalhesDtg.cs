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
    public class LmDetalhesDtg : LmAbs<ConjuntoDetalhadoDgt>
    {
        private string _prefixoConjunto;

        public LmDetalhesDtg(Model model, string prefixoConjunto) : base(model)
        {
            _prefixoConjunto = prefixoConjunto;
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
                        pecasUnicasNoDesenho.Add(drwPart.ModelIdentifier);
                    }
                }
            }

            return pecasUnicasNoDesenho;
        }

        public void addPeca(TSM.Part part)
        {
            TSM.Assembly assy = part.GetAssembly();
            if (assy == null)
            {
                return;
            }

            string assemblyPos = assy.ObterPropriedade("ASSEMBLY_POS").ToString();

            if (!assemblyPos.Contains(_prefixoConjunto))
            {
                return;
            }

            if (assemblyPos == string.Empty)
            {
                Console.WriteLine("A posição do conjunto não pode ser nula ou vazia.");
                return;
            }

            if (_conjuntos.Any(conjunto => conjunto.AssemblyPos == assemblyPos))
            {
                ConjuntoDetalhadoDgt conjuntoExistente = _conjuntos.FirstOrDefault(c => c.AssemblyPos == assemblyPos);

                conjuntoExistente.AddItem(part, assy);

                return;
            }

            var novoConjunto = new ConjuntoDetalhadoDgt(part);
            _conjuntos.Add(novoConjunto);
        }

        internal bool ContemPeca(string partPos)
        {
            return _conjuntos.Any(conjunto => conjunto.Itens.Any(peca => peca.PartPos == partPos));
        }

        internal void Ligar(FixacaoDgt fixacao, string tipoFixacao)
        {
            var marcaConjunto = fixacao.Parafuso.PecaChega.MarcaMontagem;

            if (!_conjuntos.Any(c => c.AssemblyPos == marcaConjunto))
                return;

            ConjuntoDetalhadoDgt conjunto = (ConjuntoDetalhadoDgt)_conjuntos.FirstOrDefault(c => c.AssemblyPos == marcaConjunto);

            List<FixacaoDgt> fixacaoList = tipoFixacao == "BOLT_TYPE_WORKSHOP" ? conjunto.FixacaoFabrica : conjunto.FixacaoObra;
            if (!fixacaoList.Contains(fixacao))
            {
                fixacaoList.Add(fixacao);
            }
        }
    }
}
