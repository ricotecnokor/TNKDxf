using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public abstract class ObjetosDxf
    {
        protected TSG.Point transformarPonto(TSG.Point pontoLocal, TSD.View view)
        {
            // Obtemos o sistema de coordenadas da vista
            TSG.CoordinateSystem cs = view.ViewCoordinateSystem;

            // Criamos a matriz de transformação manualmente usando a Origem e os Eixos
            // Isso garante compatibilidade total na versão 2024
            TSG.Matrix matriz = TSG.MatrixFactory.FromCoordinateSystem(cs);

            // Transforma o ponto local da vista para o ponto global da folha
            return matriz.Transform(pontoLocal);
        }
    }
}
