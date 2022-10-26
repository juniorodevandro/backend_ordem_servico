namespace WebApi.Models
{
    public class OrdemServicoDTO
    {        
        public int ItemCodigo { get; set; }
        
        public decimal Quantidade { get; set; }

        public decimal ValorUnitario { get; set; }

        public string? Observacao { get; set; }
    }
}
