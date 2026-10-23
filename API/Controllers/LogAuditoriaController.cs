using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public IActionResult BuscarAuditoriaPorId(int id, int usuarioId)
        {
            try
            {
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
        public IActionResult ListarAuditorias(int usuarioId)
        {
            try
            {
                return Ok(_service.ListarAuditorias(usuarioId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}