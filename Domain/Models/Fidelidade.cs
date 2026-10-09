namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Fidelidade
    {
        public int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required decimal QtdPontos { get; set; } = 0;
        public Usuario? Usuarios { get; set; }
        public ICollection<PontosFidelidade>? PontosFidelidade { get; set; }

        public decimal ConsultarSaldoPontos()
        {
            return QtdPontos;
        }
    }
}
