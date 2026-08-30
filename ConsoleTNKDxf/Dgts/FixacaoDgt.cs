using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class FixacaoDgt
    {
        string _montagem;
        ParafusoDgt _parafuso;
        PorcaDgt _porca;
        ArruelaDgt _arruela;

        public ParafusoDgt Parafuso => _parafuso;
        public PorcaDgt Porca => _porca;
        public ArruelaDgt Arruela => _arruela;

        public string Montagem => _montagem;

        public FixacaoDgt(TSM.BoltArray boltArray)
        {
            _montagem = boltArray.BoltType.ToString();
            _parafuso = new ParafusoDgt(boltArray);

            incluirPorcasArruelas(boltArray);
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
        }

        private void processarArruela(BoltArray boltArray)
        {
            _arruela = new ArruelaDgt(boltArray);
        }
    }
}
