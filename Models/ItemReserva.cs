using System.ComponentModel.DataAnnotations;

namespace GestaoReservasUni.Models
{
    public class ItemReserva
    {
        public int Id { get; set; }

        // Vínculo com a Reserva mãe
        public int ReservaId { get; set; }
        public Reserva Reserva { get; set; } = default!;

        // Vínculo com o Equipamento escolhido
        public int EquipamentoId { get; set; }
        public Equipamento Equipamento { get; set; } = default!;

        // Caso o professor queira mais de 1 unidade do mesmo item (ex: 2 notebooks)
        [Required]
        [Display(Name = "Quantidade Solicitada")]
        public int Quantidade { get; set; } = 1;
    }
}