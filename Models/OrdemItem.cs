using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    [Table("OrdemItem")]
    public class OrdemItem
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "O campo 'Código' é obrigatório")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O campo 'Ordem de serviço' é obrigatório")]
        public Ordem? Ordem { get; set; }

        [Required]
        public int OrdemCodigo { get; set; }

        [Required(ErrorMessage = "O campo 'Item' é obrigatório")]
        public Item? Item { get; set; }

        [Required]
        public int ItemCodigo { get; set; }

        [Required(ErrorMessage = "O campo 'Quantidade' é obrigatório")]
        public decimal Quantidade { get; set; }

        public decimal ValorUnitario { get; set; }

        public string Observacao { get; set; }
    }
}
