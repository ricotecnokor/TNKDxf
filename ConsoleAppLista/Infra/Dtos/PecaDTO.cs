namespace ConsoleAppLista.Infra.Dtos
{
    public class PecaDTO
    {
        public string PartNumber { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; } // Ex: Produto, Conjunto, etc.
        public string Material { get; set; } // Material da peça
        public double Peso { get; set; } // Peso da peça em kg
        public double Comprimento { get; set; } // Comprimento da peça em mm
        public double Largura { get; set; } // Largura da peça em mm
        public double Altura { get; set; } // Altura da peça em mm
    }
}
