using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Item
    {
        [Key]
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "O campo 'Código' é obrigatório")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "O campo 'Tipo' é obrigatório")]
        public int Tipo { get; set; }

        [Required(ErrorMessage = "O campo 'Cliente' é obrigatório")]
        public Pessoa Cliente { get; set; }

        public string Observacao { get; set; }

        public decimal ValorUnitario { get; set; }        
    }
}
