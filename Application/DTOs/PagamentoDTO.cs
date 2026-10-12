using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PagamentoDTO
    {
        public int Id { get; set; }
        public required TipoPagamento TipoPagamento { get; set; }
        public required StatusPagamento StatusPagamento { get; set; }
        public required decimal ValorPagamento { get; set; }
        public DateTime DataPagamento { get; set; } = DateTime.Now;
        public DateTime DataConfirmaPg { get; set; }
        public string? IdTransacaoExterna { get; set; }
        public string? RespostaPayload { get; set; }
        public decimal PontosResgatados { get; set; }
        public PedidoPagamentoDTO? Pedidos { get; set; }
    }
}
