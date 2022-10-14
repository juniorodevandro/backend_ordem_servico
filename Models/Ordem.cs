﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Models
{
    public class Ordem
    {
        [Key]
        public Guid Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        [Required(ErrorMessage = "O campo 'Cliente' é obrigatório")]
        public Pessoa? ClientePessoa { get; set; }

        [Required(ErrorMessage = "O campo 'Responsável' é obrigatório")]
        public Pessoa? ResponsavelPessoa { get; set; }

    }
}
