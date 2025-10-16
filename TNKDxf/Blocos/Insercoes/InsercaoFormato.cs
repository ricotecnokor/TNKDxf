using netDxf;
using netDxf.Entities;
using System.Linq;
using TNKDxf.Dominio.Abstracoes;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Blocos
{
    public class InsercaoFormato : Insercao
    {

        public InsercaoFormato(string arquivoOrigem, string nomeBloco, Formato formato, ArquivoDxf arquivoDxf) : base(arquivoOrigem, nomeBloco, formato, arquivoDxf)
        {
        }

        public override Insert Inserir()
        {

            var insert = DxfSingleton.DxfDocument.Entities.Inserts
                .FirstOrDefault(i => i.Block.Name == _nomeBloco);

            if (insert == null)
            {
                insert = inserir(new Vector3(0.0, 0.0, 0.0), _formato.FatorEscala);
            }


            return insert;
        }
    }
}
