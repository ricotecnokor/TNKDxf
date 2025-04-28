using TNKDxf.Dominio.Listas;

namespace TNKDxf.Dominio.Coletores
{
    public interface IColetaRevisoes : IColeta
    {
        void Coletar(IRevisoesExtraidas revs);

        int QtdConjuntos { get; }
    }
}
