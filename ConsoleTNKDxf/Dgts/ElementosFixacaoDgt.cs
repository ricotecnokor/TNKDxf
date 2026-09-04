using ConsoleTNKDxf.Abstracoes;
using System;
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
        //Dictionary<string, int> _pecasValidacao = new Dictionary<string, int>();

        LmDetalhesDtg _lm;
        private List<FixacaoDgt> _fixacaoFabrica = new List<FixacaoDgt>();
        private List<FixacaoDgt> _fixacaoObra  = new List<FixacaoDgt>();
        //private List<PorcaDgt> _porcasFabrica = new List<PorcaDgt>();
        //private List<PorcaDgt> _porcasObra = new List<PorcaDgt>();
        //private List<ArruelaDgt> _arruelasFabrica = new List<ArruelaDgt>();
        //private List<ArruelaDgt> _arruelasObra = new List<ArruelaDgt>();
        private Model _model;
        //private List<string> _prefixosConjunto;
        public List<FixacaoDgt> FixacaoObra => _fixacaoObra;
        public List<FixacaoDgt> FixacaoFabrica => _fixacaoFabrica;
        //public List<PorcaDgt> PorcasObra => _porcasObra;
        //public List<PorcaDgt> PorcasFabrica => _porcasFabrica;
        //public List<ArruelaDgt> ArruelasObra => _arruelasObra;
        //public List<ArruelaDgt> ArruelasFabrica => _arruelasFabrica;


        public ElementosFixacaoDgt(Model model)
        {
            _model = model;
            //_prefixosConjunto = prefixosConjunto;
            //coletar(multiDrawing);
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
                    addParafuso(modelBoltArray, prefixosConjunto);
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
            bool temPecaAparafusar = false;


            var fixacao = new FixacaoDgt(boltArray);

            if(!_lm.ContemPeca(fixacao.Parafuso.PecaChega.Posicao))//_pecasValidacao.ContainsKey(fixacao.Parafuso.PecaChega.Posicao))
            {
                return;
            }

            if(fixacao.Montagem == "BOLT_TYPE_WORKSHOP")
            {
                _lm.Ligar(fixacao, "BOLT_TYPE_WORKSHOP");
                return;
            }


            if (fixacao.Montagem == "BOLT_TYPE_SITE")
            {
                _lm.Ligar(fixacao, "BOLT_TYPE_SITE");
                return;
            }






            //if (!deveAparafusar(fixacao, prefixosConjunto))
            //{
            //    return;
            //}




            //ArrayList stringReportProperties = new ArrayList { "SITE_WORKSHOP" };
            //Hashtable stringProperties = new Hashtable();
            //boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            //string siteWorkshop = stringProperties["SITE_WORKSHOP"]?.ToString();

            //if (siteWorkshop == "Site")
            //return;

            //int numeroParafusosBoltArray = fixacaoDgt.BoltPositions.Count;

            //if (numeroParafusosBoltArray > 1)
            //{
            //    bool oi = false;
            //}

            //var fixacao = new FixacaoDgt(fixacaoDgt);
            //var listaPecasRecebem = boltArray.OtherPartsToBolt.Count > 0 ? boltArray.OtherPartsToBolt.Cast<TSM.Part>().ToList() : new List<TSM.Part>();

            //if (listaPecasRecebem.Count == 0 && boltArray.PartToBeBolted != null)
            //{

            //    incluirPorcasArruelas(boltArray, fixacao);
            //    processarParafuso(fixacao);
            //}
            //else
            //{
            //    foreach (var pecaRecebe in listaPecasRecebem)
            //    {

            //        processarParafuso(fixacao);
            //        incluirPorcasArruelas(boltArray, fixacao);
            //    }
            //}

            //if (fixacao.Montagem == "BOLT_TYPE_WORKSHOP")
            //{
            //    _fixacaoFabrica.Add(fixacao);
            //}
            //else
            //{
            //    _fixacaoObra.Add(fixacao);
            //}



        }

        private void ligar(FixacaoDgt fixacao, List<FixacaoDgt> fixacaoList)
        {
            if (!fixacaoList.Contains(fixacao))
            {
                fixacao.IncrementarQuantidadeLigacoes();
                fixacaoList.Add(fixacao);
                return;
            }
            else
            {
                var fixacaoExistente = fixacaoList.FirstOrDefault(f => f.Equals(fixacao));
                fixacaoExistente.IncrementarQuantidadeLigacoes();
            }
            return;
        }

        private void addParafuso(TSM.BoltArray boltArray, List<string> prefixosConjunto)
        {
            bool temPecaAparafusar = false;

            

            //if (!deveAparafusar(boltArray, prefixosConjunto))
            //{
            //    return;
            //}


            //ArrayList stringReportProperties = new ArrayList { "SITE_WORKSHOP" };
            //Hashtable stringProperties = new Hashtable();
            //boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            //string siteWorkshop = stringProperties["SITE_WORKSHOP"]?.ToString();

            //if (siteWorkshop == "Site")
            //return;

            int numeroParafusosBoltArray = boltArray.BoltPositions.Count;

            //if (numeroParafusosBoltArray > 1)
            //{
            //    bool oi = false;
            //}

            var fixacao = new FixacaoDgt(boltArray);
            //var listaPecasRecebem = boltArray.OtherPartsToBolt.Count > 0 ? boltArray.OtherPartsToBolt.Cast<TSM.Part>().ToList() : new List<TSM.Part>();

            //if (listaPecasRecebem.Count == 0 && boltArray.PartToBeBolted != null)
            //{
                
            //    incluirPorcasArruelas(boltArray, fixacao);
            //    processarParafuso(fixacao);
            //}
            //else
            //{
            //    foreach (var pecaRecebe in listaPecasRecebem)
            //    {
                    
            //        processarParafuso(fixacao);
            //        incluirPorcasArruelas(boltArray, fixacao);
            //    }
            //}

            if(fixacao.Montagem == "BOLT_TYPE_WORKSHOP")
            {
                _fixacaoFabrica.Add(fixacao);
            }
            else
            {
                _fixacaoObra.Add(fixacao);
            }



        }

        

        //private bool deveAparafusar(FixacaoDgt fixacao, List<string> prefixosConjunto)
        //{
        //    var pfsAparafusar = fixacao.OtherPartsToBolt.GetEnumerator();
        //    bool temPecaAparafusar = false;
        //    var prefixoPecaAparfusar = fixacao.PartToBeBolted.PartNumber.Prefix;
        //    if (contemUmDosPrefixos(prefixoPecaAparfusar, prefixosConjunto))
        //    {
        //        temPecaAparafusar = true;
        //    }
        //    else
        //    {
        //        while (pfsAparafusar.MoveNext())
        //        {
        //            var pecaAparafusar = pfsAparafusar.Current as TSM.Part;
        //            if (pecaAparafusar != null)
        //            {
        //                if (contemUmDosPrefixos(pecaAparafusar.PartNumber.Prefix, prefixosConjunto))
        //                {
        //                    temPecaAparafusar = true;
        //                    break;
        //                }


        //            }
        //        }
        //    }

        //    return temPecaAparafusar;
        //}

        private bool contemUmDosPrefixos(string prefixoPecaAparfusar, List<string> prefixosConjunto)
        {
            return prefixosConjunto.Any(prefixo => prefixoPecaAparfusar.Contains(prefixo));
        }

        //private void processarParafuso(ParafusoDgt parafuso)
        //{
        //    if (parafuso.Montagem == "Workshop")// && _parafusosFabrica.Any(p => p.Name == parafusoExistente.Name))
        //    {
        //        //ParafusoDgt parafusoExistente = _parafusosFabrica.FirstOrDefault(p => p.Name == parafuso.Name);
        //        _parafusosObra.Add(parafuso);
        //    }
        //    else //if(_parafusosObra.Any(p => p.Name == parafusoExistente.Name))
        //    {

        //        _parafusosFabrica.Add(parafuso);
        //    }


        //}

       


    }
}
