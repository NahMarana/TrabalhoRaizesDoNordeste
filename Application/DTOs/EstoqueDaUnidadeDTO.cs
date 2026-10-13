using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class EstoqueDaUnidadeDTO
    {
        public int Id { get; set; }
        public required int QtdEstoqueDisponivel { get; set; }
        public required bool VendaDisponivel { get; set; }
        public ProdutoEstoqueDTO? Produtos { get; set; }
        public EstabelecimentoEstoqueDTO? UnidadesEstabelecimento { get; set; }
        public ICollection<MovimentacaoEstoqueDTO>? EstoquesMovimentacao { get; set; }
    }
}
