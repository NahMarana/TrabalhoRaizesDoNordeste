namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class ItensPedido
    {
        public required int Id { get; set; }
        public required int PedidoId { get; set; }
        public required int ProdutoId { get; set; }
        public required int QtdItens { get; set; }
        public required decimal PrecoUnitario { get; set; }
        public required decimal PrecoTotal { get; set; }
    }
}
