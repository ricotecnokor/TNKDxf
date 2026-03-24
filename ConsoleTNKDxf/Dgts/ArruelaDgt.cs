using System;
using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class ArruelaDgt
    {
        private string _whasherName;
        private string _boltStandard;
        private double _washerWeight;
        private int _quantidade;

        public string WasherName => _whasherName;
        public string BoltStandard => _boltStandard;
        public string Quantidade => _quantidade.ToString();
        public string WasherWeight => Math.Round(_washerWeight, 2).ToString();

        public ArruelaDgt(TSM.BoltArray boltArray)
        {
            string name = string.Empty;
            ArrayList stringReportProperties = new ArrayList { "WASHER.NAME", "BOLT_STANDARD" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            _whasherName = stringProperties.ContainsKey("WASHER.NAME") ? stringProperties["WASHER.NAME"]?.ToString() : string.Empty;
            _boltStandard = stringProperties.ContainsKey("BOLT_STANDARD") ? stringProperties["BOLT_STANDARD"]?.ToString() : string.Empty;
            _quantidade = 1;

            ArrayList doubleReportProperties = new ArrayList { "WASHER.WEIGHT" };
            Hashtable doubleProperties = new Hashtable();
            boltArray.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _washerWeight = doubleProperties.ContainsKey("WASHER.WEIGHT") ? Convert.ToDouble(doubleProperties["WASHER.WEIGHT"]) : 0;

        }

        public void IncrementarQuantidade()
        {
            _quantidade++;
        }
    }
}
