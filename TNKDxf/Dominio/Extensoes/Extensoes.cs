using netDxf;
using netDxf.Header;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Extensoes
{
    public static class Extensoes
    {


        public static Ponto2d Extmin(this DxfDocument dxf)
        {
            HeaderVariable headerVariable;
            Vector3 vector3 = new Vector3();
            if (dxf.DrawingVariables.TryGetCustomVariable("$EXTMIN", out headerVariable))
            {
                vector3 = (Vector3)headerVariable.Value;
            }

            return Ponto2d.CriarSemEscala(vector3.X, vector3.Y); ;
        }

        public static Ponto2d Extmax(this DxfDocument dxf)
        {
            HeaderVariable headerVariable;
            Vector3 vector3 = new Vector3();
            if (dxf.DrawingVariables.TryGetCustomVariable("$EXTMAX", out headerVariable))
            {
                vector3 = (Vector3)headerVariable.Value;
            }

            return Ponto2d.CriarSemEscala(vector3.X, vector3.Y); ;
        }
    }
}
