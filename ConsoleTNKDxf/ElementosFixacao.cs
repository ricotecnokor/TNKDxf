using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class ElementosFixacao 
    {
        private List<Parafuso> _parafusos = new List<Parafuso>();
        private List<Porca> _porcas = new List<Porca>();
        private List<Arruela> _arruelas = new List<Arruela>();
        private Model _model;
        
        public List<ILinhaLM> Parafusos => _parafusos.Cast<ILinhaLM>().ToList();
        public List<ILinhaLM> Porcas => _porcas.Cast<ILinhaLM>().ToList();
        public List<ILinhaLM> Arruelas => _arruelas.Cast<ILinhaLM>().ToList();


        public ElementosFixacao(Model model, MultiDrawing multiDrawing)
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

                // 2. Pegar todas as partes gráficas nesta vista
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
            var parafuso = new Parafuso(boltArray);
            if (_parafusos.Any(p => p.Posicao == parafuso.Posicao))
            {
                Parafuso parafusoExistente = _parafusos.FirstOrDefault(p => p.Posicao == parafuso.Posicao);
                parafusoExistente.IncrementarQuantidade();
                parafusoExistente.IncrementarPeso();
                return;
            }
            _parafusos.Add(parafuso);
        }

        private void processarPorca(BoltArray boltArray)
        {
            var porca = new Porca(boltArray);

            if (_porcas.Any(p => p.Posicao == porca.Posicao))
            {
                Porca porcaExistente = _porcas.FirstOrDefault(p => p.Posicao == porca.Posicao);
                porcaExistente.IncrementarQuantidade();
                porcaExistente.IncrementarPeso();
                return;

            }
            _porcas.Add(porca);
        }


        private void processarArruela(BoltArray boltArray)
        {
            var arruela = new Arruela(boltArray);

            if (_arruelas.Any(p => p.Posicao == arruela.Posicao))
            {
                Arruela arruelaExistente = _arruelas.FirstOrDefault(p => p.Posicao == arruela.Posicao);
                arruelaExistente.IncrementarQuantidade();
                arruelaExistente.IncrementarPeso();
                return;

            }
            _arruelas.Add(arruela);
        }

       
        public void AddParafuso(Parafuso fixacao)
        {
            _parafusos.Add(fixacao);
        }
        

    }

    


}
