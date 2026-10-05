using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required int EstabelecimentoId { get; set; }
        public required CanalPedido CanalPedido { get; set; }
        public required ModoReceber ModoReceber { get; set; }
        public required StatusPedido StatusPedido { get; set; }
        public required decimal ValorTotalPedido { get; set; }
        public DateTime DataHoraPedido { get; set; } = DateTime.Now;
        public string? Descricao { get; set; }
        public Usuario? Usuarios { get; set; }
        public UnidadesEstabelecimento? UnidadesEstabelecimento { get; set; }
        public ICollection<EstoqueMovimentacao>? EstoquesMovimentacao { get; set; }
        public ICollection<ItensPedido>? ItensPedidos { get; set; }
        public Pagamento? Pagamentos { get; set; }
        public ICollection<PontosFidelidade>? PontosFidelidades { get; set; }
    }
}
