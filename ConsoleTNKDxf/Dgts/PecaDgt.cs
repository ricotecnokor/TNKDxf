using System;
using System.Collections;
using System.Globalization;
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

        private int _quantidade;

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
            Hashtable doubleProperties = new Hashtable();
            Hashtable stringProterties = new Hashtable();
            ArrayList doubleReportProperties = new ArrayList { "HEIGHT", "LENGTH", "WIDTH", "PROFILE.DIAMETER", "PROFILE.PLATE_THICKNESS", "PROFILE.WEIGHT_PER_UNIT_LENGTH", "WEIGHT_NET", "WEIGHT_GROSS", "WEIGHT_M", "WEIGTH" };
            pecaChild.GetDoubleReportProperties(doubleReportProperties, ref doubleProperties);
            _height = doubleProperties.ContainsKey("HEIGHT") ? double.Parse(doubleProperties["HEIGHT"].ToString(), CultureInfo.InvariantCulture) : 0.0;
            _length = doubleProperties.ContainsKey("LENGTH") ? double.Parse(doubleProperties["LENGTH"].ToString(), CultureInfo.InvariantCulture) : 0.0;
            _width = doubleProperties.ContainsKey("WIDTH") ? double.Parse(doubleProperties["WIDTH"].ToString(), CultureInfo.InvariantCulture) : 0.0;
            _profileDiameter = doubleProperties.ContainsKey("PROFILE.DIAMETER") ? double.Parse(doubleProperties["PROFILE.DIAMETER"].ToString(), CultureInfo.InvariantCulture) : 0.0;
            _profilePlateThickness = doubleProperties.ContainsKey("PROFILE.PLATE_THICKNESS") ? double.Parse(doubleProperties["PROFILE.PLATE_THICKNESS"].ToString(), CultureInfo.InvariantCulture    ) : 0.0;
            _profileWeightPerUnitLength = doubleProperties.ContainsKey("PROFILE.WEIGHT_PER_UNIT_LENGTH") ? double.Parse(doubleProperties["PROFILE.WEIGHT_PER_UNIT_LENGTH"].ToString(), CultureInfo.InvariantCulture) : 0.0;
            _weightNet = doubleProperties.ContainsKey("WEIGHT_NET") ? double.Parse(doubleProperties["WEIGHT_NET"].ToString(), CultureInfo.InvariantCulture) : 0.0;
            _weightGross = doubleProperties.ContainsKey("WEIGHT_GROSS") ? double.Parse(doubleProperties["WEIGHT_GROSS"].ToString(), CultureInfo.InvariantCulture) : 0.0;
            _weightM = doubleProperties.ContainsKey("WEIGHT_M") ? double.Parse(doubleProperties["WEIGHT_M"].ToString(), CultureInfo.InvariantCulture) : 0.0;

            ArrayList stringReportProperties = new ArrayList { "PART_POS", "FINISH", "PROFILE", "PROFILE_TYPE", "MATERIAL", "MAIN_PART" };
            pecaChild.GetStringReportProperties(stringReportProperties, ref stringProterties);
            _partPos = stringProterties.ContainsKey("PART_POS") ? stringProterties["PART_POS"].ToString() : string.Empty;
            _finish = stringProterties.ContainsKey("FINISH") ? stringProterties["FINISH"].ToString() : string.Empty;
            _profile = stringProterties.ContainsKey("PROFILE") ? stringProterties["PROFILE"].ToString() : string.Empty;
            _profileType = stringProterties.ContainsKey("PROFILE_TYPE") ? stringProterties["PROFILE_TYPE"].ToString() : string.Empty;
            _material = stringProterties.ContainsKey("MATERIAL") ? stringProterties["MATERIAL"].ToString() : string.Empty;
            var mainPart = stringProterties.ContainsKey("MAIN_PART") ? stringProterties["MAIN_PART"].ToString() : string.Empty;

            _quantidade = 1;
        }

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
