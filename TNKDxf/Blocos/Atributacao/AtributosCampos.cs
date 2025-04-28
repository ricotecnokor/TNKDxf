using netDxf.Entities;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using TNKDxf.Dominio.Abstracoes;
using TNKDxf.Dominio.Coletores;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;
using TNKDxf.Dominio.Extensoes;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Blocos.Atributacao
{
    public class AtributosCampos : Atributos
    {
        public AtributosCampos(ArquivoDxf arquivoDxf) : base(arquivoDxf)
        {
        }

        //private CamposFormato _campos;


        //public AtributosCampos(CamposFormato campos)
        //{
        //    _campos = campos;
        //}

        public override void Atributar(Insert inserido)
        {
            Ponto2d ptReferenciaLista = DxfSingleton.DxfDocument.Extmin();

            Hashtable hashtableMarca = new Hashtable();

            // Use LINQ's GroupBy to achieve the same effect as DistinctBy
            //var campos = _campos.ObterCampos()
            //                           .GroupBy(c => c[0])
            //                           .Select(g => g.First());

            var campos = _arquivoDxf.ObterCamposDoFormato();
               

            foreach (var campo in campos)
            {
                var nomeCampo = campo[0];
                nomeCampo = Encoding.UTF8.GetString(Encoding.Default.GetBytes(nomeCampo));
             
                //nomeCampo = nomeCampo.Replace("Ãš", "Ú");
                //nomeCampo = nomeCampo.Replace("Ã\\u008d", "Í");

                hashtableMarca.Add(nomeCampo, campo[1]);
            }

            preencherAtributos(inserido, hashtableMarca);
        }
    }
}
