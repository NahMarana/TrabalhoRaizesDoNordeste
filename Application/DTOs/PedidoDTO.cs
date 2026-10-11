using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public DateTime DataHoraPedido { get; set; } = DateTime.Now;
        public string? Descricao { get; set; }
        public required decimal ValorTotalPedido { get; set; }
        public required CanalPedido CanalPedido { get; set; }
        public required ModoReceber ModoReceber { get; set; }
        public required StatusPedido StatusPedido { get; set; }
        public UsuarioPedidoDTO? Usuarios { get; set; }
        public ICollection<ItensPedidoDTO>? ItensPedidos { get; set; }
        public PgPedidoDTO? Pagamentos { get; set; }

    }
}
