using System;
using System.Collections;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Arruela : ILinhaLM
    {
        private string _posicao;
        private int _quantidade;
        private DescricaoArruela _descricao;
        private string _observacao;
        private MaterialArruela _material;
        private double _peso;
        private double _pesoUnitario;

        public string Posicao => _posicao;
        public string Quantidade => _quantidade.ToString();
        public string Descricao => _descricao;
        public string Observacao => _observacao;
        public string Material => _material;
        public string Peso => Math.Round(_peso, 2).ToString();

        public Arruela(BoltArray boltArray)
        {
            string name = string.Empty;
            ArrayList stringReportProperties = new ArrayList { "WASHER.NAME" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            _posicao = stringProperties["WASHER.NAME"]?.ToString().Substring(0, 4);
            _quantidade = 1;
            _descricao = new DescricaoArruela(boltArray);
            _observacao = "GALV.";

            _material = new MaterialArruela(boltArray);

            ArrayList doubleReportProperties = new ArrayList { "WASHER.WEIGHT" };
            Hashtable doubleProperties = new Hashtable();
            boltArray.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _pesoUnitario = doubleProperties["WASHER.WEIGHT"] != null ? Convert.ToDouble(doubleProperties["WASHER.WEIGHT"]) : 0;
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
