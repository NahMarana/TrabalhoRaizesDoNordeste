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
                    return NotFound("Estoque da Unidade por ID não encontrado.");

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

        [HttpGet("verificar")]
        public IActionResult VerificaSePossuiEstoque([FromQuery] int estabelecimentoId, [FromQuery] int produtoId, [FromQuery] int quantidade)
        {
            try
            {
                var possuiEstoque = _service.VerificaSePossuiEstoque(estabelecimentoId, produtoId, quantidade);

                return Ok(new
                {
                    estabelecimentoId,
                    produtoId,
                    quantidade,
                    possuiEstoque
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarEstoqueDeUnidade(EstoqueUnidade estoque)
        {
            try
            {
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
                var estoqueAtualizado = _service.AtualizarEstoque(id, estoque);

                if (estoqueAtualizado == null)
                    return NotFound("Estoque não encontrado!");

                return Ok("Os dados do estoque foram atualizados.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}