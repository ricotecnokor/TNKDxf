using netDxf;
using netDxf.Entities;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Extensoes;

namespace TNKDxf.Blocos
{
    public class InsercaoCabecalho : Insercao
    {
        protected const string BLOCO_MARCA = @"ITEM_LISTA0";
      
        public InsercaoCabecalho(string arquivoOrigem, string nomeBloco, Formato formato, ArquivoDxf arquivoDxf) : base(arquivoOrigem, nomeBloco, formato, arquivoDxf)
        {
        }
   
        public override Insert Inserir()
        {
            var margem = 10 * _formato.FatorEscala;
            Vector3 ptReferenciaLista = new Vector3(
                    DxfSingleton.DxfDocument.Extmax().X - margem,
                    DxfSingleton.DxfDocument.Extmax().Y - margem, 0.0);

            Insert insert = inserir(ptReferenciaLista, _formato.FatorEscala);

            var ajusteCabecalho = new AjusteCabecalho(_formato, _arquivoDxf);
            ajusteCabecalho.Ajustar(insert); 

            InsercaoConjuntos insercaoConjuntos = new InsercaoConjuntos(_arquivoOrigem, BLOCO_MARCA, _formato, _arquivoDxf);
            insercaoConjuntos.Inserir();

            return insert;  
        }
    }
}
