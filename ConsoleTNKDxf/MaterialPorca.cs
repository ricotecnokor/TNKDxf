using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class MaterialPorca
    {
        private string _descricao;
        public MaterialPorca(TSM.BoltArray boltArray)
        {
            ArrayList stringReportProperties = new ArrayList { "BOLT_STANDARD" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            string boltStandard = stringProperties["BOLT_STANDARD"]?.ToString();

            _descricao = boltStandard == "A325" || boltStandard == "RTA325" ? "ASTM A194-2H" :
                         boltStandard == "A307" ? "ASTM A563-A" :
                         boltStandard == "5.8" ? "5" : string.Empty;


            /*
             if ((GetValue("BOLT_STANDARD")== "A325")|| (GetValue("BOLT_STANDARD")== "RTA325"))
then
"ASTM A194-2H"
else

if (GetValue("BOLT_STANDARD")== "A307")
then
"ASTM A563-A"
else

if (GetValue("BOLT_STANDARD")== "5.8")
then
"5"
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



        public static implicit operator string(MaterialPorca valor)
        {
            return valor?._descricao ?? string.Empty;
        }
    }
}
