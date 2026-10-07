namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class EstoqueUnidadeDTO
    {
        public int Id { get; set; }
        public required int QtdEstoqueDisponivel { get; set; }
        public required int ProdutoId { get; set; }
        public required string NomeProduto { get; set; }
        public string? DescricaoProduto { get; set; }
        public required decimal PrecoUnitario { get; set; }
        public required bool ProdutoSazonal { get; set; }


    }
}
