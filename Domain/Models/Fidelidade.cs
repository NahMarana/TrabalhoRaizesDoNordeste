namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Fidelidade
    {
        public required int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required decimal QtdPontos { get; set; }
    }
}
