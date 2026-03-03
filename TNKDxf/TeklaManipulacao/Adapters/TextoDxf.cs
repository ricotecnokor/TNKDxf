using netDxf;
using netDxf.Entities;
using netDxf.Tables;
using TSD = Tekla.Structures.Drawing;
using TSG = Tekla.Structures.Geometry3d;

namespace TNKDxf.TeklaManipulacao.Adapters
{
    public class TextoDxf : ObjetosDxf
    {
        private DxfDocument _doc;

        public TextoDxf(DxfDocument doc)
        {
            _doc = doc;
        }

        public void Processar(TSD.Text texto, TSD.View view)
        {
            // 1. Transforma o ponto de inserção para as coordenadas da folha
            TSG.Point pontoTransformado = transformarPonto(texto.InsertionPoint, view);

            // 2. Cria a entidade de texto no netDxf 3.0.1
            // O conteúdo vem da propriedade Contents do Tekla
            Text dxfText = new Text(
                texto.TextString,
                new netDxf.Vector3(pontoTransformado.X, pontoTransformado.Y, 0),
                texto.Attributes.Font.Height,
                TextStyle.Default // Mantém a altura definida no desenho
            );

            // 3. Ajusta o ângulo se o texto estiver rotacionado
            dxfText.Rotation = texto.Attributes.Angle;

            _doc.Entities.Add(dxfText);
        }
    }
}