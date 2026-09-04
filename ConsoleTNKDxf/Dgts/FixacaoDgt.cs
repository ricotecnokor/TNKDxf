using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class FixacaoDgt
    {
        int _quantidadeLigacoes = 0;
        string _montagem;
        int _numeroParafusos = 0;
        int _numeroPorcas = 0;
        int _numeroArruelas = 0;
        ParafusoDgt _parafuso;
        PorcaDgt _porca;
        ArruelaDgt _arruela;

        public ParafusoDgt Parafuso => _parafuso;
        public PorcaDgt Porca => _porca;
        public ArruelaDgt Arruela => _arruela;

        public string Montagem => _montagem;

        public void IncrementarQuantidadeLigacoes()
        {
            _quantidadeLigacoes++;
        }


        public FixacaoDgt(TSM.BoltArray boltArray)
        {
            _montagem = boltArray.BoltType.ToString();
            _numeroParafusos = boltArray.BoltPositions.Count;
            _parafuso = new ParafusoDgt(boltArray, boltArray.PartToBeBolted);



            //var listaPecasRecebem = boltArray.OtherPartsToBolt.Count > 0 ? boltArray.OtherPartsToBolt.Cast<TSM.Part>().ToList() : new List<TSM.Part>();

            //if (listaPecasRecebem.Count == 0 && boltArray.PartToBeBolted != null)
            //{

                incluirPorcasArruelas(boltArray);
                //processarParafuso(parafuso);
            //}
            //else
            //{
            //    foreach (var pecaRecebe in listaPecasRecebem)
            //    {

                    //processarParafuso(parafuso);
                    //incluirPorcasArruelas(boltArray);
            //    }
            //}

            _numeroArruelas = _numeroArruelas * _numeroParafusos;
            _numeroPorcas = _numeroPorcas * _numeroParafusos;
        }

        private void incluirPorcasArruelas(BoltArray boltArray)
        {
            if (boltArray.Nut1) processarPorca(boltArray);
            if (boltArray.Nut2) processarPorca(boltArray);

            if (boltArray.Washer1) processarArruela(boltArray);
            if (boltArray.Washer2) processarArruela(boltArray);
            if (boltArray.Washer3) processarArruela(boltArray);
        }

        private void processarPorca(BoltArray boltArray)
        {
            _porca = new PorcaDgt(boltArray);

            _numeroPorcas = _numeroPorcas + 1;

            //if (_porcasFabrica.Any(p => p.NutName == porca.NutName))
            //{
            //    PorcaDgt porcaExistente = _porcasFabrica.FirstOrDefault(p => p.NutName == porca.NutName);
            //    porcaExistente.IncrementarQuantidade();
            //    return;

            //}
            //_porcasFabrica.Add(porca);
        }


        private void processarArruela(BoltArray boltArray)
        {
            _arruela = new ArruelaDgt(boltArray);

            _numeroArruelas = _numeroArruelas + 1;

            //if (_arruelasFabrica.Any(p => p.WasherName == arruela.WasherName))
            //{
            //    ArruelaDgt arruelaExistente = _arruelasFabrica.FirstOrDefault(p => p.WasherName == arruela.WasherName);
            //    arruelaExistente.IncrementarQuantidade();
            //    return;

            //}
            //_arruelasFabrica.Add(arruela);
        }
    }
}
