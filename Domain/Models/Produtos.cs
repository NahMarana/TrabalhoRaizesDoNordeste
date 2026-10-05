namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Produtos
    {
        public int Id { get; set; }
        public required string NomeProduto { get; set; }
        public required string DescricaoProduto { get; set; }
        public required decimal PrecoUnitario { get; set; }
        public required bool ProdutoSazonal { get; set; }
        public DateOnly DataInicioSazonal { get; set; }
        public DateOnly DataFimSazonal { get; set; }
        public bool ProdutoAtivo { get; set; }
        public int CategoriaId { get; set; }
        public Categorias? Categoria { get; set; }
        public ICollection<EstoqueUnidade>? EstoqueUnidades { get; set; }
        public ICollection<ItensPedido>? ItensPedidos { get; set; }
        public ICollection<PromocoesCampanhas>? PromocoesCampanhas { get; set; }

    }
}
