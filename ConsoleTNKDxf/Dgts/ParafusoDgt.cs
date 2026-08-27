using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class ParafusoDgt
    {
        private string _name;
        private string _nameShort;
        private string _profile;

        private double _weight;


        private int _quantidade;
        private string _montagem;
        private Peca _pecaChega;
        private Peca _pecaRecebe;

        public string Name => _name;
        public string NameShort => _nameShort;

        
        public string Profile => _profile;

      
        public double Weight => _weight;
        public int Quantidade => _quantidade;
        public Peca PecaChega => _pecaChega;
        public Peca PecaRecebe => _pecaRecebe;
        public string Montagem => _montagem;

        

        public ParafusoDgt(TSM.BoltArray boltArray, TSM.Part pecaChega)
        {

            //if (_montagem == "Workshop")// && _parafusosFabrica.Any(p => p.Name == parafusoExistente.Name))
            //{
            //    //ParafusoDgt parafusoExistente = _parafusosFabrica.FirstOrDefault(p => p.Name == parafuso.Name);
            //    IncrementarQuantidade();
            //    //_parafusosObra.Add(parafusoExistente);
            //}
            //else //if(_parafusosObra.Any(p => p.Name == parafusoExistente.Name))
            //{
            //    IncrementarQuantidade();
            //    //_parafusosFabrica.Add(parafusoExistente);
            //}

            _pecaChega = new Peca(boltArray.PartToBeBolted);

            

            _pecaRecebe = new Peca(boltArray.PartToBoltTo);

            

            //if(boltArray.OtherPartsToBolt.Count > 0)
            //{
            //    _pecasRecebem = boltArray.OtherPartsToBolt.Count > 0 ? boltArray.OtherPartsToBolt.Cast<TSM.Part>().ToList() : new List<TSM.Part>();
            //}

            definir(boltArray);
        }

        private void definir(TSM.BoltArray boltArray)
        {
            ArrayList stringReportProperties = new ArrayList { "NAME", "NAME_SHORT", "PROFILE", "SITE_WORKSHOP" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            _name = stringProperties.ContainsKey("NAME") ? stringProperties["NAME"]?.ToString() : string.Empty;
            _nameShort = stringProperties.ContainsKey("NAME_SHORT") ? stringProperties["NAME_SHORT"]?.ToString() : string.Empty;
            _profile = stringProperties.ContainsKey("PROFILE") ? stringProperties["PROFILE"]?.ToString() : string.Empty;
            _montagem = stringProperties.ContainsKey("SITE_WORKSHOP") ? stringProperties["SITE_WORKSHOP"]?.ToString() : string.Empty;

            _quantidade = boltArray.BoltPositions.Count;

            ArrayList doubleReportProperties = new ArrayList { "WEIGHT" };
            Hashtable doubleProperties = new Hashtable();
            boltArray.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _weight = doubleProperties.ContainsKey("WEIGHT") ? doubleProperties["WEIGHT"].ToString().ConverterParaDouble() : 0.0;
        }

        //public void IncrementarQuantidade()
        //{
        //    _quantidade++;
        //}
    }
}
