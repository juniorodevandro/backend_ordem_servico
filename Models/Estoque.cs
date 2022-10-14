using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
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
