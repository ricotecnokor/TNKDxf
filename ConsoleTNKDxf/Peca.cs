using System;
using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Peca : ILinhaLM
    {
        string _marcaMontagem;
        private string _posicao;
        private int _quantidade;
        private string _observacao;
        private DescricaoItem _descricao;
        private Material _material;
        private PesoItem _peso;
        Hashtable _doubleProperties = new Hashtable();
        Hashtable _stringProterties = new Hashtable();


        public string Posicao => _posicao;
        public string Quantidade => _quantidade.ToString();   
        public string Descricao => _descricao;
        public string Observacao => _observacao;
        public string Material => _material;
        public string Peso => Math.Round(_peso.PesoTotal, 2).ToString();
        public string MarcaMontagem => _marcaMontagem;

        public double PesoCalculado => _peso.PesoCalculado;
        

        public Peca(TSM.Part pecaChild)
        {
            
            ArrayList doubleReportProperties = new ArrayList { "HEIGHT", "LENGTH", "WIDTH", "PROFILE.DIAMETER", "PROFILE.PLATE_THICKNESS" };
            ArrayList stringReportProperties = new ArrayList { "ASSEMBLY_POS", "PART_POS", "FINISH" };
            pecaChild.GetDoubleReportProperties(doubleReportProperties, ref _doubleProperties);
            pecaChild.GetStringReportProperties(stringReportProperties, ref _stringProterties);
            _posicao = _stringProterties["PART_POS"]?.ToString();
            _marcaMontagem = _stringProterties["ASSEMBLY_POS"]?.ToString();
            _observacao = _stringProterties["FINISH"]?.ToString();
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
