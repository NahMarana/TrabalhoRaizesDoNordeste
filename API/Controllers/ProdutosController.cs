using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ProdutosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("buscar")]
        public IActionResult BuscarProdutosPorNome(string nome)
        {
            nome = String.Concat(char.ToUpper(nome[0]), nome.Substring(1));
            var produtos = _appDbContext.Produtos
                .Where(p => p.NomeProduto.Contains(nome))
                .Select(p => new
                {
                    p.Id,
                    p.NomeProduto,
                    p.DescricaoProduto,
                    p.PrecoUnitario,
                    p.ProdutoSazonal,
                    p.DataInicioSazonal,
                    p.DataFimSazonal,
                    p.ProdutoAtivo,
                    Categoria = new
                    {
                        p.CategoriaId,
                        NomeCategoria = p.Categoria!.NomeCategoria
                    }
                })
                .ToList(); ;

            if (!produtos.Any())
                return NotFound("Produto não encontrado.");

            return Ok(produtos);
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarProdutosPorId(int id)
        {
            var produto = _appDbContext.Produtos
                .Where(p => p.Id == id)
                .Select(p => new
            {
                    p.Id,
                    p.NomeProduto,
                    p.DescricaoProduto,
                    p.PrecoUnitario,
                    p.ProdutoSazonal,
                    p.DataInicioSazonal,
                    p.DataFimSazonal,
                    p.ProdutoAtivo,
                    Categoria = new
                    {
                        p.CategoriaId,
                        NomeCategoria = p.Categoria!.NomeCategoria
                    }
            })
            .FirstOrDefault();

            if (produto == null)
                return NoContent();

            return Ok(produto);
        }

        [HttpGet]
        public IActionResult ListarProdutos()
        {
            var produtos = _appDbContext.Produtos
            .Select(p => new
            {
                p.Id,
                p.NomeProduto,
                p.DescricaoProduto,
                p.PrecoUnitario,
                p.ProdutoSazonal,
                p.DataInicioSazonal,
                p.DataFimSazonal,
                p.ProdutoAtivo,
                Categoria = new
                {
                    p.CategoriaId,
                    NomeCategoria = p.Categoria!.NomeCategoria
                }
            })
            .ToList();

            return Ok(produtos);
        }

        [HttpPost]
        public IActionResult CriarProduto(Produto produtos)
        {
            if (produtos.CategoriaId == 0)
            {
                return BadRequest("Categoria do produto não identificado!");
            }

            var categoria = _appDbContext.Categorias.FirstOrDefault(c => c.Id == produtos.CategoriaId);

            if (categoria == null)
            {
                return BadRequest("Categoria não encontrada");
            }

            produtos.Categoria = categoria;
            _appDbContext.Produtos.Add(produtos);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult ProdutoUpdate(int id, Produto produtos)
        {
            var produto = _appDbContext.Produtos.Find(id);
            if (produto == null)
                return NoContent();

            produto.NomeProduto = produtos.NomeProduto;
            produto.CategoriaId = produtos.CategoriaId;
            produto.DescricaoProduto = produtos.DescricaoProduto;
            produto.PrecoUnitario = produtos.PrecoUnitario;
            produto.ProdutoSazonal = produtos.ProdutoSazonal;
            produto.DataInicioSazonal = produtos.DataInicioSazonal;
            produto.DataFimSazonal = produtos.DataFimSazonal;
            produto.ProdutoAtivo = produtos.ProdutoAtivo;

            _appDbContext.Produtos.Update(produto);
            _appDbContext.SaveChanges();

            return Ok("Produto atualizado!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduto(int id)
        {
            var produto = _appDbContext.Produtos.Find(id);
            if (produto == null)
                return NoContent();

            _appDbContext.Produtos.Remove(produto);
            _appDbContext.SaveChanges();

            return Ok("Produto deletado com sucesso!");
        }
    }
}
