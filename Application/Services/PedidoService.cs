using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class PedidoService(PedidoRepository repository, LogAuditoriaService auditoriaService, ItensPedidoService itensPedidoService)
    {
        public PedidoDTO? BuscaPorId(int id)
        {
            if (id <= 0)
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

        public ICollection<PedidoDTO> ListarPedidosAtivos()
        {
            var pedido = repository.ListarPedidosAtivos();
            var pedidoDTO = CriarObjetoCollection(pedido);

            return pedidoDTO;
        }

        public ICollection<PedidoDTO> ListarPedidosCancelados()
        {
            var pedido = repository.ListarPedidosCancelados();
            var pedidoDTO = CriarObjetoCollection(pedido);

            return pedidoDTO;
        }

        public ICollection<PedidoDTO> ListarPedidosPorCanal(CanalPedido canalPedido)
        {
            if (!Enum.IsDefined(canalPedido))
            {
                throw new ArgumentException("Canal do pedido inválido.");
            }

            var pedidos = repository.ListarPedidosPorCanal(canalPedido); 
            return CriarObjetoCollection(pedidos);
        }

        public ICollection<RetornoDoPedidoDTO> BuscarPedidosDoCliente(int usuarioId)
        {
            if (usuarioId <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var pedidos = repository.BuscaPedidosDoCliente(usuarioId) ?? new List<Pedido>();

            if (pedidos.Count == 0)
            {
                throw new KeyNotFoundException("Nenhum pedido encontrado para este cliente.");
            }

            return CriarRetornoClienteCollection(pedidos);
        }

        public Pedido CriarPedido(Pedido pedido)
        {
            if (pedido == null)
            {
                throw new ArgumentNullException("Pedido inexistente.");
            }

            var usuario = repository.BuscaUsuarioPorId(pedido.UsuarioId);

            if (pedido.UsuarioId <= 0)
            {
                throw new ArgumentException("Usuário é obrigatório.");
            }

            if (!usuario.UsuarioAtivo)
            {
                throw new InvalidOperationException("Usuário inativo! Não pode fazer pedido.");
            }

            var unidade = repository.BuscaUnidadePorId(pedido.EstabelecimentoId);

            if (pedido.EstabelecimentoId <= 0)
            {
                throw new ArgumentException("Unidade de Estabelecimento é obrigatória.");
            }

            if (!unidade.UnidadeAtiva)
            {
                throw new InvalidOperationException("Unidade inativada! Não é possível fazer pedido.");
            }

            if (!Enum.IsDefined(pedido.ModoReceber))
            {
                throw new ArgumentException("Modo de recebimento inválido.");
            }
                
            if (pedido.ItensPedidos == null || !pedido.ItensPedidos.Any())
            {
                throw new ArgumentException("O pedido precisa possuir algum item.");
            }

            var novoPedido = new Pedido
            {
                UsuarioId = pedido.UsuarioId,
                EstabelecimentoId = pedido.EstabelecimentoId,
                CanalPedido = pedido.CanalPedido,
                ModoReceber = pedido.ModoReceber,
                StatusPedido = StatusPedido.PENDENTE,
                ValorTotalPedido = 0,
                Descricao = pedido.Descricao,
            };

            var pedidoCriado = repository.CriarPedido(novoPedido);

            try
            {
                foreach (var item in pedido.ItensPedidos)
                {
                    var novoItem = new ItensPedido
                    {
                        PedidoId = pedidoCriado.Id,
                        ProdutoId = item.ProdutoId,
                        QtdItens = item.QtdItens
                    };

                    itensPedidoService.CriarItemPedido(novoItem);
                }
            }
            catch
            {
                CancelarPedido(pedidoCriado.Id);
                throw;
            }

            var pedidoCompleto = repository.BuscaPedidoPorId(pedidoCriado.Id)!;
            pedidoCompleto.CalcularValorTotalPedido();
            repository.SalvarPedido(pedidoCompleto);

            auditoriaService.RegistrarLog(
                pedidoCompleto.UsuarioId,
                "CRIAR_PEDIDO",
                "Pedido",
                pedidoCompleto.Id,
                "",
                $"Status: {pedidoCompleto.StatusPedido}; Valor: {pedidoCompleto.ValorTotalPedido}; Canal: {pedidoCompleto.CanalPedido}"
            );

            return pedidoCompleto;

        }

        public Pedido? AtualizarPedido(int id, Pedido pedido)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            if (!Enum.IsDefined(pedido.ModoReceber))
            {
                throw new ArgumentException("Modo de recebimento inválido.");
            }

            var pedidoEncontrado = repository.BuscaPedidoPorId(id);

            if (pedidoEncontrado == null)
            {
                return null;
            }

            pedidoEncontrado.ValidarEditarPedido();

            repository.AtualizarPedido(id, pedido);

            foreach (var item in pedido.ItensPedidos ?? new List<ItensPedido>())
            {
                if (item.QtdItens < 0)
                {
                    throw new ArgumentException("Não é permitida quantidade de produto negativa.");
                }

                var itemExistente = pedidoEncontrado.ItensPedidos?
                    .FirstOrDefault(i => i.ProdutoId == item.ProdutoId);

                if (itemExistente != null)
                {
                    itensPedidoService.AtualizarItemPedido(itemExistente.Id, item);
                }
                else if (item.QtdItens > 0)
                {
                    var novoItem = new ItensPedido
                    {
                        PedidoId = id,
                        ProdutoId = item.ProdutoId,
                        QtdItens = item.QtdItens
                    };

                    itensPedidoService.CriarItemPedido(novoItem);
                }
            }
            var pedidoAtualizado = repository.BuscaPedidoPorId(id)!;
            pedidoAtualizado.CalcularValorTotalPedido();
            repository.SalvarPedido(pedidoAtualizado);

            return pedidoAtualizado;
        }

        public Pedido? AtualizarStatusPedido(int id, StatusPedido statusPedido)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            if (!Enum.IsDefined(statusPedido))
            {
                throw new ArgumentException("Status do pedido inválido.");
            }

            var pedido = repository.BuscaPedidoPorId(id);

            if (pedido == null)
            {
                return null;
            }

            if (pedido.StatusPedido == StatusPedido.CANCELADO)
            {
                throw new InvalidOperationException("Pedido cancelado não pode mudar de status.");
            }

            if (pedido.Pagamentos == null && statusPedido != StatusPedido.PENDENTE)
            {
                throw new InvalidOperationException("O pedido só pode mudar de status depois do pagamento ser aceito.");
            }

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

        }

        public Pedido CancelarPedido(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var pedido = repository.BuscaPedidoPorId(id);

            if (pedido == null)
            {
                return null;
            }

            if (pedido.StatusPedido == StatusPedido.CANCELADO)
            {
                throw new ArgumentException("Este pedido já está cancelado.");
            }

            pedido.ValidarEditarPedido();

            var statusAnterior = pedido.StatusPedido;
            itensPedidoService.DevolverItensAoEstoque(pedido.Id);

            var pedidoCancelado = repository.CancelarPedido(id);
            

            auditoriaService.RegistrarLog(
                pedido.UsuarioId,
                "CANCELAR_PEDIDO",
                "Pedido",
                id,
                $"Status: {statusAnterior}",
                "Status: CANCELADO");

            return pedidoCancelado;
        }

        public RetornoDoPedidoDTO CriarRetornoCliente(Pedido pedido)
        {
            var itens = new List<ItensPedidoDTO>();

            foreach (var item in pedido.ItensPedidos ?? new List<ItensPedido>())
            {
                itens.Add(new ItensPedidoDTO
                {
                    Id = item.Id,
                    ProdutoId = item.ProdutoId,
                    NomeProduto = item.Produtos?.NomeProduto ?? "",
                    QtdItens = item.QtdItens,
                    PrecoUnitario = item.Produtos?.PrecoUnitario ?? item.PrecoUnitario,
                    ValorPromocional = item.PrecoUnitario < item.Produtos?.PrecoUnitario ? item.PrecoUnitario : null
                });
            }

            return new RetornoDoPedidoDTO
            {
                PedidoId = pedido.Id,
                NomeCliente = pedido.Usuarios?.Nome ?? "",
                DataHoraPedido = pedido.DataHoraPedido,
                CanalPedido = pedido.CanalPedido.ToString(),
                ModoReceber = pedido.ModoReceber.ToString(),
                StatusPedido = pedido.StatusPedido.ToString(),
                Itens = itens,
                ValorTotalPedido = pedido.ValorTotalPedido,
                Pagamento = pedido.Pagamentos == null
                    ? null : new PgPedidoDTO
                    {
                        Id = pedido.Pagamentos.Id,
                        ValorPagamento = pedido.Pagamentos.ValorPagamento,
                        TipoPagamento = pedido.Pagamentos.TipoPagamento,
                        DataPagamento = pedido.Pagamentos.DataPagamento,
                    }
            };
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
                    ValorPromocional = p.PrecoUnitario < p.Produtos?.PrecoUnitario ? p.PrecoUnitario : null,
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

        public ICollection<PedidoDTO> CriarObjetoCollection(ICollection<Pedido> pedidos)
        {
            var auxiliaPedido = new List<PedidoDTO>();

            foreach (var item in pedidos)
            {
                auxiliaPedido.Add(CriarObjeto(item));
            }

            return auxiliaPedido;
        }

        public ICollection<RetornoDoPedidoDTO> CriarRetornoClienteCollection(ICollection<Pedido> pedidos)
        {
            var auxiliaRetorno = new List<RetornoDoPedidoDTO>();

            foreach (var item in pedidos)
            {
                auxiliaRetorno.Add(CriarRetornoCliente(item));
            }

            return auxiliaRetorno;
        }
    }
}
