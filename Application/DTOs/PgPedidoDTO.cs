using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PgPedidoDTO
    {
        public int Id { get; set; }
        public required decimal ValorPagamento { get; set; }
        public required TipoPagamento TipoPagamento { get; set; }
        public DateTime DataPagamento { get; set; } = DateTime.Now;
    }
}
