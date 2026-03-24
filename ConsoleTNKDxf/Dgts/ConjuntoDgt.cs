using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class ConjuntoDgt
    {
        private List<PecaDgt> _itens = new List<PecaDgt>();
        private double _height;
        private double _weigth;
        private string _assemblyPos;
        private string _mainPartName;
        private int _quantidade;

        public List<PecaDgt> Itens => _itens;

        public string AssemblyPos => _assemblyPos;
        public string MainPartName => _mainPartName;
        public int Quantidade => _quantidade;
        public double Height => _height;
        public double Weigth => _weigth;

        private readonly PecaDgt _pecaDgtMaiPart;


        public ConjuntoDgt(TSM.Part part, PecaDgt pecaDgtMaiPart)
        {
            var assy = part.GetAssembly();
            _pecaDgtMaiPart = pecaDgtMaiPart;

            Hashtable stringProterties = new Hashtable();
            ArrayList stringReportProperties = new ArrayList { "ASSEMBLY_POS", "MAINPART.NAME" };
            assy.GetStringReportProperties(stringReportProperties, ref stringProterties);
            _mainPartName = stringProterties.ContainsKey("MAINPART.NAME") ? stringProterties["MAINPART.NAME"].ToString() : string.Empty;
            _assemblyPos = stringProterties.ContainsKey("ASSEMBLY_POS") ? stringProterties["ASSEMBLY_POS"].ToString() : string.Empty;

            Hashtable doubleProperties = new Hashtable();
            ArrayList doubleReportProperties = new ArrayList { "HEIGHT", "WEIGTH" };
            assy.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _height = doubleProperties.ContainsKey("HEIGHT") ? double.Parse(doubleProperties["HEIGHT"].ToString()) : 0;
            _weigth = doubleProperties.ContainsKey("WEIGTH") ? double.Parse(doubleProperties["WEIGTH"].ToString()) : 0;

            Hashtable integerProperties = new Hashtable();
            ArrayList integerReportProperties = new ArrayList { "NUMBER", "ASSEMBLY.MODEL_TOTAL" };
            assy.GetIntegerReportProperties(integerReportProperties, ref integerProperties);
            int number = integerProperties.ContainsKey("NUMBER") ? int.Parse(integerProperties["NUMBER"].ToString()) : 0;
            int modelTotal = integerProperties.ContainsKey("ASSEMBLY.MODEL_TOTAL") ? int.Parse(integerProperties["ASSEMBLY.MODEL_TOTAL"].ToString()) : 1;
            _quantidade = number * modelTotal;



            int qtdModel = 0;
            part.GetReportProperty("ASSEMBLY.MODEL_TOTAL", ref qtdModel);

            

            _quantidade = number * qtdModel;




            PecaDgt peca = new PecaDgt(part);
            
            _itens.Add(peca);
        }

        //private void pegarDadosRelatorio()
        //{
        //    PropertyInfo propInfo = drawing.GetType().GetProperty("Identifier",
        //                                BindingFlags.Instance | BindingFlags.NonPublic);
        //    object value = propInfo.GetValue(drawing, null);
        //    Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

        //    // Cria um objeto ModelObject temporário com o mesmo Identifier
        //    Beam tempBeam = new Beam();
        //    tempBeam.Identifier = identifier;

        //    tempBeam.GetReportProperty("REVISION.LAST_MARK", ref _revisionLastMark);
        //    tempBeam.GetReportProperty("REVISION.LAST_DESCRIPTION", ref _revisionLastDescription);
        //    tempBeam.GetReportProperty("REVISION.LAST_CREATED_BY", ref _revisionLastCreatedBy);
        //    tempBeam.GetReportProperty("REVISION.LAST_DATE_CREATE", ref _revisionLastDateCreated);
        //    tempBeam.GetReportProperty("REVISION.LAST_CHECKED_BY", ref _revisionLastCheckedBy);
        //    tempBeam.GetReportProperty("REVISION.LAST_DATE_CHECKED", ref _revisionLastDateChecked);
        //    tempBeam.GetReportProperty("REVISION.LAST_APPROVED_BY", ref _revisionLastApprovedBy);
        //    tempBeam.GetReportProperty("REVISION.LAST_DATE_APPROVED", ref _revisionLastDateApproved);
        //}
            
        

        public void AddItem(TSM.Part part, string assemblyPos)
        {
            string posicaoPeca = part.ObterPropriedade("PART_POS").ToString();

            if (_itens.Any(c => c.PartPos == posicaoPeca))
            {
                var existingItem = _itens.First(c => c.PartPos == posicaoPeca);
                existingItem.IncrementarQuantidade();

                if (existingItem.PartPos == _pecaDgtMaiPart.PartPos)
                {
                    _quantidade++;
                }

                return;
            }

            PecaDgt item = new PecaDgt(part);
            _itens.Add(item);
        }

        public void AddItemExistente(string posicaoPeca)
        {
            var existingItem = _itens.FirstOrDefault(c => c.PartPos == posicaoPeca);
            if (existingItem != null)
            {
                existingItem.IncrementarQuantidade();
            }
        }

        public void MultiplicaQtdConjuntoPorPeca()
        {
            //var itemAvulso = _itens.FirstOrDefault();
            //itemAvulso.MultiplicaQtdConjuntoPorPeca(_quantidade);
            foreach (var item in _itens)
            {
                item.MultiplicaQtdConjuntoPorPeca(_quantidade);
            }
        }
    }
}
