using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class PedidoRepository(AppDbContext _context)
    {
        public Pedido? BuscaPedidoPorId(int id)
        {
            return _context.Pedidos
                .Include(p => p.Usuarios).ThenInclude(u => u!.Fidelidade)
                .Include(p => p.UnidadesEstabelecimento)
                .Include(p => p.ItensPedidos!).ThenInclude(i => i.Produtos)
                .Include(p => p.Pagamentos)
                .FirstOrDefault(p => p.Id == id);
        }

        public ICollection<Pedido>? ListarPedidos()
        {
            return _context.Pedidos
                .Include(p => p.Usuarios).ThenInclude(u => u!.Fidelidade)
                .Include(p => p.UnidadesEstabelecimento)
                .Include(p => p.ItensPedidos!).ThenInclude(i => i.Produtos)
                .Include(p => p.Pagamentos)
                .ToList();
        }

        public ICollection<Pedido>? ListarPedidosPorCanal(CanalPedido canalPedido)
        {
            return _context.Pedidos
                .Include(p => p.Usuarios).ThenInclude(u => u!.Fidelidade)
                .Include(p => p.UnidadesEstabelecimento)
                .Include(p => p.ItensPedidos!).ThenInclude(i => i.Produtos)
                .Include(p => p.Pagamentos)
                .Where(p => p.CanalPedido == canalPedido)
                .ToList();
        }

        public bool UsuarioPossuiFidelidade(int usuarioId)
        {
            return _context.Fidelidades
                .Any(f => f.UsuarioId == usuarioId);
        }

        public Produto? BuscaProdutoPorId(int produtoId)
        {
            return _context.Produtos.Find(produtoId);
        }

        public Pedido? CriarPedido(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            return pedido;
        }

        public Pedido? AtualizarPedido(int id, Pedido pedido)
        {
            var pedidoEncontrado = _context.Pedidos.Find(id);

            pedidoEncontrado.CanalPedido = pedido.CanalPedido;
            pedidoEncontrado.ModoReceber = pedido.ModoReceber;
            pedidoEncontrado.Descricao = pedido.Descricao;
            
            _context.Pedidos.Update(pedidoEncontrado);
            _context.SaveChanges();

            return pedidoEncontrado;
        }

        public Pedido? AtualizarStatusPedido(int id, StatusPedido statusPedido)
        {
            var pedido = _context.Pedidos.Find(id);
            pedido.StatusPedido = statusPedido;

            _context.Pedidos.Update(pedido);
            _context.SaveChanges();

            return pedido;
        }

        public Pedido CancelarPedido(int id)
        {
            var pedido = _context.Pedidos.Find(id);

            pedido.StatusPedido = StatusPedido.CANCELADO;

            _context.Pedidos.Update(pedido);
            _context.SaveChanges();

            return pedido;
        }
    }
}
