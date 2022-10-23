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

        [JsonIgnore]
        [Required(ErrorMessage = "O campo 'Código' é obrigatório")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O campo 'Data' é obrigatório")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "O campo 'Valor líquido' é obrigatório")]
        public decimal ValorLiquido { get; set; }

        [Required(ErrorMessage = "O campo 'Valor bruto' é obrigatório")]
        public decimal ValorBruto { get; set; }

        [Required(ErrorMessage = "O campo 'Tipo' é obrigatório")]
        public int Tipo { get; set; }

        public int Desconto { get; set; }

        public string? Observacao { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "O campo 'Cliente' é obrigatório")]
        public Pessoa? Cliente { get; set; }

        [Required]
        public int ClienteCodigo { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "O campo 'Responsável' é obrigatório")]
        public Pessoa? Reponsavel { get; set; }

        [Required]
        public int ResponsavelCodigo { get; set; }

    }
}
