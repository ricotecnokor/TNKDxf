using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class DescricaoPorca
    {
        private string _descricao;
        public DescricaoPorca(TSM.BoltArray boltArray)
        {
            ArrayList stringReportProperties = new ArrayList { "NUT.NAME" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            string name = stringProperties["NUT.NAME"]?.ToString();

            string etapa1 = name.Substring(0, name.IndexOf("A"));
            _descricao = name.Substring(0, 16) == "M16 - DIN 934" ? name.Substring(5, 30) : "PORCA SEXT " + etapa1.Substring(15, etapa1.Length - 15);

            /*
             if(mid(GetValue("NAME"),16.) == "M16 - DIN 934") then

mid(GetValue("NAME"),5,30)

else

"PORCA SEXT " + mid(GetFieldFormula("NUT_1"),"15","100")

endif
             */

            //mid(GetValue("NAME"), "0", find(GetValue("NAME"), "A"))
        }

        public override string ToString()
        {
            return _descricao;
        }



        public static implicit operator string(DescricaoPorca valor)
        {
            return valor?._descricao ?? string.Empty;
        }
    }
}
