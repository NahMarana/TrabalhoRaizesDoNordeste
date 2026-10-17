using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class PagamentoService(PagamentoRepository repository, PontosFidelidadeService pontosFidelidadeService, PedidoService pedidoService, FidelidadeService fidelidadeService)
    {
        public PagamentoDTO? BuscaPorId(int id)
        {
            if (id < 0)
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

        public PagamentoPayloadDTO CriarPagamento(Pagamento pagamento, bool usarPontos)
        {
            if (pagamento == null)
            {
                throw new ArgumentNullException("Pagamento inexistente.");
            }

            if (!Enum.IsDefined(pagamento.TipoPagamento))
            {
                throw new ArgumentException("Modo de escolha de pagamento inválido.");
            }

            var pedido = repository.BuscaPedidoPorId(pagamento.PedidoId);

            if (pedido.Pagamentos != null)
            {
                throw new InvalidOperationException("Este pedido já possui um pagamento.");
            }

            if (pedido.StatusPedido != StatusPedido.PENDENTE)
            {
                throw new InvalidOperationException("Só é possível pagar seu pedido se o status estiver PENDENTE.");
            }

            //var novoPagamento = new Pagamento
            //{
            //    PedidoId = pedido.Id,
            //    TipoPagamento = pagamento.TipoPagamento,
            //    StatusPagamento = StatusPagamento.PENDENTE,
            //    ValorPagamento = pedido.ValorTotalPedido,
            //};

            var mensagemPagamento = "";
            var tentativaPagamento = TentativaDePagamento();
            if (tentativaPagamento > 5)
            {
                var saldo = ValidarResgate(pedido);
                if (usarPontos)
                {
                    pagamento.AplicarResgate(pedido.ValorTotalPedido, saldo);
                }
                else
                {
                    saldo += pagamento.CalculaPontosGanhos();
                }

                pagamento.StatusPagamento = StatusPagamento.PENDENTE;
                var pagamentoCriado = repository.CriarPagamento(pagamento);
                mensagemPagamento = "Pagamento realizado com sucesso.";

                pedidoService.AtualizarStatusPedido(pagamentoCriado.Id, StatusPedido.EMPREPARO);

                var fidelidadeId = fidelidadeService.BuscaFidelidadePorUsuario(pagamento.Pedidos.UsuarioId);

                pontosFidelidadeService.RegistrarPontuacao(new PontosFidelidade
                {
                    FidelizacaoId = fidelidadeId,
                    PedidoId = pagamento.PedidoId,
                    TipoMovimentacaoPontos = usarPontos ? TipoMovimentoPontos.RESGATE : TipoMovimentoPontos.ACUMULO,
                    Pontos = saldo
                });
            }
            else
            {
                pagamento.StatusPagamento = StatusPagamento.RECUSADO;
                mensagemPagamento = "Pagamento recusado.";
                pedidoService.AtualizarStatusPedido(pagamento.Pedidos.Id, StatusPedido.CANCELADO);
            }            

            return CriarObjetoPayload(pagamento, mensagemPagamento);
        }

        public Pagamento? AtualizarPagamento(int id, Pagamento pagamento)
        {
            if (!Enum.IsDefined(typeof(TipoPagamento), pagamento.TipoPagamento))
            {
                throw new ArgumentException("Tipo de pagamento inválido inválido.");
            }

            if (id < 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var pagamentoAtualizado = repository.AtualizarPagamento(id, pagamento);
            return pagamentoAtualizado;
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

        public int TentativaDePagamento()
        {
            return new Random().Next(1, 10);
        }

        public PagamentoPayloadDTO CriarObjetoPayload(Pagamento pagamento, string mensagem)
        {
            return new PagamentoPayloadDTO
            {
                IdTransacaoExterna = pagamento.IdTransacaoExterna,
                CanalDoPedido = pagamento.Pedidos.CanalPedido,
                TipoPagamento = pagamento.TipoPagamento,
                ValorPagamento = pagamento.ValorPagamento,
                StatusPagamento = pagamento.StatusPagamento,
                Codigo = "",
                Mensagem = mensagem,
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
                var pagamentoDTO = CriarObjeto(item);

                auxiliaPagamento.Add(pagamentoDTO);
            }

            return auxiliaPagamento;
        }
    }
}
