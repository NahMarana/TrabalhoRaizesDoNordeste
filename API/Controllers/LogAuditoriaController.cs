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
    [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
    public class LogAuditoriaController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly LogAuditoriaService _service;

        public LogAuditoriaController(AppDbContext appDbContext, LogAuditoriaService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LogAuditoriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult BuscarAuditoriaPorId(int id)
        {
            try
            {
                var idDoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(idDoToken, out var usuarioId))
                    return Unauthorized("Sua sessão expirou. Faça login novamente.");

                var auditoria = _service.BuscaPorId(id, usuarioId);

                if (auditoria == null)
                    return NotFound("Auditoria não encontrada.");

                return Ok(auditoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        [ProducesResponseType(typeof(ICollection<LogAuditoriaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public IActionResult ListarAuditorias()
        {
            try
            {
                var idDoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(idDoToken, out var usuarioId))
                    return Unauthorized("Sua sessão expirou. Faça login novamente.");

                return Ok(_service.ListarAuditorias(usuarioId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}