using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class PagamentoService(PagamentoRepository repository, PontosFidelidadeService pontosFidelidadeService, PedidoService pedidoService, FidelidadeService fidelidadeService, LogAuditoriaService auditoriaService)
    {
        public PagamentoDTO? BuscaPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var pagamento = repository.BuscaPagamentoPorId(id);

            if (pagamento == null)
            {
                return null;
            }

            var pagamentoDTO = CriarObjeto(pagamento);

            return pagamentoDTO;
        }

        public ICollection<PagamentoDTO> ListarPagamentos()
        {
            var pagamento = repository.ListarPagamentos();
            var pagamentoDTO = CriarObjetoCollection(pagamento);

            return pagamentoDTO;

        }

        public PagamentoPayloadDTO CriarPagamento(Pagamento pagamento, bool usarPontos, bool simularRecusa, int? clienteId = null)
        {
            if (pagamento == null)
            {
                throw new ArgumentNullException("Pagamento inexistente.");
            }

            if (pagamento.PedidoId <= 0)
            {
                throw new ArgumentException("Pedido é obrigatório.");
            }

            if (!Enum.IsDefined(pagamento.TipoPagamento))
            {
                throw new ArgumentException("Modo de escolha de pagamento inválido.");
            }

            var pedido = repository.BuscaPedidoPorId(pagamento.PedidoId);

            if (pedido == null)
            {
                throw new KeyNotFoundException("Pedido não encontrado.");
            }

            if (clienteId != null && pedido.UsuarioId != clienteId)
            {
                throw new UnauthorizedAccessException("Este pedido não pertence a você.");
            }

            if (pedido.Pagamentos != null)
            {
                throw new InvalidOperationException("Este pedido já possui um pagamento.");
            }

            if (pedido.StatusPedido != StatusPedido.PENDENTE)
            {
                throw new InvalidOperationException("Só é possível pagar seu pedido se o status estiver PENDENTE.");
            }

            pagamento.ValorPagamento = pedido.ValorTotalPedido;

            decimal saldo = 0;

            if (usarPontos)
            {
                saldo = ValidarResgate(pedido);
                pagamento.AplicarResgate(pedido.ValorTotalPedido, saldo);
            }

            var mensagemPagamento = "";
            decimal pontosAcumulados = 0;
            decimal pontosResgatados = 0;

            if (TentativaDePagamento(simularRecusa))
            {
                pagamento.StatusPagamento = StatusPagamento.ACEITO;
                pagamento.DataConfirmaPg = DateTime.Now;
                mensagemPagamento = "Pagamento realizado com sucesso.";
                pagamento.RespostaPayload = $"ACEITO - {mensagemPagamento}";

                var pagamentoCriado = repository.CriarPagamento(pagamento)!;

                auditoriaService.RegistrarLog(
                    pedido.UsuarioId,
                    "PAGAMENTO_ACEITO",
                    "Pagamento",
                    pagamentoCriado.Id,
                    "",
                    $"Pedido: {pedido.Id}; Tipo: {pagamentoCriado.TipoPagamento}; Valor: {pagamentoCriado.ValorPagamento}; Transação: {pagamentoCriado.IdTransacaoExterna}");

                pedidoService.AtualizarStatusPedido(pedido.Id, StatusPedido.EMPREPARO);

                var usuario = pedido.Usuarios;

                if (usuario != null && usuario.ParticipaFidelidade && usuario.Fidelidade != null)
                {
                    var fidelidadeId = fidelidadeService.BuscaFidelidadePorUsuario(pedido.UsuarioId);

                    if (usarPontos)
                    {
                        pontosResgatados = pagamentoCriado.CalcularPontosResgatados(pedido.ValorTotalPedido);
                    }
                    else
                    {
                        pontosAcumulados = pagamentoCriado.CalculaPontosGanhos();
                    }

                    var pontos = usarPontos ? pontosResgatados : pontosAcumulados;

                    if (pontos > 0)
                    {
                        pontosFidelidadeService.RegistrarPontuacao(new PontosFidelidade
                        {
                            FidelizacaoId = fidelidadeId,
                            PedidoId = pedido.Id,
                            TipoMovimentacaoPontos = usarPontos ? TipoMovimentoPontos.RESGATE : TipoMovimentoPontos.ACUMULO,
                            Pontos = pontos
                        });
                    }
                }
            }
            else
            {
                pagamento.StatusPagamento = StatusPagamento.RECUSADO;
                pagamento.DataConfirmaPg = DateTime.Now;
                mensagemPagamento = "Pagamento recusado. O seu pedido foi cancelado.";
                pagamento.RespostaPayload = $"RECUSADO - {mensagemPagamento}";

                pedidoService.CancelarPedido(pedido.Id);

                var pagamentoRecusado = repository.CriarPagamento(pagamento)!;

                auditoriaService.RegistrarLog(
                    pedido.UsuarioId,
                    "PAGAMENTO_RECUSADO",
                    "Pagamento",
                    pagamentoRecusado.Id,
                    "",
                    $"Pedido: {pedido.Id}; Tipo: {pagamentoRecusado.TipoPagamento}; Valor: {pagamentoRecusado.ValorPagamento}; Transação: {pagamentoRecusado.IdTransacaoExterna}");
            }

            return CriarObjetoPayload(pagamento, pedido, mensagemPagamento, pontosAcumulados, pontosResgatados);

        }

        private static decimal ValidarResgate(Pedido pedido)
        {
            var usuario = pedido.Usuarios ?? throw new KeyNotFoundException("Usuário do pedido não encontrado.");

            if (usuario.Perfil != TipoPerfil.CLIENTE || !usuario.ParticipaFidelidade || usuario.Fidelidade == null)
            {
                throw new InvalidOperationException("Este usuário não participa do programa de fidelidade.");
            }

            if (usuario.Fidelidade.QtdPontos <= 0)
            {
                throw new InvalidOperationException("O cliente não possui pontos para resgatar.");
            }

            return usuario.Fidelidade.QtdPontos;
        }

        public bool TentativaDePagamento(bool simularRecusa)
        {
            return !simularRecusa;
        }

        public PagamentoPayloadDTO CriarObjetoPayload(Pagamento pagamento, Pedido pedido, string mensagem, decimal pontosAcumulados, decimal pontosResgatados)
        {
            return new PagamentoPayloadDTO
            {
                IdTransacaoExterna = pagamento.IdTransacaoExterna,
                CanalDoPedido = pedido.CanalPedido,
                TipoPagamento = pagamento.TipoPagamento,
                ValorPagamento = pagamento.ValorPagamento,
                StatusPagamento = pagamento.StatusPagamento,
                Codigo = pagamento.StatusPagamento == StatusPagamento.ACEITO ? "00" : "05",
                Mensagem = mensagem,
                PontosAcumulados = pontosAcumulados,
                PontosResgatados = pontosResgatados,
                DataConfirmaPg = pagamento.DataConfirmaPg
            };
        }

        public PagamentoDTO CriarObjeto(Pagamento pagamento)
        {
            var pagamentoDTO = new PagamentoDTO
            {
                Id = pagamento.Id,
                TipoPagamento = pagamento.TipoPagamento,
                StatusPagamento = pagamento.StatusPagamento,
                ValorPagamento = pagamento.ValorPagamento,
                DataPagamento = pagamento.DataPagamento,
                DataConfirmaPg = pagamento.DataConfirmaPg,
                IdTransacaoExterna = pagamento.IdTransacaoExterna,
                RespostaPayload = pagamento.RespostaPayload,
                PontosResgatados = pagamento.Pedidos == null
                ? 0 : pagamento.CalcularPontosResgatados(pagamento.Pedidos.ValorTotalPedido),
                Pedidos = pagamento.Pedidos == null
                ? null : new PedidoPagamentoDTO
                {
                    Id = pagamento.Pedidos.Id,
                    UsuarioId = pagamento.Pedidos.UsuarioId,
                    EstabelecimentoId = pagamento.Pedidos.EstabelecimentoId,
                    FidelizacaoId = pagamento.Pedidos.Usuarios?.Fidelidade?.Id ?? 0,
                    QtdPontos = pagamento.Pedidos.Usuarios?.Fidelidade?.QtdPontos ?? 0,
                },
            };
            return pagamentoDTO;
        }

        public ICollection<PagamentoDTO> CriarObjetoCollection(ICollection<Pagamento> pagamentos)
        {
            var auxiliaPagamento = new List<PagamentoDTO>();

            foreach (var item in pagamentos)
            {
                auxiliaPagamento.Add(CriarObjeto(item));
            }

            return auxiliaPagamento;
        }
    }
}
