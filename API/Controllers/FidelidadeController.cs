using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FidelidadeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly FidelidadeService _service;

        public FidelidadeController(AppDbContext appDbContext, FidelidadeService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(FidDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult BuscarFidelizacaoPorId(int id)
        {
            try
            {
                var fidelidade = _service.BuscaPorId(id);

                if (fidelidade == null)
                    return NotFound("Fidelidade não encontrada.");

                return Ok(fidelidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<FidDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public IActionResult ListarFidelidades()
        {
            try
            {
                var fidelidade = _service.ListarFidelidades();

                return Ok(fidelidade);
            } 
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.CLIENTE)}")]
        [HttpGet("meu-saldo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult ConsultarSaldo()
        {
            try
            {
                var idDoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(idDoToken, out var usuarioId))
                    return Unauthorized("Sua sessão expirou. Faça login novamente.");

                var fidelidadeId = _service.BuscaFidelidadePorUsuario(usuarioId);

                if (fidelidadeId <= 0)
                    return NotFound("Você não participa do programa de fidelidade.");

                var saldo = _service.ConsultarSaldo(fidelidadeId);

                return Ok(new { fidelidadeId, saldoPontos = saldo });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}