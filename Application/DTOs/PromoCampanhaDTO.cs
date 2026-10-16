using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PromoCampanhaDTO
    {
        public int Id { get; set; }
        public string? Descricao { get; set; }
        public required decimal ValorPromocional { get; set; }
        public required StatusPromoCampanha StatusPromoCampanha { get; set; }
        public DateOnly DataInicio { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly DataFim { get; set; }
        public ProdutoDTO? Produtos { get; set; }
        public EstabelecimentoDTO? UnidadesEstabelecimento { get; set; }
    }
}
