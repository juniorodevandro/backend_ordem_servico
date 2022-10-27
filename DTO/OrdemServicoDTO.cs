namespace WebApi.Models
{
    public class OrdemServicoDTO
    {
        public int Codigo { get; set; }

        public int CodigoOrdem { get; set; }

        public string ItemCodigoReferencia { get; set; } = string.Empty;

        public decimal Quantidade { get; set; }

        public decimal ValorUnitario { get; set; }

        public string? Observacao { get; set; }
    }
}
