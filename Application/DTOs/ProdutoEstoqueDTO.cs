namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class ProdutoEstoqueDTO
    {
        public int Id { get; set; }
        public required string NomeProduto { get; set; }
        public required bool ProdutoSazonal { get; set; }
        public bool ProdutoAtivo { get; set; }
    }
}
