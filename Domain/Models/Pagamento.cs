using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Pagamento
    {
        public int Id { get; set; }
        public required int PedidoId { get; set; }
        public required TipoPagamento TipoPagamento { get; set; }
        public required StatusPagamento StatusPagamento { get; set; }
        public required decimal ValorPagamento { get; set; }
        public DateTime DataPagamento { get; set; } = DateTime.Now;
        public DateTime DataConfirmaPg { get; set; }
        public string? IdTransacaoExterna { get; set; }
        public string? RespostaPayload { get; set; }
        public Pedido? Pedidos { get; set; }


        public const decimal ValorPorPontoGanho = 10m;
        public const decimal ValorPorPonto = 1m;

        //Valor por ponto: A cada R$ 10.00 reais gastos, o Cliente ganha 1 ponto.
        public decimal CalculaPontosGanhos()
        {
            return Math.Floor(ValorPagamento / ValorPorPontoGanho);
        }

        //Quando usar o "Resgate" dos pontos, irá usar todo o saldo. O saldo só será limitado ao valor do pedido.
        public void AplicarResgate(decimal valorPedido, decimal saldoDePontos)
        {
            var qtdPontosPrecisos = Math.Floor(valorPedido / ValorPorPonto);
            var pontosUsados = Math.Min(Math.Floor(saldoDePontos), qtdPontosPrecisos);

            ValorPagamento = valorPedido - (pontosUsados * ValorPorPonto);
        }

        //Aqui vai descobrir quantos pontos foram usados, vendo a diferença entre o pedido e o pagamento
        public decimal CalcularPontosResgatados(decimal valorPedido)
        {
            return (valorPedido - ValorPagamento) / ValorPorPonto;
        }
    }
}
