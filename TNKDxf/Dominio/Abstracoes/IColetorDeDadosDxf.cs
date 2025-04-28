using System.Collections;
using System.Collections.Generic;
using TNKDxf.Dominio.Dxfs;
using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Dominio.Abstracoes
{
    public interface IColetorDeDadosDxf
    {
        void ApagarSelecao();
        void ColetarDados(ArquivoDxf arquivoDxf);
        
        List<string[]> ObterCamposDoFormato();
        IEnumerable<ConjuntoLM> ObterConjuntos();
        //Hashtable ObterHashTableLinhas(ConjuntoLM conjuntoLM);
        double ObterPesoTotalDaLM();
        List<Dictionary<string, string>> ObterRevisoes();
        double ObterTamanhoDaLista();
    }
}
