using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class LogAuditoriaDTO
    {
        public int Id { get; set; }
        public required string Acao { get; set; }
        public required string EntidadeAfetada { get; set; }
        public required int EntidadeAfetadaId { get; set; }
        public required string DadosNovos { get; set; }
        public required string DadosAnteriores { get; set; }
        public DateTime DataOrigem { get; set; } = DateTime.Now;
        public UsuarioDTO? Usuarios { get; set; }
    }
}
