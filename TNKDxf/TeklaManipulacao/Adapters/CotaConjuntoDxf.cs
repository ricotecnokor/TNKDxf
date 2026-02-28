using netDxf;
using netDxf.Entities;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class CotaConjuntoDxf : CotaDxf
    {
        public CotaConjuntoDxf(DxfDocument doc) : base(doc) { }

        public void Processar(TSD.StraightDimensionSet conjunto, TSD.View view)
        {
            // Na ausência de acesso direto aos pontos, pegamos os objetos que compõem o conjunto
            var objetosInternos = conjunto.GetObjects();

            while (objetosInternos.MoveNext())
            {
                // Cada segmento de um conjunto costuma ser uma StraightDimension individual
                if (objetosInternos.Current is TSD.StraightDimension cotaIndividual)
                {
                    // Transforma os pontos da cota individual (Start/End) para a folha
                    TSG.Point p1 = transformarPonto(cotaIndividual.StartPoint, view);
                    TSG.Point p2 = transformarPonto(cotaIndividual.EndPoint, view);

                    // Criamos o LinearDimension para este segmento específico no netDxf 3.0.1
                    var dxfDim = new LinearDimension(
                        new Vector2(p1.X, p1.Y),
                        new Vector2(p2.X, p2.Y),
                        conjunto.Distance, // Usamos a distância do conjunto pai
                        0
                    );

                    ConfigurarEstilo(dxfDim, conjunto);
                    _doc.Entities.Add(dxfDim);
                }
            }
        }
    }
}