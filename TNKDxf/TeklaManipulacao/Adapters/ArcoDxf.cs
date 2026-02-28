using netDxf;
using netDxf.Entities;
using System;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class ArcoDxf : ObjetosDxf
    {
        private DxfDocument _doc;

        public ArcoDxf(DxfDocument doc)
        {
            _doc = doc;
        }

        public void Processar(TSD.Arc arco, TSD.View view)
        {
            // 1. Transforma os pontos das extremidades do sistema da vista para a folha
            TSG.Point pInício = transformarPonto(arco.StartPoint, view);
            TSG.Point pFim = transformarPonto(arco.EndPoint, view);
            double raio = arco.Radius;

            // 2. Cálculo do Centro do Arco
            // No Tekla, o arco é definido por pontos e raio. Precisamos achar o centro geométrico.
            TSG.Point centro = calcularCentro(pInício, pFim, raio);

            // 3. Cálculo dos ângulos (O DXF exige ângulos em graus em relação ao eixo X)
            double anguloInicio = Math.Atan2(pInício.Y - centro.Y, pInício.X - centro.X) * (180.0 / Math.PI);
            double anguloFim = Math.Atan2(pFim.Y - centro.Y, pFim.X - centro.X) * (180.0 / Math.PI);

            // 4. Criação da entidade no netDxf 3.0.1
            // O netDxf usa Vector3 para o centro e double para ângulos
            Arc dxfArc = new Arc(
                new Vector3(centro.X, centro.Y, 0),
                raio,
                anguloInicio,
                anguloFim
            );

            _doc.Entities.Add(dxfArc);
        }

        private TSG.Point calcularCentro(TSG.Point p1, TSG.Point p2, double r)
        {
            // Lógica simplificada para encontrar o centro de um arco dados dois pontos e o raio
            double d2 = (p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y);
            double d = Math.Sqrt(d2);

            // h é a distância do ponto médio da corda até o centro do círculo
            double h = Math.Sqrt(Math.Max(0, r * r - d2 / 4));

            double midX = (p1.X + p2.X) / 2;
            double midY = (p1.Y + p2.Y) / 2;

            // Nota: Existem dois possíveis centros. O Tekla geralmente assume o arco menor ou 
            // baseia-se na ordem dos pontos. Aqui calculamos uma das soluções:
            double centroX = midX + h * (p1.Y - p2.Y) / d;
            double centroY = midY - h * (p1.X - p2.X) / d;

            return new TSG.Point(centroX, centroY, 0);
        }
    }
}