using netDxf;
using netDxf.Entities;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Blocos
{
    public class InsercaoConjuntos : Insercao
    {
        protected const string ARQUIVO_ORIGEM = @"C:\BlocosTecnoedCSN\BLOCOS.dxf";
        protected const string BLOCO_ITEM_LISTA1 = @"ITEM_LISTA1";
        protected const double LARGURA_LINHA = 5.0;

        
        public InsercaoConjuntos(string arquivoOrigem, string nomeBloco, Formato formato, ArquivoDxf arquivoDxf):base(arquivoOrigem, nomeBloco, formato, arquivoDxf)
        {
            
            
        }

        public override Insert Inserir()
        {
            Vector3 ptReferenciaLista = new Vector3(_formato.PontoInsercaoLm.X, _formato.PontoInsercaoLm.Y, 0.0);

            Vector3 ptInsercaoLocal = new Vector3(
                ptReferenciaLista.X - 180.0,
                ptReferenciaLista.Y - LARGURA_LINHA,
                ptReferenciaLista.Z);

            var espacoEntreMarcas = 0.0;
            foreach (ConjuntoLM conjunto in _arquivoDxf.ObterConjuntos())
            {
                ptInsercaoLocal = new Vector3(
                    ptInsercaoLocal.X,
                    ptInsercaoLocal.Y - espacoEntreMarcas,
                    ptInsercaoLocal.Z);

                var conjuntoInserido = inserir(ptInsercaoLocal, _formato.FatorEscala);

                var atributos = new AtributosLinhaMarca(_arquivoDxf, conjunto);
                atributos.Atributar(conjuntoInserido);

                foreach (var item in conjunto.LinhasItem)
                {

                    ptInsercaoLocal = new Vector3(
                      ptInsercaoLocal.X,
                      ptInsercaoLocal.Y - LARGURA_LINHA,
                      ptInsercaoLocal.Z);

                    var insercaoItens = new InsercaoItem(ARQUIVO_ORIGEM, BLOCO_ITEM_LISTA1, _formato, _arquivoDxf, item, ptInsercaoLocal, _formato.FatorEscala, new AtributosLinhaItem(_arquivoDxf, item));

                    insercaoItens.Inserir();

                }

                espacoEntreMarcas = LARGURA_LINHA;

            }

            return null;

        }
    }
}
