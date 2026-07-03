namespace GestaoReservasUni.Models
{
    public class Equipamento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty; // Data Show, CPU, Amplificador, etc.
        public int QuantidadeTotal { get; set; }
        public int QuantidadeDisponivel { get; set; }
    }
}