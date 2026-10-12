using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class PagamentoPayloadDTO
    {

        //Dados do retorno do payload
        public required string IdTransacaoExterna { get; set; }
        public required string CanalDoPedido { get; set; }
        public required string TipoPagamento { get; set; }
        public required decimal ValorPagamento { get; set; }
        public required string StatusPagamento { get; set; }
        public required string Codigo { get; set; }
        public required string Mensagem { get; set; }
        public DateTime DataConfirmaPg { get; set; } = DateTime.Now;
        public required int PedidoId { get; set; }

    }
}
