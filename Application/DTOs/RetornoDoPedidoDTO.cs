namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class RetornoDoPedidoDTO
    {
        public int PedidoId { get; set; }
        public string NomeCliente { get; set; } = "";
        public DateTime DataHoraPedido { get; set; }
        public string CanalPedido { get; set; } = "";
        public string ModoReceber { get; set; } = "";
        public string StatusPedido { get; set; } = "";
        public List<ItensPedidoDTO> Itens { get; set; } = new();
        public decimal ValorTotalPedido { get; set; }
        public PgPedidoDTO? Pagamento { get; set; }
    }
}
