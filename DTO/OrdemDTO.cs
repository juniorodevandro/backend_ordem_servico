using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class OrdemDTO
    {
        public DateTime Data { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public int Desconto { get; set; }

        public string Observacao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF do 'Cliente' é obrigatório")]
        public string ClienteCPF { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF do 'Responsável' é obrigatório")]
        public string ResponsavelCPF { get; set; } = string.Empty;
        
        public List<OrdemItemDTO>? OrdemItem { get; set; }

        public List<OrdemServicoDTO>? OrdemServico { get; set; }
    }
}
