using System.Collections.Generic;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class ElementosFixacaoDgt
    {
        LmDetalhesDtg _lm;
        private List<FixacaoDgt> _fixacaoFabrica = new List<FixacaoDgt>();
        private List<FixacaoDgt> _fixacaoObra = new List<FixacaoDgt>();
        private Model _model;

        public List<FixacaoDgt> FixacaoObra => _fixacaoObra;
        public List<FixacaoDgt> FixacaoFabrica => _fixacaoFabrica;

        public ElementosFixacaoDgt(Model model)
        {
            _model = model;
        }

        public void Coletar(Drawing multiDrawing, List<string> prefixosConjunto, LmDetalhesDtg lm)
        {
            _lm = lm;

            HashSet<Identifier> boltArraysUnicosNoDesenho = obterBoltArraysUnicosDesenho(multiDrawing);

            foreach (Identifier modelId in boltArraysUnicosNoDesenho)
            {
                var modelObj = _model.SelectModelObject(modelId);

                if (modelObj is TSM.BoltArray modelBoltArray)
                {
                    addFixacoes(modelBoltArray);
                }
            }
        }

        public void Coletar(Drawing multiDrawing, List<string> prefixosConjunto, LmMontagemDgt conjuntos)
        {
            HashSet<Identifier> parafsUnicosNoDesenho = obterBoltArraysUnicosDesenho(multiDrawing);

            foreach (Identifier modelId in parafsUnicosNoDesenho)
            {
                var modelObj = _model.SelectModelObject(modelId);

                if (modelObj is TSM.BoltArray modelBoltArray)
                {
                    addParafuso(modelBoltArray);
                }
            }
        }

        private HashSet<Identifier> obterBoltArraysUnicosDesenho(Drawing multiDrawing)
        {
            HashSet<Identifier> boltArraysUnicosDesenho = new HashSet<Identifier>();

            var views = multiDrawing.GetSheet().GetAllViews().GetEnumerator();
            while (views.MoveNext())
            {
                var view = views.Current as TSD.View;
                if (view == null) continue;

                DrawingObjectEnumerator drawingBolts = view.GetObjects(new[] { typeof(TSD.Bolt) });
                while (drawingBolts.MoveNext())
                {
                    TSD.Bolt drwBolt = drawingBolts.Current as TSD.Bolt;

                    if (drwBolt != null)
                    {
                        boltArraysUnicosDesenho.Add(drwBolt.ModelIdentifier);
                    }
                }
            }

            return boltArraysUnicosDesenho;
        }

        private void addFixacoes(TSM.BoltArray boltArray)
        {
            var fixacao = new FixacaoDgt(boltArray);

            if (!_lm.ContemPeca(fixacao.Parafuso.PecaChega.Posicao))
            {
                return;
            }

            if (fixacao.Montagem == "BOLT_TYPE_WORKSHOP")
            {
                _lm.Ligar(fixacao, "BOLT_TYPE_WORKSHOP");
                return;
            }

            if (fixacao.Montagem == "BOLT_TYPE_SITE")
            {
                _lm.Ligar(fixacao, "BOLT_TYPE_SITE");
                return;
            }
        }

        private void addParafuso(TSM.BoltArray boltArray)
        {
            var fixacao = new FixacaoDgt(boltArray);

            if (fixacao.Montagem == "BOLT_TYPE_WORKSHOP")
            {
                _fixacaoFabrica.Add(fixacao);
            }
            else
            {
                _fixacaoObra.Add(fixacao);
            }
        }
    }
}
