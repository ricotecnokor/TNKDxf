using TNKDxf.Dominio.Dxfs;

namespace TNKDxf.Dominio.Abstracoes
{
    public interface IConversorDxf
    {
        void Converter(ArquivoDxf arquivoDxf, string usuario);
    }
}
