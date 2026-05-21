using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTNKDxf.EstruturaDxf
{
    public struct Ponto2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Ponto2D(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
