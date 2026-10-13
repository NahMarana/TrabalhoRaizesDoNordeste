using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueUnidadeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly EstoqueUnidadeService _service;

        public EstoqueUnidadeController(AppDbContext appDbContext, EstoqueUnidadeService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarEstoqueDeUnidadePorId(int id)
        {
            try
            {
                var estoque = _service.BuscaPorId(id);

                if (estoque == null)
                    return NoContent();

                return Ok(estoque);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarEstoqueDeUnidades()
        {
            try
            {
                var estoque =_service.ListarEstoqueDaUnidade();

                return Ok(estoque);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarEstoqueDeUnidade(EstoqueUnidade estoque)
        {
            try
            {
                if (estoque.ProdutoId == 0)
                {
                    return BadRequest("Pedido não identificado!");
                }

                if (estoque.EstabelecimentoId == 0)
                {
                    return BadRequest("Produto não identificado!");
                }

                var estoqueCriado = _service.CriarEstoque(estoque);

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult EstoqueDeUnidadeUpdate(int id, EstoqueUnidade estoque)
        {
            try
            {
                var estoqueAtualizad = _service.AtualizarEstoque(id, estoque);

                if (estoque == null)
                    return NotFound("Estoque não encontrado!");

                return Ok("Os dados do estoque foram atualizados.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("baixa")]
        public IActionResult DarBaixaNoEstoque(int estabelecimentoId, int produtoId, int quantidade)
        {
            try
            {
                return Ok(_service.DarBaixaNoEstoque(estabelecimentoId, produtoId, quantidade));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("repor")]
        public IActionResult ReporNoEstoque(int estabelecimentoId, int produtoId, int quantidade)
        {
            try
            {
                return Ok(_service.ReporNoEstoque(estabelecimentoId, produtoId, quantidade));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}