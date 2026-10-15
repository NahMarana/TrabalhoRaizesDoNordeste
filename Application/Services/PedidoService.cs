using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class PedidoService(PedidoRepository repository, LogAuditoriaService auditoriaService)
    {
        public PedidoDTO? BuscaPorId(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var pedido = repository.BuscaPedidoPorId(id);

            if (pedido == null)
            {
                return null;
            }

            var pedidoDTO = CriarObjeto(pedido);

            return pedidoDTO;
        }

        public ICollection<PedidoDTO> ListarPedidos()
        {
            var pedido = repository.ListarPedidos();
            var pedidoDTO = CriarObjetoCollection(pedido);

            return pedidoDTO;

        }

        public ICollection<PedidoDTO> ListarPedidosPorCanal(CanalPedido canalPedido)
        {
            var pedidos = repository.ListarPedidosPorCanal(canalPedido); 
            return CriarObjetoCollection(pedidos);

        }

        public Pedido CriarPedido(Pedido pedido)
        {
            if (pedido == null)
            {
                throw new ArgumentNullException("Pedido inexistente.");
            }

            if (!Enum.IsDefined(pedido.ModoReceber))
                throw new ArgumentException("Modo de recebimento inválido.");

            if (pedido.ItensPedidos == null || !pedido.ItensPedidos.Any())
                throw new ArgumentException("O pedido precisa possuir algum item.");

            var itensPedido = AdicionaItensPedido(pedido.ItensPedidos);

            var novoPedido = new Pedido
            {
                UsuarioId = pedido.UsuarioId,
                EstabelecimentoId = pedido.EstabelecimentoId,
                CanalPedido = pedido.CanalPedido,
                ModoReceber = pedido.ModoReceber,
                StatusPedido = StatusPedido.PENDENTE,
                ValorTotalPedido = 0,
                Descricao = pedido.Descricao,
                ItensPedidos = itensPedido
            };

            novoPedido.CalcularValorTotalPedido();

            var pedidoCriado = repository.CriarPedido(novoPedido);

            auditoriaService.RegistrarLog(
                pedidoCriado.UsuarioId,
                "CRIAR_PEDIDO",
                "Pedido",
                pedidoCriado.Id,
                "",
                $"Status: {pedidoCriado.StatusPedido}; Valor: {pedidoCriado.ValorTotalPedido}; Canal: {pedidoCriado.CanalPedido}"
            );

            return pedidoCriado;

            //return repository.CriarPedido(novoPedido);
        }

        public Pedido? AtualizarPedido(int id, Pedido pedido)
        {
            if (id < 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var pedidoEncontrado = repository.BuscaPedidoPorId(id);

            var itensPedido = AdicionaItensPedido(pedido.ItensPedidos);

            pedidoEncontrado.ValidarEditarPedido();

            return repository.AtualizarPedido(id, pedido);
        }

        public ICollection<ItensPedido> AdicionaItensPedido(
            ICollection<ItensPedido>? itens)
        {
            var novosItens = new List<ItensPedido>();

            foreach (var item in itens)
            {
                var produto = repository.BuscaProdutoPorId(item.ProdutoId);

                if (produto.PrecoUnitario <= 0)
                {
                    throw new ArgumentException("O produto possui um preço zerado.");
                }

                var novoItemPedido = new ItensPedido
                {
                    PedidoId = 0,
                    ProdutoId = produto.Id,
                    QtdItens = item.QtdItens,
                    PrecoUnitario = produto.PrecoUnitario,
                    PrecoTotal = 0
                };

                novoItemPedido.CalcularPrecoTotal();
                novosItens.Add(novoItemPedido);
            }

            return novosItens;
        }
        //metodo e pagamento
        //deu certo? AtualizarStatusPedido(idPedido, StatusPedido.EMPREPARO);

        public Pedido AtualizarStatusPedido(int id, StatusPedido statusPedido)
        {
            var pedido = repository.BuscaPedidoPorId(id);

            var statusPedidoAnterior = pedido.StatusPedido;

            var statusPedidoAtualizado = repository.AtualizarStatusPedido(id, statusPedido);

            auditoriaService.RegistrarLog(
                pedido.UsuarioId,
                "ALTERAR_STATUS_PEDIDO",
                "Pedido",
                id,
                $"Status: {statusPedidoAnterior}",
                $"Status: {statusPedido}"
            );

            return statusPedidoAtualizado;

            //return repository.AtualizarStatusPedido(id, statusPedido);
        }

        public Pedido CancelarPedido(int id)
        {
            var pedido = repository.BuscaPedidoPorId(id);

            if (pedido.StatusPedido == StatusPedido.CANCELADO)
            {
                throw new ArgumentException("Pedido cancelado.");
            }

            var statusAnterior = pedido.StatusPedido;

            var pedidoCancelado = repository.CancelarPedido(id);

            auditoriaService.RegistrarLog(
                pedido.UsuarioId,
                "CANCELAR_PEDIDO",
                "Pedido",
                id,
                $"Status: {statusAnterior}",
                "Status: CANCELADO");

            return pedidoCancelado;

            //return repository.CancelarPedido(id);
        }

        public PedidoDTO CriarObjeto(Pedido pedido)
        {
            var pedidoDTO = new PedidoDTO
            {
                Id = pedido.Id,
                DataHoraPedido = pedido.DataHoraPedido,
                Descricao = pedido.Descricao,
                ValorTotalPedido = pedido.ValorTotalPedido,
                CanalPedido = pedido.CanalPedido,
                ModoReceber = pedido.ModoReceber,
                StatusPedido = pedido.StatusPedido,
                Usuarios = pedido.Usuarios == null
                ? null : new UsuarioPedidoDTO
                {
                    Id = pedido.Usuarios.Id,
                    Nome = pedido.Usuarios.Nome,
                    Perfil = pedido.Usuarios.Perfil,
                    ParticipaFidelidade = pedido.Usuarios.ParticipaFidelidade,
                    FidelizacaoID = pedido.Usuarios.Fidelidade?.Id ?? 0,
                    QtdPontos = pedido.Usuarios.Fidelidade?.QtdPontos ?? 0,
                    EstabelecimentoId = pedido.Usuarios.EstabelecimentoId,
                    NomeEstabelecimento = pedido.Usuarios.UnidadesEstabelecimento.NomeEstabelecimento ?? "",
                    CNPJ = pedido.Usuarios.UnidadesEstabelecimento.CNPJ ?? "",
                    Cidade = pedido.Usuarios.UnidadesEstabelecimento.Cidade ?? "",
                    Estado = pedido.Usuarios.UnidadesEstabelecimento.Estado ?? "",
                },
                ItensPedidos = pedido.ItensPedidos.Select(p => new ItensPedidoDTO
                {
                    Id = p.Id,
                    ProdutoId = p.ProdutoId,
                    NomeProduto = p.Produtos.NomeProduto ?? "",
                    QtdItens = p.QtdItens,
                    PrecoUnitario = p.Produtos?.PrecoUnitario ?? 0,
                }).ToList(),
                Pagamentos = pedido.Pagamentos == null
                ? null : new PgPedidoDTO
                {
                    Id = pedido.Pagamentos.Id,
                    ValorPagamento = pedido.Pagamentos.ValorPagamento,
                    TipoPagamento = pedido.Pagamentos.TipoPagamento,
                    DataPagamento = pedido.Pagamentos.DataPagamento,
                }
            };
            return pedidoDTO;
        }

        public ICollection<PedidoDTO>CriarObjetoCollection(ICollection<Pedido> pedidos)
        {
            var auxiliaPedido = new List<PedidoDTO>();
            foreach (var item in pedidos)
            {
                var pedidoDTO = new PedidoDTO
                {
                    Id = item.Id,
                    DataHoraPedido = item.DataHoraPedido,
                    Descricao = item.Descricao,
                    ValorTotalPedido = item.ValorTotalPedido,
                    CanalPedido = item.CanalPedido,
                    ModoReceber = item.ModoReceber,
                    StatusPedido = item.StatusPedido,
                    Usuarios = item.Usuarios == null
                    ? null : new UsuarioPedidoDTO
                    {
                        Id = item.Usuarios.Id,
                        Nome = item.Usuarios.Nome,
                        Perfil = item.Usuarios.Perfil,
                        ParticipaFidelidade = item.Usuarios.ParticipaFidelidade,
                        FidelizacaoID = item.Usuarios.Fidelidade.Id,
                        QtdPontos = item.Usuarios.Fidelidade.QtdPontos,
                        EstabelecimentoId = item.Usuarios.EstabelecimentoId,
                        NomeEstabelecimento = item.Usuarios.UnidadesEstabelecimento.NomeEstabelecimento,
                        CNPJ = item.Usuarios.UnidadesEstabelecimento.CNPJ,
                        Cidade = item.Usuarios.UnidadesEstabelecimento.Cidade,
                        Estado = item.Usuarios.UnidadesEstabelecimento.Estado,
                    },
                    ItensPedidos = item.ItensPedidos.Select(p => new ItensPedidoDTO
                    {
                        Id = p.Id,
                        ProdutoId = p.ProdutoId,
                        NomeProduto = p.Produtos.NomeProduto,
                        QtdItens = p.QtdItens,
                        PrecoUnitario = p.Produtos.PrecoUnitario,
                    }).ToList(),
                    Pagamentos = item.Pagamentos == null
                    ? null : new PgPedidoDTO
                    {
                        Id = item.Pagamentos.Id,
                        ValorPagamento = item.Pagamentos.ValorPagamento,
                        TipoPagamento = item.Pagamentos.TipoPagamento,
                        DataPagamento = item.Pagamentos.DataPagamento,
                    }
                };

                auxiliaPedido.Add(pedidoDTO);
            }
            return auxiliaPedido;
        }
    }
}
