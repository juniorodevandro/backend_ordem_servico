namespace WebApi.Models
{
    public class EstoqueDTO
    {
        public string ItemCodigoReferencia { get; set; } = string.Empty;

        public string ItemNome { get; set; } = string.Empty;
     
        public decimal Quantidade { get; set; }
    }
}
