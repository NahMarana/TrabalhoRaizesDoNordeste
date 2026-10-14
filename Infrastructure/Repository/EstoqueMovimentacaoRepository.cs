using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class EstoqueMovimentacaoRepository(AppDbContext _context)
    {
        public EstoqueMovimentacao? BuscaEstoqueMovimentacaoPorId(int id)
        {
            return _context.EstoquesMovimentacao
                .Include(em => em.EstoqueUnidade)
                .FirstOrDefault(em => em.Id == id);
        }

        public ICollection<EstoqueMovimentacao>? ListarEstoqueMovimentacoes()
        {
            return _context.EstoquesMovimentacao
                .Include(em => em.EstoqueUnidade)
                .ToList();
        }

        public ICollection<EstoqueMovimentacao>? ListarMovimentacoesPorPedido(int pedidoId)
        {
            return _context.EstoquesMovimentacao
                .Include(em => em.EstoqueUnidade)
                .Where(em => em.PedidoId == pedidoId)
                .OrderBy(em => em.DataMovimentacao)
                .ToList();
        }

        public EstoqueMovimentacao? CriarMovimentacao(EstoqueMovimentacao movimentacao)
        {
            _context.EstoquesMovimentacao.Add(movimentacao);
            _context.SaveChanges();

            return movimentacao;
        }

    }
}
