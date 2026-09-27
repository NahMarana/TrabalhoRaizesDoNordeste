using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Pedidos
    {
        public required int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required int EstabelecimentoId { get; set; }
        public required CanalPedido CanalPedido { get; set; }
        public required ModoReceber ModoReceber { get; set; }
        public required StatusPedido StatusPedido { get; set; }
        public required decimal ValorTotalPedido { get; set; }
        public required DateTime DataHoraPedido { get; set; }
        public required string Descricao { get; set; }

    }
}
