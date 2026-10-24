using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class EstoqueMovimentacaoService(EstoqueMovimentacaoRepository repository)
    {
        public MovimentacaoDTO? BuscaPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var movimentacao = repository.BuscaEstoqueMovimentacaoPorId(id);

            if (movimentacao == null)
            {
                return null;
            }

            var estoquemMovimentacaoDTO = CriarObjeto(movimentacao);

            return estoquemMovimentacaoDTO;
        }

        public ICollection<MovimentacaoDTO> ListarEstoqueMovimentacao()
        {
            var movimentacao = repository.ListarEstoqueMovimentacoes();
            var estoqueMovimentacaoDTO = CriarObjetoCollection(movimentacao);

            return estoqueMovimentacaoDTO;
        }

        public ICollection<MovimentacaoDTO> ListarMovimentacoesPorPedido(int pedidoId)
        {
            if (pedidoId <= 0)
            {
                throw new ArgumentException("ID do pedido é inválido.");
            }

            var movimentacoes = repository.ListarMovimentacoesPorPedido(pedidoId);

            if (movimentacoes == null || !movimentacoes.Any())
            {
                throw new KeyNotFoundException("Nenhuma movimentação encontrada para este pedido.");
            }

            return CriarObjetoCollection(movimentacoes);
        }

        public ICollection<MovimentacaoDTO> ListarMovimentacoesEntrada()
        {
            var movimentacao = repository.ListarMovimentacoesEntrada();
            var movimentacaoDTO = CriarObjetoCollection(movimentacao);

            return movimentacaoDTO;
        }

        public ICollection<MovimentacaoDTO> ListarMovimentacoesSaida()
        {
            var movimentacao = repository.ListarMovimentacoesSaida();
            var movimentacaoDTO = CriarObjetoCollection(movimentacao);

            return movimentacaoDTO;
        }

        public void RegistrarMovimentacao(int estoqueId, int pedidoId, int usuarioId, TipoMovimento tipo, int quantidade, string motivo)
        {
            if (quantidade <= 0)
            {
                throw new ArgumentException("A movimentação de quantidade nunca deve ser zerada!");
            }

            if (string.IsNullOrWhiteSpace(motivo))
            {
                throw new ArgumentException("O motivo da movimentação é obrigatório.");
            }

            var movimentacao = new EstoqueMovimentacao
            {
                EstoqueId = estoqueId,
                PedidoId = pedidoId,
                UsuarioUsadoId = usuarioId,
                TipoMovimento = tipo,
                QtdEmEstoque = quantidade,
                MotivoMovimentacao = motivo,
                DataMovimentacao = DateTime.Now
            };

            repository.CriarMovimentacao(movimentacao);
        }

        public MovimentacaoDTO CriarObjeto(EstoqueMovimentacao movimentacao)
        {
            var estoqueMovimentacaoDTO = new MovimentacaoDTO
            {
                Id = movimentacao.Id,
                UsuarioUsadoId = movimentacao.UsuarioUsadoId,
                PedidoId = movimentacao.PedidoId,
                QtdEmEstoque = movimentacao.QtdEmEstoque,
                TipoMovimento = movimentacao.TipoMovimento,
                MotivoMovimentacao = movimentacao.MotivoMovimentacao,
                DataMovimentacao = movimentacao.DataMovimentacao,
                EstoqueUnidade = movimentacao.EstoqueUnidade == null
                ? null : new MovEstoqueDTO
                {
                    Id = movimentacao.EstoqueUnidade.Id,
                    ProdutoId = movimentacao.EstoqueUnidade.ProdutoId,
                    EstabelecimentoId = movimentacao.EstoqueUnidade.EstabelecimentoId,
                    QtdEstoqueDisponivel = movimentacao.EstoqueUnidade.QtdEstoqueDisponivel,
                }
            };
            return estoqueMovimentacaoDTO;
        }

        public ICollection<MovimentacaoDTO> CriarObjetoCollection(ICollection<EstoqueMovimentacao> movimentacao)
        {
            var auxiliaMovimentacao = new List<MovimentacaoDTO>();

            foreach (var item in movimentacao)
            {
                auxiliaMovimentacao.Add(CriarObjeto(item));
            }

            return auxiliaMovimentacao;
        }
    }
}
