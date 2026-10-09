namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PontosDoPedidoDTO
    {
        public int Id { get; set; }
        public required int UsuarioId { get; set; }
        public required decimal ValorTotalPedido { get; set; }
    }
}
