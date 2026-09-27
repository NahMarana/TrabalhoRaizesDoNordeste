using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Pagamentos
    {
        public required int Id { get; set; }
        public required int PedidoId { get; set; }
        public required TipoPagamento TipoPagamento { get; set; }
        public required StatusPagamento StatusPagamento { get; set; }
        public required decimal ValorPagamento { get; set; }
        public required DateTime DataPagamento { get; set; }
        public required DateTime DataConfirmaPg { get; set; }
        public required string IdTransacaoExterna { get; set; }
        public required string RespostaPayload { get; set; }
    }
}
