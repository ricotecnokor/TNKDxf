using System.Collections;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class DescricaoArruela
    {
        private string _descricao;
        public DescricaoArruela(TSM.BoltArray boltArray)
        {

            ArrayList stringReportProperties = new ArrayList { "WASHER.NAME" };
            Hashtable stringProperties = new Hashtable();
            boltArray.GetStringReportProperties(stringReportProperties, ref stringProperties);
            string name = stringProperties["WASHER.NAME"]?.ToString();

            string etapa1 = name.Substring(0, name.IndexOf("_"));
            string etapa2 = etapa1.Substring(17, etapa1.Length - 18);

            _descricao = "ARRUELA LISA " + etapa2;
            //"ARRUELA LISA " + mid(GetFieldFormula("ARRUELA_1"),"15","5")

            //mid(GetValue("NAME"),"0",find(GetValue("NAME"),"_"))

        }

        public override string ToString()
        {
            return _descricao;
        }



        public static implicit operator string(DescricaoArruela valor)
        {
            return valor?._descricao ?? string.Empty;
        }
    }
}
