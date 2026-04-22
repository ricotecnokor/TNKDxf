using ConsoleTNKDxf.Abstracoes;
using netDxf.Tables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override void Coletar(MultiDrawing multiDrawing)
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


            //foreach (var conjuntoAvulso in _conjuntos)
            //{
            //    conjuntoAvulso.MultiplicaQtdConjuntoPorPeca();
            //}
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
                        var tipoLinhasVisiveis = drwPart.Attributes.VisibleLines.Type;
                        string tipoLinhasVisiveisStr = tipoLinhasVisiveis.ToString();
                        if (tipoLinhasVisiveisStr == "SolidLine")
                        {

                            pecasUnicasNoDesenho.Add(drwPart.ModelIdentifier);
                            // Encontramos um objeto relacionado que é uma peça do modelo
                            // Podemos parar de iterar pelos objetos relacionados, pois já temos a peça
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

            //if (!assemblyPos.Contains(_prefixoConjunto))
            //{
            //    return;
            //}



            var partPos = part.ObterPropriedade("PART_POS").ToString();

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

            //assy.GetSecondaries
            var pecaPrincipal = new PecaDgt(assy.GetMainPart() as TSM.Part);
            var novoConjunto = new ConjuntoMontagemDgt(part, pecaPrincipal);
            novoConjunto.AddPeso(pecaPrincipal.WeightNet);
            _conjuntos.Add(novoConjunto);
        }
        //public void addPeca(TSM.Part part)
        //{
        //    var assy = part.GetAssembly();
        //    if (assy == null)
        //    {
        //        return;
        //    }

        //    string assemblyPos = assy.ObterPropriedade("ASSEMBLY_POS").ToString();

        //    //if (!assemblyPos.Contains(_prefixoConjunto))
        //    //{
        //    //    return;
        //    //}



        //    var partPos = part.ObterPropriedade("PART_POS").ToString();

        //    if (assemblyPos == string.Empty)
        //    {
        //        Console.WriteLine("A posição do conjunto não pode ser nula ou vazia.");
        //        return;
        //    }

        //    if (_conjuntos.Any(conjunto => conjunto.AssemblyPos == assemblyPos))
        //    {

        //        //ConjuntoMontagemDgt conjuntoExistente = _conjuntos.FirstOrDefault(c => c.AssemblyPos == assemblyPos);


        //        //conjuntoExistente.AddItem(part, assemblyPos);
        //        return;
        //    }

        //    //assy.GetSecondaries

        //    var novoConjunto = new ConjuntoMontagemDgt(part, new PecaDgt(assy.GetMainPart() as TSM.Part));
        //    _conjuntos.Add(novoConjunto);
        //}

        
    }
}
