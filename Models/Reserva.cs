using System;

namespace GestaoReservasUni.Models
{
    public enum StatusSolicitacao { Reservado, EmUso, ProntoParaRetirada, Concluido }

    public class Reserva
    {
        public int Id { get; set; }
        public int AssistenteId { get; set; } 
        public string NomeProfessorBeneficiario { get; set; } = string.Empty; 
        public string Sala { get; set; } = string.Empty; 
        
        // Novas propriedades para vincular ao Equipamento sob demanda:
        public int EquipamentoId { get; set; }
        public Equipamento? Equipamento { get; set; }

        public DateTime Data { get; set; }
        public string Periodo { get; set; } = string.Empty; // Ex: "Noite - 1º Tempo"
        public StatusSolicitacao Status { get; set; } = StatusSolicitacao.Reservado;
    }
}