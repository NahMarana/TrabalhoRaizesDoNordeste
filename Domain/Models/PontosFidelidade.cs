using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class PontosFidelidade
    {
        public required int Id { get; set; }
        public required int FidelizacaoId { get; set; }
        public required int PedidoId { get; set; }
        public required TipoMovimentacaoPontos TipoMovimentacaoPontos { get; set; }
        public required decimal Pontos { get; set; }
        public required DateTime DataPontos { get; set; }
    }
}
