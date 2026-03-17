using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using static Tekla.Structures.Model.ReferenceModel;
using TSD = Tekla.Structures.Drawing;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Desenho
    {
        TSM.Model _model;
        TSD.MultiDrawing _multiDrawing;
        private List<Conjunto> _conjuntos = new List<Conjunto>();
        private Revisao _revisao;

        public string Title1 => _multiDrawing.Title1;
        public Revisao Revisao => _revisao;

        public List<Conjunto> Conjuntos => _conjuntos;

        public Desenho(TSD.MultiDrawing multiDrawing, TSM.Model model)
        {
            _multiDrawing = multiDrawing;
            _model = model;
            _revisao = new Revisao(multiDrawing);
        }

        public void AddPeca(TSM.Part part)
        {
            var assy = part.GetAssembly();
            if (assy == null)
            {
                return;
            }

            //TSM.Assembly assy = _model.SelectModelObject(assembly.Identifier) as TSM.Assembly;


            string assemblyPos = assy.ObterPropriedade("ASSEMBLY_POS").ToString();
            if (assemblyPos == string.Empty)
            {
                Console.WriteLine("A posição do conjunto não pode ser nula ou vazia.");
                return;
            }
            

            string posicaoItemPrincipal = assy.ObterPosicaoItemPrincipal();

            if (_conjuntos.Any(conjunto => conjunto.Posicao == assemblyPos))
            {
                //string posicaoPeca = part.ObterPropriedade("PART_POS").ToString();
                //if (posicaoItemPrincipal == posicaoPeca)
                //{
                //    var marcaExistente = _conjuntos.FirstOrDefault(c => c.Posicao == assemblyPos);
                //    marcaExistente.IncrementarQuantidade();
                //    marcaExistente.IncrementarPeso();
                //}

                Conjunto conjuntoExistente = _conjuntos.FirstOrDefault(c => c.Posicao == assemblyPos);
                conjuntoExistente.AddItem(part);


                return;
            }

            var marca = new Conjunto(assy);
            _conjuntos.Add(marca);
        }

    }
}
