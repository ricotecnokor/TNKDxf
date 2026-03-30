using System;
using System.Collections;
using TSM = Tekla.Structures.Model;


namespace ConsoleTNKDxf
{
    public class DescricaoItem
    {
        
        private double _height;
        private double _width;
        private double _length;
        private double _diametro;
        private double _espessura;
        private string _perfil;
        private string _tipoPerfil;
        private string _material;
        private string _descricao;

      
        public DescricaoItem(TSM.Part pecaChild)
        {
            ArrayList doubleReportProperties = new ArrayList { "HEIGHT", "LENGTH", "WIDTH", "PROFILE.DIAMETER", "PROFILE.PLATE_THICKNESS" };
            ArrayList stringReportProperties = new ArrayList { "PROFILE", "PROFILE_TYPE", "MATERIAL" };
            Hashtable doubleProperties = new Hashtable();
            Hashtable stringProterties = new Hashtable();
            pecaChild.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            pecaChild.GetStringReportProperties(stringReportProperties, ref stringProterties);

            _height = doubleProperties.ContainsKey("HEIGHT") && double.TryParse(doubleProperties["HEIGHT"].ToString(), out double result) ? result : 0;
            _width = doubleProperties.ContainsKey("WIDTH") && double.TryParse(doubleProperties["WIDTH"].ToString(), out result) ? result : 0;
            _length = doubleProperties.ContainsKey("LENGTH") && double.TryParse(doubleProperties["LENGTH"].ToString(), out result) ? result : 0;
            _diametro = doubleProperties.ContainsKey("PROFILE.DIAMETER") && double.TryParse(doubleProperties["PROFILE.DIAMETER"].ToString(), out result) ? result : 0;
            _espessura = doubleProperties.ContainsKey("PROFILE.PLATE_THICKNESS") && double.TryParse(doubleProperties["PROFILE.PLATE_THICKNESS"].ToString(), out result) ? result : 0;

            _perfil = stringProterties.ContainsKey("PROFILE") ? stringProterties["PROFILE"].ToString() : string.Empty;
            _tipoPerfil = stringProterties.ContainsKey("PROFILE_TYPE") ? stringProterties["PROFILE_TYPE"].ToString() : string.Empty;
            _material = stringProterties.ContainsKey("MATERIAL") ? stringProterties["MATERIAL"].ToString() : string.Empty;

            _descricao = pegaDescricao();



        }

        private string pegaDescricao()
        {
            string _SIGLA_PROFILE = pegaSigla();
            if (_SIGLA_PROFILE == "CH" || _SIGLA_PROFILE == "CH_EXP" || _SIGLA_PROFILE == "CHXAD" || _SIGLA_PROFILE == "GRADE")
            {
                return pegaChapa(_SIGLA_PROFILE);
            }

            if ((_perfil.Length >= 18 && _perfil.Substring(0, 18) == "MÓDULO DE PROTEÇÃO") || (_perfil.Length >= 17 && _perfil.Substring(0, 17) == "RABICHO SEGURANÇA") || (_perfil.Length >= 12 && _perfil.Substring(0, 12) == "TAMPA 40X 60") || (_perfil.Length >= 13 && _perfil.Substring(0, 13) == "FIXADOR BELGO"))
            {
                return pegaModuloProtecaoBelgo();
            }

            if ((_perfil.Length >= 14 && _perfil.Substring(0, 14) == "POSTE SUSPENSO") || (_perfil.Length >= 23 && _perfil.Substring(0, 23) == "POSTE COM BASE METÁLICA"))
            {
                return pegaPosteProtecaoBelgo();
            }

            if (_perfil.Length >= 4 && _perfil.Substring(0, 4) == "TELA")
            {
                return PegaTela();
            }

            if (_perfil.Length >= 11 && _perfil.Substring(0, 11) == "CONTRA PINO")
            {
                return pegaContraPino();
            }

            if (_perfil.Length >= 6 && _perfil.Substring(0, 6) == "GRAMPO")
            {
                return pegaGrampo();
            }

            if ((_perfil.Length >= 7 && _perfil.Substring(0, 7) == "ARRUELA") || (_perfil.Length >= 5 && _perfil.Substring(0, 5) == "PARAF") || (_perfil.Length >= 5 && _perfil.Substring(0, 5) == "PORCA"))
            {
                return pegaParafusos();
            }

            if (_tipoPerfil == "I" || _tipoPerfil == "L" || _tipoPerfil == "U" || _tipoPerfil == "C" || _tipoPerfil == "T" || _tipoPerfil == "Z")
            {
                return _perfil + " X " + (int)_length;
            }

            if (_tipoPerfil == "B" && (_perfil.Length >= 2 && (_perfil.Substring(0, 2) == "BQ" || _perfil.Substring(0, 2) == "BC")))
            {
                return _perfil + " X " + (int)_length;
            }

            if (_tipoPerfil == "RU")
            {
                return pegTipoRUBREDCHDE();
            }

            if (_tipoPerfil == "RO")
            {
                return pegTipoROTUBOCHDEDI();
            }

            return _perfil + " X " + (int)_length;

        }

