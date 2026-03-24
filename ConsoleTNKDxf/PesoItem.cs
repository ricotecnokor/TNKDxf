using System;
using System.Collections;
using System.Globalization;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class PesoItem
    {
        private string _perfil;
        private string _tipoPerfil;
        private double _peso;
        private double _pesoLiquido;
        private double _pesoBruto;
        private double _pesoUnitarioComprimento;
        private double _pesoM;
        private double _length;
        private double _pesoCalculado;
        private double _pesoTotal;

        public double PesoTotal => _pesoTotal;
        public double PesoCalculado => _pesoCalculado;  

        public PesoItem(TSM.Part pecaChild)
        {
            ArrayList doubleReportProperties = new ArrayList { "PROFILE.WEIGHT_PER_UNIT_LENGTH", "WEIGHT_NET", "WEIGHT_GROSS", "WEIGHT_M", "WEIGTH", "LENGTH" };
            ArrayList stringReportProperties = new ArrayList { "PROFILE", "PROFILE_TYPE" };
            Hashtable doubleProperties = new Hashtable();
            Hashtable stringProterties = new Hashtable();
            pecaChild.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            pecaChild.GetStringReportProperties(stringReportProperties, ref stringProterties);

            _perfil = stringProterties.ContainsKey("PROFILE") ? stringProterties["PROFILE"].ToString() : string.Empty;
            _tipoPerfil = stringProterties.ContainsKey("PROFILE_TYPE") ? stringProterties["PROFILE_TYPE"].ToString() : string.Empty;
            _length = doubleProperties.ContainsKey("LENGTH") && double.TryParse(doubleProperties["LENGTH"].ToString(), out double resultLength) ? resultLength : 0;
            _pesoLiquido = doubleProperties.ContainsKey("WEIGHT_NET") && double.TryParse(doubleProperties["WEIGHT_NET"].ToString(), out double result) ? result : 0;
            _pesoUnitarioComprimento = doubleProperties.ContainsKey("PROFILE.WEIGHT_PER_UNIT_LENGTH") && double.TryParse(doubleProperties["PROFILE.WEIGHT_PER_UNIT_LENGTH"].ToString(), out double result2) ? result2 : 0;
            _pesoBruto = doubleProperties.ContainsKey("WEIGHT_GROSS") && double.TryParse(doubleProperties["WEIGHT_GROSS"].ToString(), out double result3) ? result3 : 0;

            _pesoM = doubleProperties.ContainsKey("WEIGHT_M") && double.TryParse(doubleProperties["WEIGHT_M"].ToString(), out double result5) ? result5 : 0;

            _peso = doubleProperties.ContainsKey("WEIGTH") && double.TryParse(doubleProperties["WEIGTH"].ToString(), out double result4) ? result4 : 0;

            _pesoCalculado = obterPeso();
            _pesoTotal = _pesoCalculado;
        }





        private double obterPeso()
        {
            return _tipoPerfil == "B" || _perfil.StartsWith("LD") || _perfil.StartsWith("UE") || _perfil.StartsWith("UD") || _perfil.StartsWith("D") ? Math.Round(_pesoLiquido, 1) :
                   _perfil.StartsWith("CH_DE_DI") ? Math.Round(_pesoLiquido, 2) :
                   (_perfil.StartsWith("PORCA") || _perfil.StartsWith("PARAF") || _perfil.StartsWith("ARRUELA") || _perfil.StartsWith("CONTRA PINO")) ? Math.Round(_pesoLiquido, 2) :
                   (_perfil.StartsWith("MÓDULO DE PROTEÇÃO") || _perfil.StartsWith("RABICHO SEGURANÇA") || _perfil.StartsWith("TAMPA 40X 60") || _perfil.StartsWith("FIXADOR BELGO") || _perfil.StartsWith("POSTE SUSPENSO") || _perfil.StartsWith("POSTE COM BASE METÁLICA")) ? Math.Round(_pesoLiquido, 2) :
                   Math.Round((int)Math.Round(_length, 1) * double.Parse(
                       string.IsNullOrEmpty(_pesoUnitarioComprimento.ToString()) ? "0" : _pesoUnitarioComprimento.ToString(), CultureInfo.InvariantCulture) / 1000, 1);

        }

        public void IncrementarPeso()
        {
            _pesoTotal += _pesoCalculado;
        }



        /*
         if (GetValue("PROFILE_TYPE") == "B" ||
GetFieldFormula("_SIGLA_PROFILE") == "LD" ||
GetFieldFormula("_SIGLA_PROFILE") == "UE" ||
GetFieldFormula("_SIGLA_PROFILE") == "UD" ||
GetFieldFormula("_SIGLA_PROFILE") == "D" ) then
round(GetValue("WEIGHT_NET"), 0.1) 
else

if (mid(GetValue("PROFILE"),"0","8")=="CH_DE_DI")     

then 

round(GetValue("WEIGHT_NET"), 0.01)

else

if (mid(GetValue("PROFILE"),"0","5") == "PORCA" || 
mid(GetValue("PROFILE"),"0","5") == "PARAF" ||
mid(GetValue("PROFILE"),"0","7") == "ARRUELA" ||
mid(GetValue("PROFILE"),"0","11") == "CONTRA PINO") 
then
round(GetValue("WEIGHT_NET"), 0.01) 
else

if (mid(GetValue("PROFILE"),"0","18")=="MÓDULO DE PROTEÇÃO") ||
(mid(GetValue("PROFILE"),"0","17")=="RABICHO SEGURANÇA") ||
(mid(GetValue("PROFILE"),"0","12")=="TAMPA 40X 60") || 
(mid(GetValue("PROFILE"),"0","13")=="FIXADOR BELGO") ||
(mid(GetValue("PROFILE"),"0","14")=="POSTE SUSPENSO") ||
(mid(GetValue("PROFILE"),"0","23")=="POSTE COM BASE METÁLICA") ||

then

round(GetValue("WEIGHT_NET"), 0.01) 

else



round((int(round(GetValue("LENGTH") ,1)) * GetValue("PROFILE.WEIGHT_PER_UNIT_LENGTH") / 1000), 0.1)

endif
endif
endif
endif
         */

    }


}
