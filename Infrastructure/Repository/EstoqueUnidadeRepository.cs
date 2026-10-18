using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;


namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class EstoqueUnidadeRepository(AppDbContext _context)
    {
        public EstoqueUnidade? BuscaEstoqueUnidadePorId(int id)
        {
            return _context.EstoquesUnidade
                .Include(eu => eu.UnidadesEstabelecimento)
                .Include(eu => eu.Produtos)
                .Include(eu => eu.EstoquesMovimentacao)
                .FirstOrDefault(eu => eu.Id == id);
        }

        public ICollection<EstoqueUnidade>? ListarEstoqueDaUnidade()
        {
            return _context.EstoquesUnidade
                .Include(eu => eu.UnidadesEstabelecimento)
                .Include(eu => eu.Produtos)
                .Include(eu => eu.EstoquesMovimentacao)
                .ToList();
        }

        public EstoqueUnidade? BuscaEstoquePorUnidadeEProduto(int estabelecimentoId, int produtoId)
        {
            return _context.EstoquesUnidade
                .Include(eu => eu.UnidadesEstabelecimento)
                .Include(eu => eu.Produtos)
                .Include(eu => eu.EstoquesMovimentacao)
                .FirstOrDefault(eu => eu.EstabelecimentoId == estabelecimentoId && eu.ProdutoId == produtoId);
        }

        public UnidadesEstabelecimento? BuscaUnidadePorId(int estabelecimentoId)
        {
            return _context.UnidadesEstabelecimento.Find(estabelecimentoId);
        }

        public Produto? BuscaProdutoPorId(int produtoId)
        {
            return _context.Produtos.Find(produtoId);
        }

        public EstoqueUnidade? CriarEstoque(EstoqueUnidade estoque)
        {
            _context.EstoquesUnidade.Add(estoque);
            _context.SaveChanges();

            return estoque;
        }

        public EstoqueUnidade? AtualizarEstoque(int id, EstoqueUnidade estoque)
        {
            var estoqueEncontrado = _context.EstoquesUnidade.Find(id);

            if (estoqueEncontrado == null)
                return null;

            estoqueEncontrado.VendaDisponivel = estoque.VendaDisponivel;

            _context.EstoquesUnidade.Update(estoqueEncontrado);
            _context.SaveChanges();

            return estoqueEncontrado;
        }

        public EstoqueUnidade SalvarEstoque(EstoqueUnidade estoque)
        {
            _context.EstoquesUnidade.Update(estoque);
            _context.SaveChanges();

            return estoque;
        }

    }
}
