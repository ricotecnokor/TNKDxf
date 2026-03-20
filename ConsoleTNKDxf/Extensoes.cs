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

        public static string ObterPosicaoItemPrincipal(this TSM.Assembly assy)
        {
            var mainPart = assy.GetMainPart();
            var numeroMainPart = string.Empty;
            mainPart.GetReportProperty("PART_POS", ref numeroMainPart);
            return numeroMainPart;
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
