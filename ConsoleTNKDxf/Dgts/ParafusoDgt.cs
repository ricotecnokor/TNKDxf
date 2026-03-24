using System.Collections;
using System.Globalization;
using static Tekla.Structures.Filtering.Categories.BoltFilterExpressions;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class ParafusoDgt
    {
        private string _name;
        private string _nameShort;
        private string _profile;
        private bool _isSiteWorkshop;

        private double _weight;


        private int _quantidade;

        public string Name => _name;
        public string NameShort => _nameShort;
        public string Profile => _profile;

      
        public double Weight => _weight;
        public int Quantidade => _quantidade;

     

        public ParafusoDgt(TSM.BoltArray boltArray)
        {
            ArrayList stringReportProperties = new ArrayList { "NAME", "NAME_SHORT", "PROFILE", "SITE_WORKSHOP" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            _name = stringProperties.ContainsKey("NAME") ?  stringProperties["NAME"]?.ToString() : string.Empty;
            _nameShort = stringProperties.ContainsKey("NAME_SHORT") ? stringProperties["NAME_SHORT"]?.ToString() : string.Empty;
            _profile = stringProperties.ContainsKey("PROFILE") ? stringProperties["PROFILE"]?.ToString() : string.Empty;
   
            _quantidade = 1;

            ArrayList doubleReportProperties = new ArrayList { "WEIGHT" };
            Hashtable doubleProperties = new Hashtable();
            boltArray.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _weight = doubleProperties.ContainsKey("WEIGHT") ? double.Parse(doubleProperties["WEIGHT"].ToString(),, CultureInfo.InvariantCulture) : 0.0;
        }

        public void IncrementarQuantidade()
        {
            _quantidade++;
        }
    }
}