        private string pegTipoROTUBOCHDEDI()
        {
            return (_perfil.Substring(0, 3) == "TBS" || _perfil.Substring(0, 3) == "TBE")
                ? _perfil.Substring(0, _perfil.IndexOf("C")) + "X " + (int)_length + " " + _perfil.Substring(_perfil.LastIndexOf("C") - 2)
                : (_perfil.Substring(0, 8) == "TUBO_MEC")
                    ? "TB MEC DE" + (int)_diametro + " DI" + (int)(_diametro - (_espessura * 2)) + " X " + (int)_length
                    : (_perfil.Substring(0, 6) == "TUBO_E")
                        ? "TB DE" + (int)_diametro + " X DI" + (int)(_diametro - (_espessura * 2)) + " X " + (int)_length
                        : (_perfil.Substring(0, 8) == "CH_DE_DI")
                            ? _perfil.Substring(0, 2) + " " + (int)_length + " X DE" + (int)_diametro + " X DI" + ((int)_diametro - ((int)_espessura * 2))
                            : _perfil.Substring(0, 2) == "CURVA"
                                ? _perfil
                                : (_perfil.Substring(0, 4) == "CHR6")
                                    ? "CH 6.3 x X DI42 X DE" + (int)_height
                                    : (_perfil.Substring(0, 4) == "CHR1")
                                        ? "CH 12.5 x X DI42 X DE" + (int)_height
                                        : string.Empty;
        }

        private string pegTipoRUBREDCHDE()
        {
            return (_tipoPerfil == "RU" && _perfil.Substring(0, 3) == "BRL") || (_tipoPerfil == "RU" && _perfil.Substring(0, 3) == "BRF") || (_tipoPerfil == "RU" && _perfil.Substring(0, 3) == "BRT")
                ? _perfil + " X " + (int)_length
                : (_tipoPerfil == "RU" && _perfil.Substring(0, 5) == "CH_DE")
                    ? _perfil.Substring(0, 2) + " " + (int)_length + " X DE" + (int)_diametro    
                    : string.Empty;
        }

        private string pegaParafusos()
        {
            throw new NotImplementedException();
        }

        private string pegaGrampo()
        {
            return _perfil.Substring(0, 6) == "GRAMPO" ? _perfil : string.Empty;
        }

        private string pegaContraPino()
        {
            return _perfil.Substring(0, 25) == "CONTRA PINO 8 X 55 DIN 94" ? "CONTRA PINO 8 X 55 DIN 94" :
                   _perfil.Substring(0, 27) == "CONTRA PINO 10 X 100 DIN 94" ? "CONTRA PINO 10 X 100 DIN 94" :
                   _perfil.Substring(0, 26) == "CONTRA PINO 10 X 65 DIN 94" ? "CONTRA PINO 10 X 65 DIN 94" :
                   _perfil.Substring(0, 26) == "CONTRA PINO 10 X 75 DIN 94" ? "CONTRA PINO 10 X 75 DIN 94" : string.Empty;
        }

        private string PegaTela()
        {
            return _material.Substring(0, 29) == "TELA MALHA 25.4 X 25.4 X 2.76"
                ? "TELA MALHA 25.4 X 25.4 X 2.76" + " X " + (int)_height + " X " + (int)_length
                : _material.Substring(0, 29) == "TELA MALHA 25.4 X 25.4 X 3.05"
                    ? "TELA MALHA 25.4 X 25.4 X 3.05" + " X " + (int)_height + " X " + (int)_length
                    : _material.Substring(0, 19) == "TELA MALHA 19 X 2.1"
                        ? "TELA MALHA 19 X 2.1" + " X " + (int)_height + " X " + (int)_length
                        : _material.Substring(0, 21) == "TELA MALHA 25 FIO 2MM"
                            ? "TELA MALHA 25 FIO 2MM" + " X " + (int)_height + " X " + (int)_length
                            : string.Empty;
        }

