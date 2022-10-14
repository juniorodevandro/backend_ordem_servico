using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Pessoa
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo 'Código' é obrigatório")]
        public int Codigo { get; set; }

        [StringLength(150, ErrorMessage = "O campo nome deve ter no máximo 150 caracteres", MinimumLength = 4)]
        [Required(ErrorMessage = "O campo 'Nome' é obrigatório")]
        public string Nome { get; set; }

        [StringLength(11, ErrorMessage = "O campo 'CPF' deve ter no máximo 11 caracteres", MinimumLength = 11)]
        public string Cpf { get; set; }

        public int Tipo { get; set; }

        public string Contato { get; set; }

        public string Endereco { get; set; }
    }
}
