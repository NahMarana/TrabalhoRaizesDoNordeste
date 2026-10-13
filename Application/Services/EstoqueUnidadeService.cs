using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class EstoqueUnidadeService(EstoqueUnidadeRepository repository)
    {
        public EstoqueDaUnidadeDTO BuscaPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var estoque = repository.BuscaEstoqueUnidadePorId(id);

            if (estoque == null)
            {
                return null;
            }

            var estoqueUnidadeDTO = CriarObjeto(estoque);

            return estoqueUnidadeDTO;
        }

        public ICollection<EstoqueDaUnidadeDTO> ListarEstoqueDaUnidade()
        {
            var estoque = repository.ListarEstoqueDaUnidade();
            var estoqueUnidadeDTO = CriarObjetoCollection(estoque);

            return estoqueUnidadeDTO;
        }

        public EstoqueDaUnidadeDTO CriarEstoque(EstoqueUnidade estoque)
        {
            if (estoque == null)
            {
                throw new ArgumentNullException("Estoque Inexistente");
            }

            var unidade = repository.BuscaUnidadePorId(estoque.EstabelecimentoId);

            if (estoque.EstabelecimentoId <= 0)
            {
                throw new ArgumentNullException("É obrigatório estoque possuir Estabelecimento");
            }

            var produto = repository.BuscaProdutoPorId(estoque.ProdutoId);

            if (estoque.ProdutoId <= 0)
            {
                throw new ArgumentException("Id do produto é obrigatória.");
            }

            if (estoque.QtdEstoqueDisponivel < 0)
            {
                throw new ArgumentException("A quantidade em estoque não pode ser zerado.");
            }

            var existeEstoque = repository.BuscaEstoquePorUnidadeEProduto(estoque.EstabelecimentoId, estoque.ProdutoId);

            if (existeEstoque != null)
            {
                throw new InvalidOperationException("Já possui estoque deste produto nesta unidade.");
            }

            var novoEstoque = new EstoqueUnidade
            {
                EstabelecimentoId = unidade.Id,
                ProdutoId = produto.Id,
                QtdEstoqueDisponivel = estoque.QtdEstoqueDisponivel,
                VendaDisponivel = estoque.VendaDisponivel
            };

            var estoqueCriado = repository.CriarEstoque(novoEstoque);

            return CriarObjeto(repository.BuscaEstoqueUnidadePorId(estoqueCriado.Id)!);
        }

        public EstoqueDaUnidadeDTO? AtualizarEstoque(int id, EstoqueUnidade estoque)
        {
            if (id < 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var estoqueAtualizado = repository.AtualizarEstoque(id, estoque);

            if (estoqueAtualizado == null)
            {
                return null;
            }

            return CriarObjeto(repository.BuscaEstoqueUnidadePorId(id)!);
        }

        // Acha o estoque do produto na unidade; se não existir, dá erro
        private EstoqueUnidade BuscarEstoque(int estabelecimentoId, int produtoId)
        {
            if (estabelecimentoId <= 0 || produtoId <= 0)
            {
                throw new ArgumentException("Unidade e produto são obrigatórios.");
            }

            var estoque = repository.BuscaEstoquePorUnidadeEProduto(estabelecimentoId, produtoId);

            if (estoque == null)
            {
                throw new KeyNotFoundException("Não existe estoque deste produto nesta unidade.");
            }

            return estoque;
        }

        //Verifica se a unidade tem a quantidade de produto suficiente.
        public bool VerificaSePossuiEstoque(int estabelecimentoId, int produtoId, int qtdEstoque)
        {
            var estoque = BuscarEstoque(estabelecimentoId, produtoId);
            return estoque.PossuiEstoqueSuficiente(qtdEstoque);
        }

        //Saída de quantidade do estoque
        public EstoqueDaUnidadeDTO DarBaixaNoEstoque(int estabelecimentoId, int produtoId, int qtdEstoque)
        {
            var estoque = BuscarEstoque(estabelecimentoId, produtoId);
            estoque.DarBaixaEstoque(qtdEstoque);

            repository.SalvarEstoque(estoque);
            return CriarObjeto(estoque);
        }

        //Entrada ou retorno de quantidade de produto do estoque
        public EstoqueDaUnidadeDTO ReporNoEstoque(int estabelecimentoId, int produtoId, int qtdEstoque)
        {
            var estoque = BuscarEstoque(estabelecimentoId, produtoId);
            estoque.ReporEstoque(qtdEstoque);

            repository.SalvarEstoque(estoque);
            return CriarObjeto(estoque);
        }


        public EstoqueDaUnidadeDTO CriarObjeto(EstoqueUnidade estoque)
        {
            var estoqueUnidadeDTO = new EstoqueDaUnidadeDTO
            {
                Id = estoque.Id,
                QtdEstoqueDisponivel = estoque.QtdEstoqueDisponivel,
                VendaDisponivel = estoque.VendaDisponivel,
                UnidadesEstabelecimento = estoque.UnidadesEstabelecimento == null
                ? null : new EstabelecimentoEstoqueDTO
                {
                    Id = estoque.UnidadesEstabelecimento.Id,
                    NomeEstabelecimento = estoque.UnidadesEstabelecimento.NomeEstabelecimento,
                    CNPJ = estoque.UnidadesEstabelecimento.CNPJ,
                    UnidadeAtiva = estoque.UnidadesEstabelecimento.UnidadeAtiva,
                },
                Produtos = estoque.Produtos == null
                ? null : new ProdutoEstoqueDTO
                {
                    Id = estoque.Produtos.Id,
                    NomeProduto = estoque.Produtos.NomeProduto,
                    ProdutoSazonal = estoque.Produtos.ProdutoSazonal,
                    ProdutoAtivo = estoque.Produtos.ProdutoAtivo,
                },
                EstoquesMovimentacao = estoque.EstoquesMovimentacao.Select(eu => new MovimentacaoEstoqueDTO
                {
                    Id = eu.Id,
                    QtdEmEstoque = eu.QtdEmEstoque,
                    TipoMovimento = eu.TipoMovimento,
                }).ToList(),
            };
            return estoqueUnidadeDTO;
        }

        public ICollection<EstoqueDaUnidadeDTO> CriarObjetoCollection(ICollection<EstoqueUnidade> estoque)
        {
            var auxiliaEstoque = new List<EstoqueDaUnidadeDTO>();
            foreach (var item in estoque)
            {
                var estoqueUnidadeDTO = new EstoqueDaUnidadeDTO
                {
                    Id = item.Id,
                    QtdEstoqueDisponivel = item.QtdEstoqueDisponivel,
                    VendaDisponivel = item.VendaDisponivel,
                    UnidadesEstabelecimento = item.UnidadesEstabelecimento == null
                    ? null : new EstabelecimentoEstoqueDTO
                    {
                        Id = item.UnidadesEstabelecimento.Id,
                        NomeEstabelecimento = item.UnidadesEstabelecimento.NomeEstabelecimento,
                        CNPJ = item.UnidadesEstabelecimento.CNPJ,
                        UnidadeAtiva = item.UnidadesEstabelecimento.UnidadeAtiva,
                    },
                    Produtos = item.Produtos == null
                    ? null : new ProdutoEstoqueDTO
                    {
                        Id = item.Produtos.Id,
                        NomeProduto = item.Produtos.NomeProduto,
                        ProdutoSazonal = item.Produtos.ProdutoSazonal,
                        ProdutoAtivo = item.Produtos.ProdutoAtivo,
                    },
                    EstoquesMovimentacao = item.EstoquesMovimentacao.Select(eu => new MovimentacaoEstoqueDTO
                    {
                        Id = eu.Id,
                        QtdEmEstoque = eu.QtdEmEstoque,
                        TipoMovimento = eu.TipoMovimento,
                    }).ToList(),
                };
                auxiliaEstoque.Add(estoqueUnidadeDTO);
            }

            return auxiliaEstoque;
        }
    }
}
