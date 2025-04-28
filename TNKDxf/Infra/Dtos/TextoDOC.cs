using TNKDxf.Dominio.ObjetosValor;

namespace TNKDxf.Infra.Dtos
{
    public class TextoDOC
    {
        public string Atributo { get; set; }
        public string Campo { get; set; }
        public string Valor { get; set; }
        public Ponto2dDOC PontoInsercao { get; set; }
        public int? IndiceCor { get; set; }

        public Texto Converter()
        {
            return new Texto(Atributo, Campo, Valor,
                Ponto2d.CriarSemEscala(PontoInsercao.X, PontoInsercao.Y), IndiceCor);
        }
    }
}
