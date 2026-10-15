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
            try
            {
                var produtos = _service.BuscarPorNome(nome);

                if (produtos == null)
                    return NotFound("Produto não encontrado.");

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarProdutosPorId(int id)
        {
            try
            {
                var produto = _service.BuscaPorId(id);

                if (produto == null)
                    return NotFound("Produto não encontrada.");

                return Ok(produto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarProdutos()
        {
            try
            {
                var produtos = _service.ListarProdutos();

                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarProduto(Produto produtos)
        {
            try
            {
                if (produtos.CategoriaId == 0)
                {
                    return BadRequest("Categoria do produto não identificado!");
                }

                var produtoCriado = _service.CriarProduto(produtos);

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult ProdutoUpdate(int id, Produto produto)
        {
            try
            {
                var produtos = _service.AtualizarProduto(id, produto);

                if (produtos == null)
                    return NotFound("Produto não encontrada.");

                return Ok("Produto Atualizado!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
