using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class DescricaoParafuso
    {
        private string _descricao;
        public DescricaoParafuso(TSM.BoltArray boltArray)
        {

            ArrayList stringReportProperties = new ArrayList { "NAME", "PROFILE" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            string profile = stringProperties["PROFILE"]?.ToString();
            string name = stringProperties["NAME"]?.ToString();

            if (profile != null)
            {
                if (profile.StartsWith("PARAF") || profile.StartsWith("PORCA") || profile.StartsWith("ARRUELA"))
                {
                    _descricao = profile;
                }
                else
                {
                    _descricao = "PARAF SEXT " + name.Substring(0, name.IndexOf("A"));
                }
            }

        }

        public override string ToString()
        {
            return _descricao;
        }



        public static implicit operator string(DescricaoParafuso valor)
        {
            return valor?._descricao ?? string.Empty;
        }

    }
}


/*
 if (mid(GetValue("PROFILE"),"0","5")=="PARAF")

 || (mid(GetValue("PROFILE"),"0","5")=="PORCA") 

     || (mid(GetValue("PROFILE"),"0","7")=="ARRUELA")

          then     GetValue("PROFILE")


else

"PARAF SEXT " + mid(GetFieldFormula("PARAFUSO_1"),"5","100")

endif
 */

/*
 mid(GetValue("NAME"),"0",find(GetValue("NAME"),"A"))

 */