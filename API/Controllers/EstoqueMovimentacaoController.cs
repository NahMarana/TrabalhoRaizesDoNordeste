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
    [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
    public class EstoqueMovimentacaoController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly EstoqueMovimentacaoService _service;

        public EstoqueMovimentacaoController(AppDbContext appDbContext, EstoqueMovimentacaoService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(MovimentacaoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult BuscarMovimentacaoDoEstoquePorId(int id)
        {
            try
            {
                var movimentacao = _service.BuscaPorId(id);

                if (movimentacao == null)
                    return NotFound("Movimentação não encontrada.");

                return Ok(movimentacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<MovimentacaoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public IActionResult ListarMovimentacoesDoEstoque()
        {
            try
            {
                var movimentacaoEst = _service.ListarEstoqueMovimentacao();

                return Ok(movimentacaoEst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pedido/{pedidoId:int}")]
        [ProducesResponseType(typeof(ICollection<MovimentacaoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult ListarMovimentacoesPorPedido(int pedidoId)
        {
            try
            {
                return Ok(_service.ListarMovimentacoesPorPedido(pedidoId));
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

        [HttpGet("entrada")]
        [ProducesResponseType(typeof(ICollection<MovimentacaoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public IActionResult ListarMovimentacoesEntrada()
        {
            try
            {
                return Ok(_service.ListarMovimentacoesEntrada());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("saida")]
        [ProducesResponseType(typeof(ICollection<MovimentacaoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public IActionResult ListarMovimentacoesSaida()
        {
            try
            {
                return Ok(_service.ListarMovimentacoesSaida());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}