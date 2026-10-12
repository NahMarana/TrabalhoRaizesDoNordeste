using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class PagamentoService(PagamentoRepository repository, PontosFidelidadeService PontosFidservice, PedidoService StatusService)
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

        public PagamentoDTO CriarPagamento(Pagamento pagamento, bool usarPontos)
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

            var novoPagamento = new Pagamento
            {
                PedidoId = pedido.Id,
                TipoPagamento = pagamento.TipoPagamento,
                StatusPagamento = StatusPagamento.PENDENTE,
                ValorPagamento = pedido.ValorTotalPedido,
            };

            if (usarPontos)
            {
                var saldo = ValidarResgate(pedido);

               
                novoPagamento.AplicarResgate(pedido.ValorTotalPedido, saldo);
            }

            var pagamentoCriado = repository.CriarPagamento(novoPagamento);

            return CriarObjeto(repository.BuscaPagamentoPorId(pagamentoCriado.Id)!);
        }

        //Colocar no ConfirmarPagamento o -Retorno do StatusPedido, Retorno de Pontuação e Retorno do payload para o CanalPedido - CriarPayload e IdTransacaoExterna

        //public PagamentoDTO ConfirmarPagamento(int id)
        //{
        //    var pagamento = repository.BuscaPagamentoPorId(id);

        //    if (pagamento == null)
        //    {
        //        throw new KeyNotFoundException("Pagamento não encontrado.");
        //    }
        //}

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
