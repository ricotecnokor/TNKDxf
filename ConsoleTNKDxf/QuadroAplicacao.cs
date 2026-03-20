using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.DrawingInternal;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class QuadroAplicacao
    {
        private string _tag;
        private string _desenho;
        private string _desenhoCliente;
        private string _familia;

        public string Tag => _tag;
        public string Desenho => _desenho;
        public string DesenhoCliente => _desenhoCliente;
        public string Familia => _familia;

        public QuadroAplicacao(MultiDrawing drawing)
        {
            List<string> userProperties = new List<string> { "TCNK_TAG", "TCNK_N_TCNK", "TCNK_N_CLIENTE" };
            Dictionary<string, string> properties = new Dictionary<string, string>();
            drawing.GetStringUserProperties(userProperties, out properties);

            _tag = properties.ContainsKey("TCNK_TAG") ? properties["TCNK_TAG"] : string.Empty;
            _desenho = properties.ContainsKey("TCNK_N_TCNK") ? properties["TCNK_N_TCNK"] : string.Empty;
            _desenhoCliente = properties.ContainsKey("TCNK_N_CLIENTE") ? properties["TCNK_N_CLIENTE"] : string.Empty;

            drawing.GetIntegerUserProperties(new List<string> { "TCNK_FAMILIA" }, out Dictionary<string, int> intProperties);
            _familia = intProperties.ContainsKey("TCNK_FAMILIA") ? intProperties["TCNK_FAMILIA"].ToString() : "0";

        }
    }
}
