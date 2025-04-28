using System;

namespace TNKDxf.Dominio.Extensoes
{
    public static class ExtensionsDouble
    {
        public static bool IgualComTolerancia(this double numero, double outro, double tolerancia, double fatorEscala)
        {
            var delta = numero - outro;
            tolerancia = tolerancia * fatorEscala;
            return Math.Abs(delta) <= tolerancia ? true : false;
        }
    }
}
