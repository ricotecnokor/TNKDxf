using TNKDxf.Dominio.Entidades;

namespace TNKDxf.Dominio.Coletores
{
    public interface IColeta
    {
        void ApagarSelecao();

        Formato Formato { get; }
    }
}
