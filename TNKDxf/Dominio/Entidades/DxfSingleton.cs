using netDxf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNKDxf.Dominio.Entidades
{
    public class DxfSingleton
    {
        private static DxfDocument _dxfDocument;

        public static DxfDocument DxfDocument { get => _dxfDocument; set => _dxfDocument = value; }

        public static void Load(string filePath)
        {
            if (_dxfDocument == null)
            {
                _dxfDocument = DxfDocument.Load(filePath);
            }
        }
    }
}
