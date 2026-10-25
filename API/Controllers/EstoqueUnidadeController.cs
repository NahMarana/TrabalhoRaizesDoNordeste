using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.COZINHA)}, " +
        $"{nameof(TipoPerfil.ATENDENTE)}")]
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
        [ProducesResponseType(typeof(EstoqueDaUnidadeDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(ICollection<EstoqueDaUnidadeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult VerificaSePossuiEstoque([FromQuery] int estabelecimentoId, [FromQuery] int produtoId, [FromQuery] int quantidade)
        {
            try
            {
                return Ok(_service.VerificaQtdEmEstoqueAposPedido(estabelecimentoId, produtoId, quantidade));
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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
        [HttpPost]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
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