using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class PromocoesCampanhas
    {
        public required int Id { get; set; }
        public required int EstabelecimentoId { get; set; }
        public required int ProdutoId { get; set; }
        public string? Descricao { get; set; }
        public required decimal ValorPromocional { get; set; }
        public required StatusPromoCampanha StatusPromoCampanha { get; set; }
        public DateOnly DataInicio { get; set; }
        public DateOnly DataFim { get; set; }
    }
}
