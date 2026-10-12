using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PedidoPagamentoDTO
    {
        public int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required int EstabelecimentoId { get; set; }
        public int FidelizacaoId { get; set; }
        public decimal QtdPontos { get; set; }
    }
}
