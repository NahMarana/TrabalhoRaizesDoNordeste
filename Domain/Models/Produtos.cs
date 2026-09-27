namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Produtos
    {
        public required int Id { get; set; }
        public required string NomeProduto { get; set; }
        public required string DescricaoProduto { get; set; }
        public required decimal PrecoUnitario { get; set; }
        public required int CategoriaId { get; set; }
        public required bool ProdutoSazonal { get; set; }
        public required DateOnly DataInicioSazonal { get; set; }
        public required DateOnly DataFimSazonal { get; set; }
        public required bool ProdutoAtivo { get; set; }

    }
}
