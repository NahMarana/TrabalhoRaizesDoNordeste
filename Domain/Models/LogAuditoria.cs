namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class LogAuditoria
    {
        public int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required string Acao { get; set; }
        public required string EntidadeAfetada { get; set; }
        public required int EntidadeAfetadaId { get; set; }
        public required string DadosNovos { get; set; }
        public required string DadosAnteriores { get; set; }
        public DateTime DataOrigem { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
