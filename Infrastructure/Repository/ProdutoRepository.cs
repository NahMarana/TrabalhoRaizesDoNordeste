using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class ProdutoRepository(AppDbContext _context)
    {
        public Produto? BuscaProdutoPorId(int id)
        {
            return _context.Produtos
                .Include(c => c.Categoria)
                .FirstOrDefault(p => p.Id == id);
        }

        public Produto? BuscarProdutoPorNome(string nome)
        {
            return _context.Produtos
                .Include(c => c.Categoria)
                .FirstOrDefault(p => p.NomeProduto.ToLower().Contains(nome.ToLower()));
        }

        public ICollection<Produto>? ListarProdutos()
        {
            return _context.Produtos
                .Include(p => p.Categoria)
                .ToList();
        }

        public Produto CriarProduto(Produto produto)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.Id == produto.CategoriaId);

            produto.Categoria = categoria;

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return produto;
        }

        public Produto? AtualizarProduto(int id, Produto produto)
        {
            var produtoEncontrado = _context.Produtos.Find(id);

            produtoEncontrado.NomeProduto = produto.NomeProduto;
            produtoEncontrado.DescricaoProduto = produto.DescricaoProduto;
            produtoEncontrado.PrecoUnitario = produto.PrecoUnitario;
            produtoEncontrado.ProdutoSazonal = produto.ProdutoSazonal;
            produtoEncontrado.DataInicioSazonal = produto.DataInicioSazonal;
            produtoEncontrado.DataFimSazonal = produto.DataFimSazonal;
            produtoEncontrado.ProdutoAtivo = produto.ProdutoAtivo;

            _context.Produtos.Update(produtoEncontrado);
            _context.SaveChanges();

            return produtoEncontrado;
        }

        public bool DeletarProduto(int id)
        {
            var produto = _context.Produtos.Find(id);
            _context.Produtos.Remove(produto);
            var response = _context.SaveChanges();

            return response > 0;
        }
    }
}
