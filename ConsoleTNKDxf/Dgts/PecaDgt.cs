using System;
using System.Collections;
using System.Globalization;
using Tekla.Structures;
using TSM = Tekla.Structures.Model;

namespace ConsoleTNKDxf.Dgts
{
    public class PecaDgt
    {

        private string _partPos;
        private string _finish;
        private string _profile;
        private string _profileType;
        private string _material;

        private double _height;
        private double _length;
        private double _width;
        private double _profileDiameter;
        private double _profilePlateThickness;
        private double _profileWeightPerUnitLength;
        private double _weightNet;
        private double _weightGross;
        private double _weightM;
        private double _weight;
        private int _modelTotalQtd;
        private int _number;

        private int _quantidade;

        Identifier _identifier;

        public Identifier Identificador => _identifier;
        public string PartPos => _partPos;
        public string Finish => _finish;
        public string Profile => _profile;
        public string ProfileType => _profileType;
        public string Material => _material;
        public double Height => _height;
        public double Length => _length;
        public double Width => _width;
        public double ProfileDiameter => _profileDiameter;
        public double ProfilePlateThickness => _profilePlateThickness;
        public double ProfileWeightPerUnitLength => _profileWeightPerUnitLength;
        public double WeightNet => _weightNet;
        public double WeightGross => _weightGross;
        public double WeightM => _weightM;
        public double Weight => _weight;
        public int Quantidade => _quantidade;


        public PecaDgt(TSM.Part pecaChild)
        {
           
            _identifier = pecaChild.Identifier;

            Hashtable doubleProperties = new Hashtable();
            Hashtable stringProterties = new Hashtable();
            ArrayList doubleReportProperties = new ArrayList { "HEIGHT", "LENGTH", "WIDTH", "PROFILE.DIAMETER", "PROFILE.PLATE_THICKNESS", "PROFILE.WEIGHT_PER_UNIT_LENGTH", "WEIGHT_NET", "WEIGHT_GROSS", "WEIGHT_M", "WEIGTH" };
            pecaChild.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _height = doubleProperties.ContainsKey("HEIGHT") ? doubleProperties["HEIGHT"].ToString().ConverterParaDouble() : 0.0;
            _length = doubleProperties.ContainsKey("LENGTH") ? doubleProperties["LENGTH"].ToString().ConverterParaDouble() : 0.0;
            _width = doubleProperties.ContainsKey("WIDTH") ? doubleProperties["WIDTH"].ToString().ConverterParaDouble() : 0.0;
            _profileDiameter = doubleProperties.ContainsKey("PROFILE.DIAMETER") ? doubleProperties["PROFILE.DIAMETER"].ToString().ConverterParaDouble() : 0.0;
            _profilePlateThickness = doubleProperties.ContainsKey("PROFILE.PLATE_THICKNESS") ? doubleProperties["PROFILE.PLATE_THICKNESS"].ToString().ConverterParaDouble() : 0.0;
            _profileWeightPerUnitLength = doubleProperties.ContainsKey("PROFILE.WEIGHT_PER_UNIT_LENGTH") ? doubleProperties["PROFILE.WEIGHT_PER_UNIT_LENGTH"].ToString().ConverterParaDouble() : 0.0;
            _weightNet = doubleProperties.ContainsKey("WEIGHT_NET") ? doubleProperties["WEIGHT_NET"].ToString().ConverterParaDouble() : 0.0;
            _weightGross = doubleProperties.ContainsKey("WEIGHT_GROSS") ? doubleProperties["WEIGHT_GROSS"].ToString().ConverterParaDouble() : 0.0;
            _weightM = doubleProperties.ContainsKey("WEIGHT_M") ? doubleProperties["WEIGHT_M"].ToString().ConverterParaDouble() : 0.0;

            ArrayList stringReportProperties = new ArrayList { "PART_POS", "FINISH", "PROFILE", "PROFILE_TYPE", "MATERIAL", "MAIN_PART" };
            pecaChild.GetStringReportProperties(stringReportProperties, ref stringProterties);
            _partPos = stringProterties.ContainsKey("PART_POS") ? stringProterties["PART_POS"].ToString() : string.Empty;
            _finish = stringProterties.ContainsKey("FINISH") ? stringProterties["FINISH"].ToString() : string.Empty;
            _profile = stringProterties.ContainsKey("PROFILE") ? stringProterties["PROFILE"].ToString() : string.Empty;
            _profileType = stringProterties.ContainsKey("PROFILE_TYPE") ? stringProterties["PROFILE_TYPE"].ToString() : string.Empty;
            _material = stringProterties.ContainsKey("MATERIAL") ? stringProterties["MATERIAL"].ToString() : string.Empty;
            var mainPart = stringProterties.ContainsKey("MAIN_PART") ? stringProterties["MAIN_PART"].ToString() : string.Empty;

            Hashtable integerProperties = new Hashtable();
            ArrayList integerReportProperties = new ArrayList { "NUMBER", "MODEL_TOTAL" };
            pecaChild.GetIntegerReportProperties(integerReportProperties, ref integerProperties);
            _number = integerProperties.ContainsKey("NUMBER") ? int.Parse(integerProperties["NUMBER"].ToString()) : 1;
            _modelTotalQtd = integerProperties.ContainsKey("MODEL_TOTAL") ? int.Parse(integerProperties["MODEL_TOTAL"].ToString()) : 1;

            _quantidade = contarPartesNoAssembly(pecaChild.GetAssembly(), _partPos); 
        }

        public int contarPartesNoAssembly(TSM.Assembly assy, string posicaoAlvo)
        {
            if (assy == null || posicaoAlvo == string.Empty)
                return 0;

            int contador = 0;

            // Obtém todos os segundos membros (peças secundárias) do assembly
            //ArrayList filhos = assy.GetSecondaries();

            // Obtém o membro principal (Main Part) do assembly
            var principal = assy.GetMainPart() as TSM.Part;
            var partPosPrincipal = principal.ObterPropriedade("PART_POS").ToString();

            // 1. Verifica se a Main Part é a parte que estamos procurando
            if (principal != null && partPosPrincipal == posicaoAlvo)
            {
                contador++;
            }

            var filhos = assy.GetSecondaries().GetEnumerator();
            while (filhos.MoveNext())
            {
                var filho = filhos.Current as TSM.Part;
                var partPosFilho = filho.ObterPropriedade("PART_POS").ToString();
                if (partPosFilho == posicaoAlvo)
                {
                    contador++;
                }
            }



            return contador;
        }

        //public void AjusteQuantidade(int quantidadeConjuntos, string marcaConjunto)
        //{
          
        //    if (_quantidade != _modelTotalQtd)
        //    {
        //        if (_quantidade < _modelTotalQtd && _modelTotalQtd % quantidadeConjuntos == 0)
        //        {
        //            _quantidade = _modelTotalQtd;
        //        }
        //        else
        //        {
        //            throw new Exception($"Peça {_partPos} com quantidade maior que model_total");
        //        }
        //    }
        //}

        public void IncrementarQuantidade()
        {
            _quantidade++;
        }

        public void MultiplicaQtdConjuntoPorPeca(int quantidadeConjunto)
        {
            _quantidade = quantidadeConjunto * _quantidade;
        }
    }
}
