using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class Material
    {
        private string _perfil;
        private string _tipoPerfil;
        private string _material;
        private string _descricao;

        public string Descricao => _descricao;
        public Material(TSM.Part pecaChild)
        {
            ArrayList stringReportProperties = new ArrayList { "PROFILE", "PROFILE_TYPE", "MATERIAL" };
            Hashtable stringProterties = new Hashtable();
            pecaChild.GetStringReportProperties(stringReportProperties, ref stringProterties);
            _perfil = stringProterties.ContainsKey("PROFILE") ? stringProterties["PROFILE"].ToString() : string.Empty;
            _tipoPerfil = stringProterties.ContainsKey("PROFILE_TYPE") ? stringProterties["PROFILE_TYPE"].ToString() : string.Empty;
            _material = stringProterties.ContainsKey("MATERIAL") ? stringProterties["MATERIAL"].ToString() : string.Empty;

            _descricao = obterDescricaoMaterial();
        }

        private string obterDescricaoMaterial()
        {
           return _material == "ASTM A36 XADREZ 6.3" ? "ASTM A36" :
                  (_tipoPerfil == "B" && _perfil.StartsWith("CH")) ? _material :
                  (_tipoPerfil == "B" && _perfil.StartsWith("XAD")) ? "ASTM A36" :
                  (_tipoPerfil == "B" && _perfil.StartsWith("EXP")) ? "SAE 1006/10" :
                  (_tipoPerfil == "B" && _perfil.StartsWith("GRA")) ? "AÇO" :
                  (_tipoPerfil == "B" && _perfil.StartsWith("TELA_CRI")) ? "AÇO" :
                  _tipoPerfil != "B" ? _material :
                  _material.StartsWith("TELA MALHA 25.4 X 25.4 X 2.76") ? "AÇO" :
                  _material.StartsWith("AÇO TELHA MF18") ? "AÇO" :
                  (_material.StartsWith("AÇO-GS") || _material.StartsWith("AÇO GSA4-254")) ? "AÇO" : _material;
        }
    }
}

/*
 if (GetValue("MATERIAL")=="ASTM A36 XADREZ 6.3")

then

 "ASTM A36"

else

if (GetValue("PROFILE_TYPE")!= "B") then GetValue("MATERIAL")
else 

if ((GetValue("PROFILE_TYPE")=="B") && (mid(GetValue("PROFILE"),"0","2")=="CH")) then GetValue("MATERIAL")
else

if ((GetValue("PROFILE_TYPE")=="B") && (mid(GetValue("PROFILE"),"0","3")=="XAD")) then "ASTM A36"
else

if ((GetValue("PROFILE_TYPE")=="B") && (mid(GetValue("PROFILE"),"0","3")=="EXP"))  then "SAE 1006/10 "
else

if ((GetValue("PROFILE_TYPE")=="B") && (mid(GetValue("PROFILE"),"0","3")=="GRA")) then "AÇO"
else

if ((GetValue("PROFILE_TYPE")=="B") && (mid(GetValue("PROFILE"),"0","8")=="TELA_CRI"))  then "AÇO" 
else

if (mid(GetValue("MATERIAL"),"0","29")=="TELA MALHA 25.4 X 25.4 X 2.76")  then "AÇO"
else

if (mid(GetValue("MATERIAL"),"0","14")=="AÇO TELHA MF18")  then "AÇO"
else
 
if ((mid(GetValue("MATERIAL"),"0","6")=="AÇO-GS")) || ((mid(GetValue("MATERIAL"),"0","12")=="AÇO GSA4-254")) then "AÇO" 

else

if ((GetValue("PROFILE_TYPE")=="B") && (mid(GetValue("PROFILE"),"0","2")!="CH")||
                                       (mid(GetValue("PROFILE"),"0","3")!="XAD")||
                                       (mid(GetValue("PROFILE"),"0","3")!="EXP")||  
                                       (mid(GetValue("PROFILE"),"0","3")!="GRA"))  then GetValue("MATERIAL")
else   
                           
endif
endif
endif
endif
endif
endif
endif
endif
endif
endif
endif
 */
