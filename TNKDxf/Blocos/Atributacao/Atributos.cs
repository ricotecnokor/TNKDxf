using netDxf.Entities;
using System.Collections;
using TNKDxf.Dominio.Dxfs;

namespace TNKDxf.Blocos
{
    public abstract class Atributos : IAtributos
    {

        protected ArquivoDxf _arquivoDxf;

        protected Atributos(ArquivoDxf arquivoDxf)
        {
            _arquivoDxf = arquivoDxf;
        }

        protected string preencherAtributos(Insert insert, Hashtable campos)
        {

            if (insert.Attributes.Count > 0)
            {
                foreach (var attribute in insert.Attributes)
                {
                    var attributeDefinition = attribute.Definition;
                    if (attributeDefinition != null)
                    {
                        attribute.Position = attributeDefinition.Position + insert.Position;
                        if (campos != null)
                        {
                            if (campos.ContainsKey(attributeDefinition.Tag.ToUpper()))
                            {
                                attribute.Color = attributeDefinition.Color;
                                attribute.Value = campos[attributeDefinition.Tag.ToUpper()].ToString();
                            }
                        }
                    }
                }
            }


            return insert.Handle;
        }

        public abstract void Atributar(Insert inserido);

    }
}
