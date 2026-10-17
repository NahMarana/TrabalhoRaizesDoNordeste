using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class PagamentoRepository(AppDbContext _context)
    {
        public Pagamento? BuscaPagamentoPorId(int id)
        {
            return _context.Pagamentos
                .Include(pg => pg.Pedidos)
                    .ThenInclude(p => p!.Usuarios)
                        .ThenInclude(u => u!.Fidelidade)
                .FirstOrDefault(pg => pg.Id == id);
        }

        public ICollection<Pagamento>? ListarPagamentos()
        {
            return _context.Pagamentos
                .Include(pg => pg.Pedidos)
                    .ThenInclude(p => p!.Usuarios)
                        .ThenInclude(u => u!.Fidelidade)
                .ToList();
        }

        public Pedido? BuscaPedidoPorId(int pedidoId)
        {
            return _context.Pedidos
                .Include(p => p.Usuarios).ThenInclude(u => u!.Fidelidade)
                .Include(p => p.Pagamentos)
                .FirstOrDefault(p => p.Id == pedidoId);
        }

        public Pagamento? CriarPagamento(Pagamento pagamento)
        {
            pagamento.IdTransacaoExterna = Guid.NewGuid().ToString("N");
            pagamento.StatusPagamento = StatusPagamento.ACEITO;
            _context.Pagamentos.Add(pagamento);
            _context.SaveChanges();

            return pagamento;
        }

        public Pagamento? AtualizarPagamento(int id, Pagamento pagamento)
        {
            var pagamentoEncontrado = _context.Pagamentos.Find(id);

            pagamentoEncontrado.TipoPagamento = pagamento.TipoPagamento;

            _context.Pagamentos.Update(pagamentoEncontrado);
            _context.SaveChanges();

            return pagamentoEncontrado;
        }
    }
}
