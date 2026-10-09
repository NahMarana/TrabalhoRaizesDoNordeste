using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PontosDTO
    {
        public int Id { get; set; }
        public required int FidelizacaoId { get; set; }
        public required TipoMovimentoPontos TipoMovimentacaoPontos { get; set; }
        public required decimal Pontos { get; set; }
        public DateTime DataPontos { get; set; } = DateTime.Now;
        public PontosDoPedidoDTO? Pedidos { get; set; }
    }
}


