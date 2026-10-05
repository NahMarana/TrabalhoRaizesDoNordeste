namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class EstoqueUnidade
    {
        public int Id { get; set; }
        public required int ProdutoId { get; set; }
        public required int EstabelecimentoId { get; set; }
        public required int QtdEstoqueDisponivel { get; set; }
        public required bool VendaDisponivel { get; set; }
        public Produto? Produtos { get; set; }
        public UnidadesEstabelecimento? UnidadesEstabelecimento { get; set; }
        public ICollection<EstoqueMovimentacao>? EstoquesMovimentacao { get; set; }
    }
}
