using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    [Table("Pessoa")]
    public class Pessoa
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "O campo 'Código' é obrigatório")]
        public int Codigo { get; set; }

        [StringLength(150, ErrorMessage = "O campo nome deve ter no máximo 150 caracteres", MinimumLength = 4)]
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório")]
        public string Nome { get; set; }

        [StringLength(11, ErrorMessage = "O campo 'CPF' deve conter 11 caracteres", MinimumLength = 11)]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo 'Tipo' é obrigatório")]
        public int Tipo { get; set; }

        public string Contato { get; set; }

        public string Endereco { get; set; }
    }
}
