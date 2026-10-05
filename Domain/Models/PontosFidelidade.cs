using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class PontosFidelidade
    {
        public int Id { get; set; }
        public required int FidelizacaoId { get; set; }
        public required int PedidoId { get; set; }
        public required TipoMovimentacaoPontos TipoMovimentacaoPontos { get; set; }
        public required decimal Pontos { get; set; }
        public DateTime DataPontos { get; set; } = DateTime.Now;
        public Fidelidade? Fidelidade { get; set; }
        public Pedidos? Pedidos { get; set; }
    }
}
