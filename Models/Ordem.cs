using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Ordem
    {
        [Key]
        public Guid Id { get; set; }

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

        [Required(ErrorMessage = "O campo 'Cliente' é obrigatório")]
        public Pessoa? Cliente { get; set; }

        [Required(ErrorMessage = "O campo 'Responsável' é obrigatório")]
        public Pessoa? Responsavel { get; set; }

        public int Desconto { get; set; }

        public string? Observacao { get; set; }
    }
}
