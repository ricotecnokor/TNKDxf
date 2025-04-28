
using netDxf.Entities;
using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Dominio.Extensoes
{
    public static class ExtensoesEntidadesCAD
    {

        public static Linha CriaLinha(this Line line, double fatorEscala)
        {
            Ponto2d pontoInicial = Ponto2d.CriarComEscala(line.StartPoint.X, line.StartPoint.Y, fatorEscala);
            Ponto2d pontoFinal = Ponto2d.CriarComEscala(line.EndPoint.X, line.EndPoint.Y, fatorEscala);
            return new Linha(pontoInicial, pontoFinal);
        }

        public static Texto CriarTexto(this Text text, double fatorEscala)
        {
            var valor = text.Value;
            var pontoInsercao = Ponto2d.CriarComEscala(text.Position.X, text.Position.Y, fatorEscala);
            var indiceCor = text.Color.Index;
            return new Texto(valor, valor, valor, pontoInsercao, indiceCor);
        }
    }
}
