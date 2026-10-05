using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public required int PedidoId { get; set; }
        public required TipoPagamento TipoPagamento { get; set; }
        public required StatusPagamento StatusPagamento { get; set; }
        public required decimal ValorPagamento { get; set; }
        public DateTime DataPagamento { get; set; } = DateTime.Now;
        public DateTime DataConfirmaPg { get; set; }
        public string? IdTransacaoExterna { get; set; }
        public string? RespostaPayload { get; set; }
        public Pedido? Pedidos { get; set; }
    }
}
