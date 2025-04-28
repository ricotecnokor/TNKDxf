using netDxf.Blocks;
using netDxf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Extensoes
{
    public static class ExtensoesDxf
    {
        public static Ponto2d ExtMax(this Block block)
        {
            var lines = block.Entities.Where(x => x.Type == EntityType.Line).Select(l => (Line)l).ToList();

            if (!lines.Any()) return null;

            var xStartMax = lines.Max(l => l.StartPoint.X);
            var xEndMax = lines.Max(l => l.EndPoint.X);
            var xMax = xStartMax > xEndMax ? xStartMax : xEndMax;

            var yStartMax = lines.Max(l => l.StartPoint.Y);
            var yEndMax = lines.Max(l => l.EndPoint.Y);
            var yMax = yStartMax > yEndMax ? yStartMax : yEndMax;

            return Ponto2d.CriarSemEscala(xMax, yMax);

        }

        public static Ponto2d ExtMin(this Block block)
        {
            var lines = block.Entities.Where(x => x.Type == EntityType.Line).Select(l => (Line)l).ToList();

            if (!lines.Any()) return null;

            var xStartMin = lines.Min(l => l.StartPoint.X);
            var xEndMin = lines.Min(l => l.EndPoint.X);
            var xMin = xStartMin < xEndMin ? xStartMin : xEndMin;

            var yStartMin = lines.Min(l => l.StartPoint.Y);
            var yEndMin = lines.Min(l => l.EndPoint.Y);
            var yMin = yStartMin > yEndMin ? yStartMin : yEndMin;

            return Ponto2d.CriarSemEscala(xMin, yMin);

        }
    }
}
