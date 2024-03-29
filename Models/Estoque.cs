﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApi.Models
{
    [Table("Estoque")]
    public class Estoque
    {
        [Key]
        [JsonIgnore]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo 'Item' é obrigatório")]
        public Item? Item { get; set; }

        [Required(ErrorMessage = "O campo 'Quantidade' é obrigatório")]
        public decimal Quantidade { get; set; }
    }
}
