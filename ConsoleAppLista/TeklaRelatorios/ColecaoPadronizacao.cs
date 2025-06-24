using ConsoleAppLista.Infra.Dtos;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TSD = Dynamic.Tekla.Structures.Drawing;

namespace ConsoleAppLista.TeklaRelatorios
{
    public class ColecaoPadronizacao
    {
        TSD.DrawingHandler _dh;

        private List<ProdutoPadraoDTO> _produtosPadrao;
        private List<ConjuntoPadraoDTO> _conjuntosPadrao;
        private List<PecaDTO> _pecasPadrao;

        public ColecaoPadronizacao()
        {
            _dh = new TSD.DrawingHandler();
            _produtosPadrao = new List<ProdutoPadraoDTO>();
            _conjuntosPadrao = new List<ConjuntoPadraoDTO>();
            _pecasPadrao = new List<PecaDTO>();
        }
        public void Coletar(List<string[]> linhas)
        {

            _dh = new TSD.DrawingHandler();

            ArrayList ObjectsToSelect = new ArrayList();

            var dg = _dh.GetDrawingSelector().GetSelected();



            while (dg.MoveNext())
            {
                var drawing = dg.Current;

                if (drawing == null)
                    break;

                var tipo = drawing.GetType();

                if (tipo == typeof(TSD.GADrawing))
                {
                    var ga = drawing as TSD.GADrawing;

                    var titulo1 = ga.Title1.Trim();
                    var nome = ga.Name.Trim();
                    var data = ga.ModificationDate;

                    _produtosPadrao.Add(new ProdutoPadraoDTO()
                    {
                        PartNumber = titulo1,
                        Descricao = nome,
                        UltimaModificacao = data
                    });

                }
            }


            string partNumberDiagrama = string.Empty;
            //string partNumberConjunto = string.Empty;


            for (int i = 1; i < linhas.Count - 1; i++)
            {
                var linha = linhas[i];
                var t1 = linha[0].Trim();
                switch (t1)
                {
                    case "D":
                        {
                            var produto = _produtosPadrao.FirstOrDefault(x => x.PartNumber == linha[1].Trim());
                            if(produto != null)
                            {
                                partNumberDiagrama = linha[1].Trim();
                            }
                        }
                        break;

                    case "C":
                        {
                            var conjunto = _conjuntosPadrao.FirstOrDefault(x => x.PartNumber == linha[1].Trim());
                            if(conjunto == null)
                            {
                                conjunto = new ConjuntoPadraoDTO()
                                {
                                    PartNumber = linha[1].Trim(),
                                    Descricao = linha[2].Trim()
                                };
                            }
            
                 
                            

                            //var produto = _produtosPadrao.Last();
                            //produto.AddConjunto(conjunto);

                        }
                        break;

                    case "P":
                        {
                            var peca = new PecaDTO()
                            {
                                PartNumber = linha[1].Trim(),
                                Descricao = linha[2].Trim()
                            };

                            var produto = _produtosPadrao.FirstOrDefault(x => x.PartNumber == linha[1].Trim());
                        }
                        break;
                }





            }







        }
    }
}
