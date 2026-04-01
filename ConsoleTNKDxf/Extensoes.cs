using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public static class Extensoes
    {
        public static object ObterPropriedade(this TSM.Part part, string propertyName)
        {
            string value = string.Empty;
            part.GetReportProperty(propertyName, ref value);
            return value;
        }

        public static object ObterPropriedade(this TSM.Assembly assy, string propertyName)
        {
            string value = string.Empty;
            assy.GetReportProperty(propertyName, ref value);
            return value;
        }

        public static double ConverterParaDouble(this string valorPeso)
        {
            if (valorPeso != string.Empty)
            {
                if (valorPeso.Contains(","))
                {
                    return double.Parse(valorPeso.Replace(",", "."), System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    return double.Parse(valorPeso, System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            return 0.0;
        }

        //public static string TeklaSubstring(this string str, int startIndex, int? length = null)
        //{
        //    if (!length.HasValue)
        //    {
        //        return str.Substring(startIndex);
        //    }

        //    if(startIndex + length.Value > str.Length)
        //    {
        //        return str.Substring(startIndex, str.Length - startIndex);
        //    }

        //    return str.Substring(startIndex, length.Value);


        //}
    }


}
