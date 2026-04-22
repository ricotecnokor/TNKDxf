using ConsoleTNKDxf.Abstracoes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class ConjuntoMontagemDgt : ConjuntoAbstrato
    {

        private List<PecaDgt> _itens = new List<PecaDgt>();

        public ConjuntoMontagemDgt(TSM.Part part, PecaDgt pecaDgtMaiPart)
        {
            var assy = part.GetAssembly();

            Hashtable stringProterties = new Hashtable();
            ArrayList stringReportProperties = new ArrayList { "ASSEMBLY_POS", "MAINPART.NAME" };
            assy.GetStringReportProperties(stringReportProperties, ref stringProterties);
            _mainPartName = stringProterties.ContainsKey("MAINPART.NAME") ? stringProterties["MAINPART.NAME"].ToString() : string.Empty;
            _assemblyPos = stringProterties.ContainsKey("ASSEMBLY_POS") ? stringProterties["ASSEMBLY_POS"].ToString() : string.Empty;

            Hashtable doubleProperties = new Hashtable();
            ArrayList doubleReportProperties = new ArrayList { "HEIGHT", "WEIGTH" };
            assy.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _height = doubleProperties.ContainsKey("HEIGHT") ? doubleProperties["HEIGHT"].ToString().ConverterParaDouble() : 0;
            _weigth = doubleProperties.ContainsKey("WEIGTH") ? doubleProperties["WEIGTH"].ToString().ConverterParaDouble() : 0;
            Hashtable integerProperties = new Hashtable();
            ArrayList integerReportProperties = new ArrayList { "NUMBER", "ASSEMBLY.MODEL_TOTAL" };
            assy.GetIntegerReportProperties(integerReportProperties, ref integerProperties);
            int number = integerProperties.ContainsKey("NUMBER") ? int.Parse(integerProperties["NUMBER"].ToString()) : 1;
            int modelTotal = integerProperties.ContainsKey("ASSEMBLY.MODEL_TOTAL") ? int.Parse(integerProperties["ASSEMBLY.MODEL_TOTAL"].ToString()) : 1;
            int qtdModel = 0;
            part.GetReportProperty("ASSEMBLY.MODEL_TOTAL", ref qtdModel);



            _quantidade = number * qtdModel;




            PecaDgt peca = new PecaDgt(part);

            _itens.Add(peca);
        }

        public void AddItem(TSM.Part part, string assemblyPos)
        {
            string posicaoPeca = part.ObterPropriedade("PART_POS").ToString();

            if (_itens.Any(c => c.PartPos == posicaoPeca))
            {
                var existingItem = _itens.First(c => c.PartPos == posicaoPeca);
                existingItem.IncrementarQuantidade();

                //if (existingItem.PartPos == _pecaDgtMaiPart.PartPos)
                //{
                //    _quantidade++;
                //}

                return;
            }

            PecaDgt item = new PecaDgt(part);
            _itens.Add(item);
        }

        public void AddPeso(double weightNet)
        {
            _weigth += weightNet;
        }
    }
}