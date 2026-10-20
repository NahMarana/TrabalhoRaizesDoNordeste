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

        public ICollection<Pedido>? ListarPedidosAtivos()
        {
            return _context.Pedidos
                .Include(p => p.Usuarios).ThenInclude(u => u!.Fidelidade)
                .Include(p => p.UnidadesEstabelecimento)
                .Include(p => p.ItensPedidos!).ThenInclude(i => i.Produtos)
                .Include(p => p.Pagamentos)
                .Where(p => p.StatusPedido != StatusPedido.CANCELADO)
                .ToList();
        }

        public ICollection<Pedido>? ListarPedidosCancelados()
        {
             return _context.Pedidos
                .Include(p => p.Usuarios).ThenInclude(u => u!.Fidelidade)
                .Include(p => p.UnidadesEstabelecimento)
                .Include(p => p.ItensPedidos!).ThenInclude(i => i.Produtos)
                .Include(p => p.Pagamentos)
                .Where(p => p.StatusPedido == StatusPedido.CANCELADO)
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

        public ICollection<Pedido>? BuscaPedidosDoCliente(int usuarioId)
        {
            return _context.Pedidos
                .Include(p => p.Usuarios)
                .Include(p => p.ItensPedidos!).ThenInclude(i => i.Produtos)
                .Include(p => p.Pagamentos)
                .Where(p => p.UsuarioId == usuarioId)
                .OrderByDescending(p => p.DataHoraPedido)
                .ToList();
        }

        public Usuario? BuscaUsuarioPorId(int usuarioId)
        {
            return _context.Usuarios.Find(usuarioId);
        }

        public UnidadesEstabelecimento? BuscaUnidadePorId(int estabelecimentoId)
        {
            return _context.UnidadesEstabelecimento.Find(estabelecimentoId);
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

            if (pedidoEncontrado == null)
            {
                return null;
            }

            pedidoEncontrado.CanalPedido = pedido.CanalPedido;
            pedidoEncontrado.ModoReceber = pedido.ModoReceber;
            pedidoEncontrado.Descricao = pedido.Descricao;
            
            _context.Pedidos.Update(pedidoEncontrado);
            _context.SaveChanges();

            return pedidoEncontrado;
        }

        public void SalvarPedido(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            _context.SaveChanges();
        }

        public Pedido? AtualizarStatusPedido(int id, StatusPedido statusPedido)
        {
            var pedido = _context.Pedidos.Find(id);

            if (pedido == null)
            {
                return null;
            }

            pedido.StatusPedido = statusPedido;

            _context.Pedidos.Update(pedido);
            _context.SaveChanges();

            return pedido;
        }

        public Pedido CancelarPedido(int id)
        {
            var pedido = _context.Pedidos.Find(id);

            if (pedido == null)
            {
                return null;
            }

            pedido.StatusPedido = StatusPedido.CANCELADO;

            _context.Pedidos.Update(pedido);
            _context.SaveChanges();

            return pedido;
        }
    }
}
