using TNKDxf.Dominio.Listas;

namespace TNKDxf.Dominio.Coletores
{
    public interface IColetaLista : IColeta
    {
        void Coletar(ILmExtraida lm);
        int QtdConjuntos { get; }

    }
}
