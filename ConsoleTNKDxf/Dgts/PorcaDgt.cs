using System;
using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class PorcaDgt
    {
        private string _nutName;
        private string _boltStandard;
        private int _quantidade;
        private double _nutWeight;

        public string NutName => _nutName;
        public string BoltStandard => _boltStandard;
        public string Quantidade => _quantidade.ToString();
        public string NutWeight => Math.Round(_nutWeight, 2).ToString();

        public PorcaDgt(TSM.BoltArray boltArray)
        {

            string name = string.Empty;
            ArrayList stringReportProperties = new ArrayList { "NUT.NAME", "BOLT_STANDARD" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            _nutName = stringProperties.ContainsKey("NUT.NAME") ? stringProperties["NUT.NAME"].ToString() : string.Empty;
            _boltStandard = stringProperties.ContainsKey("BOLT_STANDARD") ? stringProperties["BOLT_STANDARD"].ToString() : string.Empty;

            _quantidade = 1;


            ArrayList doubleReportProperties = new ArrayList { "NUT.WEIGHT" };
            Hashtable doubleProperties = new Hashtable();
            boltArray.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _nutWeight = doubleProperties.ContainsKey("NUT.WEIGHT") ? Convert.ToDouble(doubleProperties["NUT.WEIGHT"]) : 0.0;
        }

        public void IncrementarQuantidade()
        {
            _quantidade++;
        }
    }
}
