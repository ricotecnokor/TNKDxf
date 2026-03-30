using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Parafuso : ILinhaLM
    {
        private string _posicao;
        private int _quantidade;
        private DescricaoParafuso _descricao;
        private string _observacao;
        private string _material;
        private double _peso;
        private double _pesoUnitario;


        public string Posicao => _posicao;  
        public string Quantidade => _quantidade.ToString();
        public string Descricao => _descricao;
        public string Observacao => _observacao;
        public string Material => _material;
        public string Peso => _peso.ToString();
        public Parafuso(TSM.BoltArray boltArray)
        {
            string name = string.Empty;
            ArrayList stringReportProperties = new ArrayList { "NAME", "NAME_SHORT" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            _posicao = stringProperties["NAME"]?.ToString().Substring(0, 4);
            _quantidade = 1;
            _descricao = new DescricaoParafuso(boltArray);
            _observacao = "GALV.";
            _material = stringProperties["NAME_SHORT"].ToString();

            ArrayList doubleReportProperties = new ArrayList { "WEIGHT" };
            Hashtable doubleProperties = new Hashtable();
            boltArray.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _pesoUnitario = doubleProperties["WEIGHT"] != null ? Convert.ToDouble(doubleProperties["WEIGHT"]) : 0;
            _peso = _pesoUnitario;


        }

        public void IncrementarQuantidade()
        {
            _quantidade++;
        }

        public void IncrementarPeso()
        {
            _peso += _pesoUnitario;
        }   
    }
}
