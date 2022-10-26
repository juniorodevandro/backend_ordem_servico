using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    [Table("Ordem")]
    public class Ordem
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo 'Código' é obrigatório")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O campo 'Data' é obrigatório")]
        public DateTime Data { get; set; }

        public decimal ValorLiquido { get; set; }

        public decimal ValorBruto { get; set; }

        public decimal ValorItem { get; set; }

        public decimal ValorServico { get; set; }

        public decimal QuantidadeItem { get; set; }

        public decimal QuantidadeServico { get; set; }

        public int Desconto { get; set; }

        [Required(ErrorMessage = "O campo 'Tipo' é obrigatório")]
        public string? Tipo { get; set; }

        public string? Observacao { get; set; }

        [Required(ErrorMessage = "O campo 'Cliente' é obrigatório")]
        public Pessoa? Cliente { get; set; }

        public Guid ClienteId { get; set; }

        //[Required(ErrorMessage = "O campo 'Responsável' é obrigatório")]
        //public Pessoa? Responsavel { get; set; }

        //public Guid ResponsavelId { get; set; }

        public List<OrdemItem>? OrdemItem { get; set; }

        public List<OrdemServico>? OrdemServico { get; set; }
    }
}
