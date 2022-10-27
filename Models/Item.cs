using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    [Table("Item")]
    public class Item
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo 'Código' é obrigatório")]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O campo 'Código de referência' é obrigatório")]
        public string CodigoReferencia { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Nome' é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo 'Tipo' é obrigatório")]
        public string? Tipo { get; set; }

        public string? Observacao { get; set; }

        [JsonIgnore]
        public List<Estoque> Estoque { get; set; } = new List<Estoque>();
    }
}
