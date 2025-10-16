
using netDxf;
using netDxf.Blocks;
using netDxf.Entities;
using System.Linq;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Blocos
{
    public abstract class Insercao : IInsercao
    {
        protected string _arquivoOrigem;
        protected string _nomeBloco;
        protected Formato _formato;
        protected ArquivoDxf _arquivoDxf;
        public Insercao(string arquivoOrigem, string nomeBloco, Formato formato, ArquivoDxf arquivoDxf)
        {
            _arquivoOrigem = arquivoOrigem;
            _nomeBloco = nomeBloco;
            _formato = formato;
            _arquivoDxf = arquivoDxf;
        }

        public abstract Insert Inserir();

        protected Insert inserir(Vector3 ptInsercao, double fatorEscala)
        {

            var insert = DxfSingleton.DxfDocument.Entities.Inserts
                .FirstOrDefault(i => i.Block.Name == _nomeBloco);

            if (insert == null)
            {
                DxfDocument sourceDoc = DxfDocument.Load(_arquivoOrigem);
                Block blockToInsert = sourceDoc.Blocks[_nomeBloco];

                DxfSingleton.DxfDocument.Blocks.Add(blockToInsert);

                insert = new Insert(blockToInsert)
                {
                    Position = ptInsercao,
                    Scale = new Vector3(fatorEscala, fatorEscala, fatorEscala)
                };

                DxfSingleton.DxfDocument.Entities.Add(insert);
            }
            else
            {
                Block blockToInsert = insert.Block;

                insert = new Insert(blockToInsert)
                {
                    Position = ptInsercao,
                    Scale = new Vector3(fatorEscala, fatorEscala, fatorEscala)
                };

                DxfSingleton.DxfDocument.Entities.Add(insert);
            }

            return insert;


        }

    }
}
