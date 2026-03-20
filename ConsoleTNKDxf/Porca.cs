using System;
using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Porca : ILinhaLM
    {

        private string _posicao;
        private int _quantidade;
        private DescricaoPorca _descricao;
        private string _observacao;
        private MaterialPorca _material;
        private double _peso;
        private double _pesoUnitario;

        public string Posicao => _posicao;
        public string Quantidade => _quantidade.ToString();
        public string Descricao => _descricao;
        public string Observacao => _observacao;
        public string Material => _material;
        public string Peso => Math.Round(_peso, 2).ToString();

        public Porca(TSM.BoltArray boltArray)
        {
            //string nomePorca = "";
            //boltArray.GetReportProperty("NUT_TYPE", ref nomePorca);

            string name = string.Empty;
            ArrayList stringReportProperties = new ArrayList { "NUT.NAME" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            _posicao = stringProperties["NUT.NAME"]?.ToString().Substring(0, 4);
            _quantidade = 1;
            _descricao = new DescricaoPorca(boltArray);
            _observacao = "GALV.";

            _material = new MaterialPorca(boltArray);

            ArrayList doubleReportProperties = new ArrayList { "NUT.WEIGHT" };
            Hashtable doubleProperties = new Hashtable();
            boltArray.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _pesoUnitario = doubleProperties["NUT.WEIGHT"] != null ? Convert.ToDouble(doubleProperties["NUT.WEIGHT"]) : 0;
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
