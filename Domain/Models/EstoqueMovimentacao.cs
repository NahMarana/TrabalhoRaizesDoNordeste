using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class EstoqueMovimentacao
    {
        public int Id { get; set; }
        public required int EstoqueId { get; set; }
        public required int PedidoId { get; set; }
        public required int UsuarioUsadoId { get; set; }
        public required int QtdEmEstoque { get; set; }
        public required TipoMovimento TipoMovimento { get; set; }
        public required string MotivoMovimentacao { get; set; }
        public DateTime DataMovimentacao { get; set; } = DateTime.Now;

    }
}