        private string pegaModuloProtecaoBelgo()
        {
            return _perfil == "MÓDULO DE PROTEÇÃO 600 X 1500" ? "MODULO DE PROTECAO 600 X 1500" :
                   _perfil == "MÓDULO DE PROTEÇÃO 800 X 1500" ? "MODULO DE PROTECAO 800 X 1500" :
                   _perfil == "MÓDULO DE PROTEÇÃO 1000 X 1500" ? "MODULO DE PROTECAO 1000 X 1500" :
                   _perfil == "MÓDULO DE PROTEÇÃO 1500 X 1500" ? "MODULO DE PROTECAO 1500 X 1500" :
                   _perfil == "MÓDULO DE PROTEÇÃO 1800 X 1500" ? "MODULO DE PROTECAO 1800 X 1500" :
                   _perfil == "MÓDULO DE PROTEÇÃO 2000 X 1500" ? "MODULO DE PROTECAO 2000 X 1500" :
                   _perfil.Substring(0, 12) == "TAMPA 40X 60" ? "TAMPA 40 X 60" :
                   _perfil == "FIXADOR BELGO 35 X 30" ? "FIXADOR 35 X 30 C/PARAF. SEXT. INOX M8" :
                   _perfil == "RABICHO SEGURANÇA BELGO DE 6 X 100" ? "RABICHO DE SEGURANÇA DE 8 X 100" : string.Empty;
            
        }
        private string pegaPosteProtecaoBelgo()
        {
    
            return _perfil == "POSTE SUSPENSO 40 X 60 X 630" ? "POSTE BASE SUSPENSA 40 X 60 X 630" :
                   _perfil == "POSTE SUSPENSO 40 X 60 X 1000" ? "POSTE BASE SUSPENSA 40 X 60 X 1000" :
                   _perfil == "POSTE SUSPENSO 40 X 60 X 1500" ? "POSTE BASE SUSPENSA 40 X 60 X 1500" :
                   _perfil == "POSTE SUSPENSO 40 X 60 X 1800" ? "POSTE BASE SUSPENSA 40 X 60 X 1800" :
                   _perfil == "POSTE SUSPENSO 40 X 60 X 2000" ? "POSTE BASE SUSPENSA 40 X 60 X 2000" :
                   _perfil == "POSTE COM BASE METÁLICA 40 X 60 X 630" ? "POSTE BASE APARAFUSADA 40 X 60 X 630" :
                   _perfil == "POSTE COM BASE METÁLICA 40 X 60 X 1080" ? "POSTE BASE APARAFUSADA 40 X 60 X 1080" :
                   _perfil == "POSTE COM BASE METÁLICA 40 X 60 X 1580" ? "POSTE BASE APARAFUSADA 40 X 60 X 1580" :
                   _perfil == "POSTE COM BASE METÁLICA 40 X 60 X 1880" ? "POSTE BASE APARAFUSADA 40 X 60 X 1880" :
                   _perfil == "POSTE COM BASE METÁLICA 40 X 60 X 2080" ? "POSTE BASE APARAFUSADA 40 X 60 X 2080" :
                   "POSTE ESPECIAL 40 X 60 X " + (int)_length;

        }
        private string pegaChapa(string _SIGLA_PROFILE)
        {
            switch (_SIGLA_PROFILE)
            {
                case "CH":
                    return pegaChapaComum();
                case "CH_EXP":
                    return pegaChapaExp();
                case "CHXAD":
                    return pegaChapaXadrez();
                case "GRADE":
                    return pegaGrade();
                default:
                    return string.Empty;
            }

        }
        private string pegaChapaXadrez()
        {
            return _material == "ASTM A36 XADREZ 4.7"
                ? "CH XAD 4.7" + " X " + (int)_height + " X " + (int)_length
                : _material == "ASTM A36 XADREZ 6.3"
                    ? "CH XAD 6.3" + " X " + (int)_height + " X " + (int)_length
                    : _material == "ASTM A36 XADREZ 9.5"
                        ? "CH XAD 9.5" + " X " + (int)_height + " X " + (int)_length
                        : string.Empty;

        }
        private string pegaChapaComum()
        {
            return "CH " + Math.Round(_width, 1) + " X " + (int)_height + " X " + (int)_length;
        }
        private string pegaChapaExp()
        {
            return _material == "SAE 1006/10 GME 3" || _material == "SAE 1006/10 GME 4"
                ? "CH EXP 4.7" + " X " + (int)_height + " X " + (int)_length + (" ") + _material.Substring(12, 5)
                : _material == "SAE 1006/10 GME 1" || _material == "SAE 1006/10 GME 2" || _material == "SAE 1006/10 GME 5" || _material == "SAE 1006/10 GME 6"
                    ? "CH EXP 6.3" + " X " + (int)_height + " X " + (int)_length + (" ") + _material.Substring(12, 5)
                    : _material == "SAE 1006/10 GME 7" || _material == "SAE 1006/10 GME 8"
                        ? "CH EXP 8.0" + " X " + (int)_height + " X " + (int)_length + (" ") + _material.Substring(12, 5)
                        : _material == "SAE 1006/10 GME 9"
                            ? "CH EXP 9.5" + " X " + (int)_height + " X " + (int)_length + (" ") + _material.Substring(12, 5)
                            : string.Empty;
        }
        private string pegaGrade()
        {
            return _material.Substring(3, 8) == "SERR."
                ? "GRADE " + _material.Substring(4, 8) + (" ") + (int)_height + " X " + (int)_length + " SERRILHADA"
                : "GRADE " + _material.Substring(4, 8) + (" ") + (int)_height + " X " + (int)_length;
        }
        private string pegaSigla()
        {
            char[] digitos = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'Ø' };
            int index = _perfil.IndexOfAny(digitos);
            return index == -1 ? _perfil : _perfil.Substring(0, index);
        }

        public override string ToString()
        {
            return _descricao;
        }



        public static implicit operator string(DescricaoItem valor)
        {
            return valor?._descricao ?? string.Empty;
        }
    }
}

