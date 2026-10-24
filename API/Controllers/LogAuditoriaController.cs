using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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