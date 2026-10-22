using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PagamentoPayloadDTO
    {

        //Dados do retorno do payload
        public required string IdTransacaoExterna { get; set; }
        public required CanalPedido CanalDoPedido { get; set; }
        public required TipoPagamento TipoPagamento { get; set; }
        public required decimal ValorPagamento { get; set; }
        public required StatusPagamento StatusPagamento { get; set; }
        public decimal PontosAcumulados { get; set; }
        public decimal PontosResgatados { get; set; }
        public required string Codigo { get; set; }
        public required string Mensagem { get; set; }
        public DateTime DataConfirmaPg { get; set; } = DateTime.Now;

    }
}
