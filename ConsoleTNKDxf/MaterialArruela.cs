using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class MaterialArruela
    {
        private string _descricao;
        public MaterialArruela(TSM.BoltArray boltArray)
        {
            ArrayList stringReportProperties = new ArrayList { "BOLT_STANDARD" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            string boltStandard = stringProperties["BOLT_STANDARD"]?.ToString();

            _descricao = boltStandard == "A325" || boltStandard == "RTA325" ? "ASTM F436" :
                         boltStandard == "A307" ? "ANSI B27.2-B" :
                         boltStandard == "5.8" ? "AÇO" : string.Empty;

            /*
            if ((GetValue("BOLT_STANDARD")== "A325")|| (GetValue("BOLT_STANDARD")== "RTA325"))
then
"ASTM F436"
else

if (GetValue("BOLT_STANDARD")== "A307")
then
"ANSI B27.2-B"
else

if (GetValue("BOLT_STANDARD")== "5.8")
then
"AÇO"
else


endif
endif
endif
             */
        }

        public override string ToString()
        {
            return _descricao;
        }



        public static implicit operator string(MaterialArruela valor)
        {
            return valor?._descricao ?? string.Empty;
        }
    }
}
