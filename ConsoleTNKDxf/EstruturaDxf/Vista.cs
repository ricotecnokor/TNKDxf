using System.Collections.Generic;

namespace ConsoleTNKDxf.EstruturaDxf
{
    public class Vista
    {
        List<Linha> _linhas = new List<Linha>();
        List<Cota> _cotas = new List<Cota>();
        List<Marca> _marcas = new List<Marca>();

        List<PecaTekla> _pecas = new List<PecaTekla>();
        public List<Linha> Linhas => _linhas;
        public List<Cota> Cotas => _cotas;
        public List<Marca> Marcas => _marcas;
        public List<PecaTekla> Pecas => _pecas;

        public void AddLinha(Linha linha)
        {
            _linhas.Add(linha);
        }

        public void AddCota(Cota cota)
        {
            _cotas.Add(cota);
        }

        public void AddMarca(Marca marca)
        {
            _marcas.Add(marca);
        }

        public void AddPeca(PecaTekla peca)
        {
            _pecas.Add(peca);
        }
    }
}
