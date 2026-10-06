using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ProdutoService _service;

        public ProdutosController(AppDbContext appDbContext, ProdutoService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("buscar")]
        public IActionResult BuscarProdutosPorNome(string nome)
        {
            var produtos = _service.BuscarPorNome(nome);

            if (produtos == null)
                return NotFound("Produto não encontrado.");

            return Ok(produtos);

        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarProdutosPorId(int id)
        {
            var produto = _service.BuscaPorId(id);

            if (produto == null)
                return NotFound("Produto não encontrada.");

            return Ok(produto);
        }

        [HttpGet]
        public IActionResult ListarProdutos()
        {
            var produtos = _service.ListarProdutos();

            return Ok(produtos);
        }

        [HttpPost]
        public IActionResult CriarProduto(Produto produtos)
        {
            if (produtos.CategoriaId == 0)
            {
                return BadRequest("Categoria do produto não identificado!");
            }

            var produtoCriado = _service.CriarProduto(produtos);

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult ProdutoUpdate(int id, Produto produto)
        {
            var produtos = _service.AtualizarProduto(id, produto);

            if (produtos == null)
                return NotFound("Produto não encontrada.");

            return Ok("Produto Atualizado!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduto(int id)
        {
            var produto = _service.DeletarProduto(id);
            if (produto == null)
                return NoContent();

            return Ok("Produto deletado com sucesso!");
        }
    }
}
