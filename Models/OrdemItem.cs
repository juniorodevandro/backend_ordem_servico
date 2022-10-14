using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class OrdemItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo 'Código' é obrigatório")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O campo 'Ordem de serviço' é obrigatório")]
        public Ordem? Ordem { get; set; }

        [Required(ErrorMessage = "O campo 'Item' é obrigatório")]
        public Item? Item { get; set; }

        [Required(ErrorMessage = "O campo 'Quantidade' é obrigatório")]
        public decimal Quantidade { get; set; }

        public decimal ValorUnitario { get; set; }

        public string Observacao { get; set; }
    }
}
