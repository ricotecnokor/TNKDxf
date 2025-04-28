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
        //ICamposExtraidos _camposExtraidos;
        //Vector3 _ptInsercaoFormato = new Vector3(0.0, 0.0, 0.0);
        //IRevisoesExtraidas _revisoesExtraidas;
        public InsercaoFormato(string arquivoOrigem, string nomeBloco, Formato formato, ArquivoDxf arquivoDxf) : base(arquivoOrigem, nomeBloco, formato, arquivoDxf)
        {
            //_camposExtraidos = new CampoExtraido(new ColetaCampos(formato, coletaErros));
            //_revisoesExtraidas = new RevisaoExtraida(new ColetaRevisoes(formato), coletaErros);
        }

        public override Insert Inserir()
        {
            //_camposExtraidos.Processar();
            //if (!_camposExtraidos.PodeInserirBlocos()) return;

            //_revisoesExtraidas.Processar();
            //if (!_revisoesExtraidas.PodeInserirBlocos()) return;


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
