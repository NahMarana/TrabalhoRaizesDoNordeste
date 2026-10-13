using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class ItensDoPedidoDTO
    {
        public int Id { get; set; }
        public required int QtdItens { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal { get; set; }
        public PedidoItemDTO? Pedidos { get; set; }
        public ProdutoItemDTO? Produtos { get; set; }
    }
}
