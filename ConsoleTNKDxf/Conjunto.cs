using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Conjunto : ILinhaLM
    {
        private List<Peca> _itens = new List<Peca>();
        private string _posicao;
        private int _quantidade;
        private double _pesoTotal;
        private string _descricao;

        public string Posicao => _posicao;
        public string Quantidade => _quantidade.ToString();
        public string Descricao => _descricao;
        public string Observacao => string.Empty;   
        public string Material => string.Empty;
        public string Peso => PesoTotal.ToString();
        public double PesoTotal => Math.Round(_pesoTotal, 2);

        public List<ILinhaLM> Pecas => _itens.Cast<ILinhaLM>().ToList();

        public Conjunto(TSM.Part part)
        {
            var assy = part.GetAssembly();

            Hashtable doubleProperties = new Hashtable();
            Hashtable stringProterties = new Hashtable();

            ArrayList doubleReportProperties = new ArrayList { "HEIGHT", "WEIGTH" };
            ArrayList stringReportProperties = new ArrayList { "ASSEMBLY_POS", "MAINPART.NAME" };
           
            assy.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            assy.GetStringReportProperties(stringReportProperties, ref stringProterties);

            _descricao = stringProterties.ContainsKey("MAINPART.NAME") ? stringProterties["MAINPART.NAME"].ToString() : string.Empty;
            _posicao = stringProterties.ContainsKey("ASSEMBLY_POS") ? stringProterties["ASSEMBLY_POS"].ToString() : string.Empty;
            _pesoTotal = doubleProperties.ContainsKey("WEIGTH") && double.TryParse(doubleProperties["WEIGTH"].ToString(), out double result) ? (int)result : 0;

           
            Peca peca = new Peca(part);
            _quantidade = 1;
            _itens.Add(peca);
            //var pecasSecundarias = assy.GetSecondaries().GetEnumerator();
            //while (pecasSecundarias.MoveNext())
            //{
            //    var child = pecasSecundarias.Current;
            //    if (child is TSM.Part pecaChild)
            //    {
            //        Peca item = new Peca(pecaChild);
            //        _itens.Add(item);
            //    }
            //}
           
            _pesoTotal = peca.PesoCalculado;
        }




       

     

        public void AddItem(TSM.Part part)
        {
            string posicaoPeca = part.ObterPropriedade("PART_POS").ToString();

            if (_itens.Any(c => c.Posicao == posicaoPeca))
            {
                var existingItem = _itens.First(c => c.Posicao == posicaoPeca);
                existingItem.IncrementarQuantidade();
                existingItem.IncrementarPeso();
                _pesoTotal += existingItem.PesoCalculado;
                return;
            }

            Peca item = new Peca(part);
            _itens.Add(item);
            _pesoTotal += item.PesoCalculado;
        }

        public void AddItemExistente(string posicaoPeca)
        {
            var existingItem = _itens.FirstOrDefault(c => c.Posicao == posicaoPeca);
            if (existingItem != null)
            {
                existingItem.IncrementarQuantidade();
                existingItem.IncrementarPeso();
            }
        }
    }
}
