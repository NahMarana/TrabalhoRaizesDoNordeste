using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PgPedidoDTO
    {
        public int Id { get; set; }
        public required decimal ValorPagamento { get; set; }
        public required string TipoPagamento { get; set; } = "";
        public string StatusPagamento { get; set; } = "";
        public string? IdTransacaoExterna { get; set; }
        public string? RespostaPayload { get; set; }
        public DateTime DataPagamento { get; set; } = DateTime.Now;
    }
}
