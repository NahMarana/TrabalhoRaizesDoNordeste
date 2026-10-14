using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class MovimentacaoDTO
    {
        public int Id { get; set; }
        public required int PedidoId { get; set; }
        public required int UsuarioUsadoId { get; set; }
        public required int QtdEmEstoque { get; set; }
        public required TipoMovimento TipoMovimento { get; set; }
        public required string MotivoMovimentacao { get; set; }
        public DateTime DataMovimentacao { get; set; } = DateTime.Now;
        public MovEstoqueDTO? EstoqueUnidade { get; set; }
    }
}
