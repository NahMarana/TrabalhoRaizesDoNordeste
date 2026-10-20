using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class ItensPedidoRepository(AppDbContext _context)
    {
        public ItensPedido? BuscaItensPedidoPorId(int id)
        {
            return _context.ItensPedido
                .Include(i => i.Pedidos).ThenInclude(i => i!.Usuarios)
                .Include(i => i.Produtos)
                .FirstOrDefault(p => p.Id == id);
        }

        public ICollection<ItensPedido>? ListarItensPedidos()
        {
            return _context.ItensPedido
                .Include(i => i.Pedidos).ThenInclude(i => i!.Usuarios)
                .Include(i => i.Produtos)
                .ToList();
        }
        public Produto? BuscaProdutoPorId(int produtoId)
        {
            return _context.Produtos.Find(produtoId);
        }

        public Pedido? BuscaPedidoPorId(int pedidoId)
        {
            return _context.Pedidos
                .Include(p => p.ItensPedidos)
                .Include(p => p.Pagamentos)
                .FirstOrDefault(p => p.Id == pedidoId);
        }

        public EstoqueUnidade? BuscaEstoque(int estabelecimentoId, int produtoId)
        {
            return _context.EstoquesUnidade
                .FirstOrDefault(e => e.EstabelecimentoId == estabelecimentoId && e.ProdutoId == produtoId);
        }

        public ICollection<PromocoesCampanhas> BuscaPromocoesDoProduto(int estabelecimentoId, int produtoId)
        {
            return _context.PromocoesCampanha
                .Where(pc => pc.EstabelecimentoId == estabelecimentoId
                          && pc.ProdutoId == produtoId
                          && pc.StatusPromoCampanha == StatusPromoCampanha.ATIVO)
                .ToList();
        }

        public ItensPedido CriarItensPedido(ItensPedido item)
        {
            _context.ItensPedido.Add(item);
            _context.SaveChanges();

            AtualizarValorTotalPedido(item.PedidoId);

            return item;
        }

        public ItensPedido? AtualizarItemPedido(int id, ItensPedido item)
        {
            var itemEncontrado = _context.ItensPedido.Find(id);

            if (itemEncontrado == null)
            {
                return null;
            }

            itemEncontrado.QtdItens = item.QtdItens;
            itemEncontrado.CalcularPrecoTotal();

            _context.ItensPedido.Update(itemEncontrado);
            _context.SaveChanges();

            AtualizarValorTotalPedido(itemEncontrado.PedidoId);

            return itemEncontrado;
        }

        public void RemoverItemPedido(ItensPedido item)
        {
            var pedidoId = item.PedidoId;

            _context.ItensPedido.Remove(item);

            _context.SaveChanges();

            AtualizarValorTotalPedido(pedidoId);
        }

        private void AtualizarValorTotalPedido(int pedidoId)
        {
            var pedido = _context.Pedidos
                .Include(p => p.ItensPedidos)
                .FirstOrDefault(p => p.Id == pedidoId);

            if (pedido == null)
                return;

            pedido.CalcularValorTotalPedido();

            _context.SaveChanges();
        }
    }
}
