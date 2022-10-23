using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    [Table("Estoque")]
    public class Estoque
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo 'Item' é obrigatório")]
        public Item Item { get; set; }

        [Required(ErrorMessage = "O campo 'Quantidade' é obrigatório")]
        public decimal Quantidade { get; set; }        
    }
}
