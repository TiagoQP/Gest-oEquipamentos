namespace GestaoReservasUni.Models
{
    public enum TipoUsuario 
    { 
        AssistenteChefia, 
        Bedel             
    }

    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public TipoUsuario Tipo { get; set; }
    }
}