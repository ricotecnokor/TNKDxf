using System.Collections.Generic;
using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Dominio.Coletores
{
    public abstract class AbsColeta : IColeta
    {
        protected Dictionary<string, string> _objectIdsToDelete = new Dictionary<string, string>();

        protected Formato _formato;

        public Formato Formato { get => _formato; private set => _formato = value; }

        public AbsColeta(Formato formato)
        {
            _formato = formato;
        }

        public abstract void ApagarSelecao();
        

        //public void ApagarSelecao()
        //{
        //    if (_objectIdsToDelete.Count() > 0)
        //    {
        //        var blocos = _objectIdsToDelete.Where(o => o.Value.Equals("BLOCO")).ToDictionary();
        //        if (blocos.Count() > 0)
        //        {
        //            foreach (var bloco in blocos)
        //            {
        //                var blocoToDelete = DxfSingleton.DxfDocument.Entities.Inserts.FirstOrDefault(i => i.Handle == bloco.Key);
        //                DxfSingleton.DxfDocument.Entities.Remove(blocoToDelete);
        //            }
        //        }
        //        else
        //        {
        //            var linhas = _objectIdsToDelete.Where(o => o.Value.Equals("LINHA")).ToDictionary();
        //            if (linhas.Count() > 0)
        //            {
        //                foreach (var linha in linhas)
        //                {
        //                    var linhaToDelete = DxfSingleton.DxfDocument.Entities.Lines.FirstOrDefault(i => i.Handle == linha.Key);
        //                    DxfSingleton.DxfDocument.Entities.Remove(linhaToDelete);
        //                }
        //            }

        //            var textos = _objectIdsToDelete.Where(o => o.Value.Equals("TEXTO")).ToDictionary();
        //            if (textos.Count() > 0)
        //            {
        //                foreach (var text in textos)
        //                {
        //                    var textToDelete = DxfSingleton.DxfDocument.Entities.Texts.FirstOrDefault(i => i.Handle == text.Key);
        //                    DxfSingleton.DxfDocument.Entities.Remove(textToDelete);
        //                }
        //            }
        //        }


        //    }
        //}


    }
}
