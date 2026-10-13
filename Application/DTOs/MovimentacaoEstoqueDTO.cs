using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class MovimentacaoEstoqueDTO
    {
        public int Id { get; set; }
        public required int QtdEmEstoque { get; set; }
        public required TipoMovimento TipoMovimento { get; set; }
    }
}
