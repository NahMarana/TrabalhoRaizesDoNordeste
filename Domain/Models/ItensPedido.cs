namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class ItensPedido
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public required int QtdItens { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal PrecoTotal { get; set; }
    }
}
