using System;
using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Peca : ILinhaLM
    {

        private int _quantidade;
        private DescricaoItem _descricao;
        private Material _material;
        private PesoItem _peso;
        Hashtable _doubleProperties = new Hashtable();
        Hashtable _stringProterties = new Hashtable();


        public string Posicao => _stringProterties["PART_POS"].ToString();
        public string Quantidade => _quantidade.ToString();   
        public string Descricao => _descricao;
        public string Observacao => _stringProterties["FINISH"].ToString();
        public string Material => _material;
        public string Peso => Math.Round(_peso.PesoTotal, 2).ToString();


        public double PesoCalculado => _peso.PesoCalculado;
        

        public Peca(TSM.Part pecaChild)
        {
            ArrayList doubleReportProperties = new ArrayList { "HEIGHT", "LENGTH", "WIDTH", "PROFILE.DIAMETER", "PROFILE.PLATE_THICKNESS" };
            ArrayList stringReportProperties = new ArrayList { "PART_POS", "FINISH" };
            pecaChild.GetDoubleReportProperties(doubleReportProperties, ref _doubleProperties);
            pecaChild.GetStringReportProperties(stringReportProperties, ref _stringProterties);
            _descricao = new DescricaoItem(pecaChild);
            _material = new Material(pecaChild);
            _peso = new PesoItem(pecaChild);
            _quantidade = 1;
        }

        public void IncrementarQuantidade()
        {
            _quantidade++;
        }

        public void IncrementarPeso()
        {
            _peso.IncrementarPeso();
        }
    }
}
