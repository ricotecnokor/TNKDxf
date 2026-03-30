using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
  
    public class ElementosFixacaoDgt
    {
        private List<ParafusoDgt> _parafusos = new List<ParafusoDgt>();
        private List<PorcaDgt> _porcas = new List<PorcaDgt>();
        private List<ArruelaDgt> _arruelas = new List<ArruelaDgt>();
        private Model _model;

        public List<ParafusoDgt> Parafusos => _parafusos.ToList();
        public List<PorcaDgt> Porcas => _porcas.ToList();
        public List<ArruelaDgt> Arruelas => _arruelas.ToList();


        public ElementosFixacaoDgt(Model model, MultiDrawing multiDrawing)
        {
            _model = model;
            coletar(multiDrawing);
        }

        private void coletar(MultiDrawing multiDrawing)
        {
            HashSet<Identifier> parafsUnicosNoDesenho = obterPafusosUnicosDesenho(multiDrawing);

            foreach (Identifier modelId in parafsUnicosNoDesenho)
            {

                var modelObj = _model.SelectModelObject(modelId);

                if (modelObj is TSM.BoltArray modelBoltArray)
                {
                    addParafuso(modelBoltArray);
                }
            }
        }

        private HashSet<Identifier> obterPafusosUnicosDesenho(MultiDrawing multiDrawing)
        {
            HashSet<Identifier> parafusosUnicosDesenho = new HashSet<Identifier>();

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
                        parafusosUnicosDesenho.Add(drwBolt.ModelIdentifier);
                    }
                }
            }

            return parafusosUnicosDesenho;
        }

        private void addParafuso(TSM.BoltArray boltArray)
        {

            ArrayList stringReportProperties = new ArrayList { "SITE_WORKSHOP" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            string siteWorkshop = stringProperties["SITE_WORKSHOP"]?.ToString();

            //if (siteWorkshop == "Site")
            //return;

            processarParafuso(boltArray);

            if (boltArray.Nut1) processarPorca(boltArray);
            if (boltArray.Nut2) processarPorca(boltArray);


            if (boltArray.Washer1) processarArruela(boltArray);
            if (boltArray.Washer2) processarArruela(boltArray);
            if (boltArray.Washer3) processarArruela(boltArray);

        }

        private void processarParafuso(BoltArray boltArray)
        {
            var parafuso = new ParafusoDgt(boltArray);
            if (_parafusos.Any(p => p.Name == parafuso.Name))
            {
                ParafusoDgt parafusoExistente = _parafusos.FirstOrDefault(p => p.Name == parafuso.Name);
                parafusoExistente.IncrementarQuantidade();
                return;
            }
            _parafusos.Add(parafuso);
        }

        private void processarPorca(BoltArray boltArray)
        {
            var porca = new PorcaDgt(boltArray);

            if (_porcas.Any(p => p.NutName == porca.NutName))
            {
                PorcaDgt porcaExistente = _porcas.FirstOrDefault(p => p.NutName == porca.NutName);
                porcaExistente.IncrementarQuantidade();
                return;

            }
            _porcas.Add(porca);
        }


        private void processarArruela(BoltArray boltArray)
        {
            var arruela = new ArruelaDgt(boltArray);

            if (_arruelas.Any(p => p.WasherName == arruela.WasherName))
            {
                ArruelaDgt arruelaExistente = _arruelas.FirstOrDefault(p => p.WasherName == arruela.WasherName);
                arruelaExistente.IncrementarQuantidade();
                return;

            }
            _arruelas.Add(arruela);
        }


    }
}
