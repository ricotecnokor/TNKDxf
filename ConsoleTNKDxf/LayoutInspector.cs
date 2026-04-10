using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace ConsoleTNKDxf
{
    public class LayoutInspector
    {
        /// <summary>
        /// Detecta se o desenho atual utiliza o template DIAGRAMA_LM.tpl dentro do seu layout.
        /// </summary>
        public bool IsDiagramaDrawing(Drawing drawing)
        {
            if (drawing == null) return false;

            // O layout definido no arquivo .lay é instanciado no Sheet (Folha)
            // do desenho. Vamos percorrer todos os objetos da folha.
            DrawingObjectEnumerator sheetObjects = drawing.GetSheet().GetAllObjects();

            var lista = sheetObjects.GetEnumerator();

            while (lista.MoveNext())
            {
                DrawingObject obj = sheetObjects.Current;
                //var obj = sheetObjects.Current as Plugin;

                if (obj != null)
                {
                    string currentTemplateFile = string.Empty;

                    PropertyInfo propInfo = obj.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
                    object value = propInfo.GetValue(obj, null);
                    Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

                    Beam tempBeam = new Beam();
                    tempBeam.Identifier = identifier;

       
                    // O Tekla armazena o nome do arquivo .tpl nesta propriedade de relatório
                    // mesmo que o objeto seja um Plugin ou uma representação gráfica
                    if (tempBeam.GetReportProperty("PADRÃO ARAUCO", ref currentTemplateFile))
                    {
                        // Verifica se o nome do arquivo contém o seu DIAGRAMA_LM
                        if (!string.IsNullOrEmpty(currentTemplateFile) &&
                            currentTemplateFile.IndexOf("DIAGRAMA_LM", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void pega(Plugin layoutPlugin)
        {
            string name = "";
            PropertyInfo propInfo = layoutPlugin.GetType().GetProperty("Identifier",
                                        BindingFlags.Instance | BindingFlags.NonPublic);
            object value = propInfo.GetValue(layoutPlugin, null);
            Tekla.Structures.Identifier identifier = (Tekla.Structures.Identifier)value;

            Beam tempBeam = new Beam();
            tempBeam.Identifier = identifier;

            tempBeam.GetReportProperty("TITLE1", ref name);
         

        }
    }
}
