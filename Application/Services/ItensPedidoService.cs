using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class ItensPedidoService(ItensPedidoRepository repository, EstoqueUnidadeService estoqueUnidadeService)
    {
        public ItensDoPedidoDTO BuscaPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var item = repository.BuscaItensPedidoPorId(id);

            if (item == null)
            {
                return null;
            }

            var itemPedidoDTO = CriarObjeto(item);

            return itemPedidoDTO;
        }

        public ICollection<ItensDoPedidoDTO> ListarItensPedidos()
        {
            var item = repository.ListarItensPedidos();
            var itemPedidoDTO = CriarObjetoCollection(item);

            return itemPedidoDTO;
        }

        public ItensDoPedidoDTO CriarItemPedido(ItensPedido item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item do Pedido Inexistente");
            }

            if (item.QtdItens <= 0)
            {
                throw new ArgumentException("É obrigatório pelo menos ter um produto.");
            }

            if (item.PrecoUnitario <= 0)
            {
                throw new ArgumentException("É obrigatório produto possuir valor.");
            }

            var pedido = repository.BuscaPedidoPorId(item.PedidoId);

            if (item.PedidoId <= 0)
            {
                throw new ArgumentException("Id do pedido é obrigatória.");
            }

            pedido.ValidarEditarPedido();

            var produto = repository.BuscaProdutoPorId(item.ProdutoId);

            if (item.ProdutoId <= 0)
            {
                throw new ArgumentException("Id do produto é obrigatória.");
            }

            estoqueUnidadeService.DarBaixaNoEstoque(pedido.EstabelecimentoId, produto.Id, item.QtdItens, pedido.Id, pedido.UsuarioId);

            var novoItem = new ItensPedido
            {
                PedidoId = pedido.Id,
                ProdutoId = produto.Id,
                QtdItens = item.QtdItens,
                PrecoUnitario = produto.PrecoUnitario
            };

            novoItem.CalcularPrecoTotal();

            var itemCriado = repository.CriarItensPedido(novoItem);

            return CriarObjeto(repository.BuscaItensPedidoPorId(itemCriado.Id)!);
        }

        public ItensDoPedidoDTO? AtualizarItemPedido(int id, ItensPedido item)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            if (item.QtdItens < 0)
            {
                throw new ArgumentException("Não é permitida quantidade de produto negativa.");
            }

            var itemEncontrado = repository.BuscaItensPedidoPorId(id);

            if (itemEncontrado == null)
            {
                throw new KeyNotFoundException("Item não encontrado.");
            }

            var pedido = repository.BuscaPedidoPorId(itemEncontrado.PedidoId);

            if (pedido == null)
            {
                throw new KeyNotFoundException("Pedido não encontrado.");
            }

            pedido.ValidarEditarPedido();

            if (item.QtdItens == 0)
            {
                estoqueUnidadeService.ReporNoEstoque(pedido.EstabelecimentoId, itemEncontrado.ProdutoId, itemEncontrado.QtdItens, pedido.Id, pedido.UsuarioId);

                repository.RemoverItemPedido(itemEncontrado);
                return null;
            }

            var diferenca = item.QtdItens - itemEncontrado.QtdItens;

            if (diferenca > 0)
            {
                estoqueUnidadeService.DarBaixaNoEstoque(pedido.EstabelecimentoId, itemEncontrado.ProdutoId, diferenca, pedido.Id, pedido.UsuarioId);
            }
            else if (diferenca < 0)
            {
                estoqueUnidadeService.ReporNoEstoque(pedido.EstabelecimentoId, itemEncontrado.ProdutoId, -diferenca, pedido.Id, pedido.UsuarioId);
            }

            var itemAtualizado = repository.AtualizarItemPedido(id, item);

            return CriarObjeto(itemAtualizado!);
        }


        public ItensDoPedidoDTO CriarObjeto(ItensPedido itens)
        {
            var itensPedidoDTO = new ItensDoPedidoDTO
            {
                Id = itens.Id,
                QtdItens = itens.QtdItens,
                PrecoUnitario = itens.PrecoUnitario,
                PrecoTotal = itens.PrecoTotal,
                Pedidos = itens.Pedidos == null
                ? null : new PedidoItemDTO
                {
                    Id = itens.Pedidos.Id,
                    UsuarioId = itens.Pedidos.UsuarioId,
                },
                Produtos = itens.Produtos == null
                ? null : new ProdutoItemDTO
                {
                    Id = itens.Produtos.Id,
                    NomeProduto = itens.Produtos.NomeProduto,
                }
            };
            return itensPedidoDTO;
        }

        public ICollection<ItensDoPedidoDTO> CriarObjetoCollection(ICollection<ItensPedido> itens)
        {
            var auxiliaItensPedido = new List<ItensDoPedidoDTO>();

            foreach (var item in itens)
            {
                var itemPedidoDTO = new ItensDoPedidoDTO
                {
                    Id = item.Id,
                    QtdItens = item.QtdItens,
                    PrecoUnitario = item.PrecoUnitario,
                    PrecoTotal = item.PrecoTotal,
                    Pedidos = item.Pedidos == null
                    ? null : new PedidoItemDTO
                    {
                        Id = item.Pedidos.Id,
                        UsuarioId = item.Pedidos.UsuarioId,
                    },
                    Produtos = item.Produtos == null
                    ? null : new ProdutoItemDTO
                    {
                        Id = item.Produtos.Id,
                        NomeProduto = item.Produtos.NomeProduto,
                    },
                };

                auxiliaItensPedido.Add(itemPedidoDTO);
            }

            return auxiliaItensPedido;
        }
    }
}
