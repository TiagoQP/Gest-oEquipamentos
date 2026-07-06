using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestaoReservasUni.Models
{
    public class Reserva
    {
        public int Id { get; set; }
    

        [Required]
        [Display(Name = "ID do Assistente")]
        public int AssistenteId { get; set; }

        [Required]
        [Display(Name = "Professor")]
        public string NomeProfessorBeneficiario { get; set; } = string.Empty;

        [Required]
        public string Sala { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Data { get; set; }

        [Required]
        [Display(Name = "Período")]
        public string Periodo { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Pendente";

        // NOVA PROPRIEDADE: Uma reserva agora tem uma lista de vários itens/equipamentos
        public List<ItemReserva> ItensReserva { get; set; } = new List<ItemReserva>();
    }
}