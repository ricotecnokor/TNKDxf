using ConsoleTNKDxf.Abstracoes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
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

        

        //TSM.Model _model;
        //private List<ConjuntoDetalhadoDgt> _conjuntos = new List<ConjuntoDetalhadoDgt>();
        //public List<ConjuntoDetalhadoDgt> Conjuntos => _conjuntos;
        //private string _prefixoConjunto;

        //public LmDetalhesDtg(TSM.Model model, string prefixoConjunto)
        //{
        //    _model = model;
        //    _prefixoConjunto = prefixoConjunto;
        //}

        public HashSet<Identifier> ParafusosUnicosNoDesenho { get; private set; } = new HashSet<Identifier>();

        public override void Coletar(Drawing multiDrawing)
        {
            obterObjetosDesenho(multiDrawing);
        }

        private void obterObjetosDesenho(Drawing multiDrawing)
        {
            HashSet<Identifier> pecasUnicasNoDesenho = new HashSet<Identifier>();
            ParafusosUnicosNoDesenho = new HashSet<Identifier>();

            var sheet = multiDrawing?.GetSheet();
            if (sheet == null) return;

            var allViews = sheet.GetAllViews();
            var listaViews = new List<TSD.View>();
            var viewEnum = allViews.GetEnumerator();
            while (viewEnum.MoveNext())
            {
                if (viewEnum.Current is TSD.View v)
                {
                    listaViews.Add(v);
                }
            }

            // Ordena vistas: principais primeiro (ModelView, FrontView, etc.), cortes/detalhes depois
            var vistasOrdenadas = listaViews
                .OrderBy(v => ehCorteOuDetalhe(v) ? 1 : 0)
                .ToList();

            bool todasVistasPrincipaisProcessadas = false;

            foreach (var view in vistasOrdenadas)
            {
                bool ehCorte = ehCorteOuDetalhe(view);
                if (ehCorte && !todasVistasPrincipaisProcessadas)
                {
                    todasVistasPrincipaisProcessadas = true;
                }

                // Se já passamos pelas vistas principais e todos os conjuntos encontrados estão 100% completos:
                if (todasVistasPrincipaisProcessadas && conjuntosEstaoCompletos(ParafusosUnicosNoDesenho))
                {
                    // Cortes e detalhes redundantes podem ser ignorados com segurança sem perda de peças ou parafusos
                    break;
                }

                DrawingObjectEnumerator drwObjs = view.GetObjects(new[] { typeof(TSD.Part), typeof(TSD.Bolt) });
                while (drwObjs.MoveNext())
                {
                    var cur = drwObjs.Current;
                    if (cur is TSD.Part drwPart)
                    {
                        if (pecasUnicasNoDesenho.Add(drwPart.ModelIdentifier))
                        {
                            var modelObj = _model.SelectModelObject(drwPart.ModelIdentifier);
                            if (modelObj is TSM.Part modelPart)
                            {
                                addPeca(modelPart);
                            }
                        }
                    }
                    else if (cur is TSD.Bolt drwBolt)
                    {
                        ParafusosUnicosNoDesenho.Add(drwBolt.ModelIdentifier);
                    }
                }
            }
        }

        private bool ehCorteOuDetalhe(TSD.View view)
        {
            try
            {
                string tipo = view.ViewType.ToString();
                return tipo == "SectionView" || tipo == "DetailView";
            }
            catch
            {
                return false;
            }
        }

        private bool conjuntosEstaoCompletos(HashSet<Identifier> parafusosColetados)
        {
            if (_conjuntos.Count == 0) return false;
            foreach (var conjunto in _conjuntos)
            {
                if (conjunto.TotalPartesNoAssembly > 0 && conjunto.TotalPartesColetadas < conjunto.TotalPartesNoAssembly)
                {
                    return false;
                }
                if (conjunto.TotalParafusosNoAssembly > 0 && parafusosColetados != null)
                {
                    if (!conjunto.BoltIdsNoAssembly.IsSubsetOf(parafusosColetados))
                    {
                        return false;
                    }
                }
            }
            return true;
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



            var partPos = part.ObterPropriedade("PART_POS").ToString();


            //if (assemblyPos == "00724AK")
            //{


            //    PecaDgt pecaTeste = new PecaDgt(part);

  
            //}




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
            var marcaConjunto = fixacao.Parafuso.PecaChega?.MarcaMontagem;
            if (string.IsNullOrEmpty(marcaConjunto) || !_conjuntos.Any(c => c.AssemblyPos == marcaConjunto))
                return;

            ConjuntoDetalhadoDgt conjunto = (ConjuntoDetalhadoDgt)_conjuntos.FirstOrDefault(c => c.AssemblyPos == marcaConjunto);

            List<FixacaoDgt> fixacaoList = tipoFixacao == "BOLT_TYPE_WORKSHOP" ? conjunto.FixacaoFabrica : conjunto.FixacaoObra;
            if (!fixacaoList.Contains(fixacao))
            {
                //fixacao.IncrementarQuantidadeLigacoes();
                fixacaoList.Add(fixacao);
                return;
            }
            else
            {
                var fixacaoExistente = fixacaoList.FirstOrDefault(f => f.Equals(fixacao));
                //fixacaoExistente.IncrementarQuantidadeLigacoes();
            }
            return;
        }
    }
}
