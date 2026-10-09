using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PontosFidDTO
    {
        public int Id { get; set; }
        public required TipoMovimentoPontos TipoMovimentacaoPontos { get; set; }
        public required decimal Pontos { get; set; }
    }
}
