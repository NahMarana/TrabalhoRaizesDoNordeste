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
                    return NoContent();

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
                var auditoria = _service.ListarAuditorias(usuarioId);

                return Ok(auditoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //[HttpPost]
        //public IActionResult CriarLogAuditoria(LogAuditoria auditoria)
        //{
        //    try
        //    {
        //        auditoria = _service.CriarAuditoria(auditoria);
        //        return Created();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPatch("{id}")]
        //public IActionResult AuditoriaUpdate(int id, LogAuditoria auditoria)
        //{
        //    var logs = _appDbContext.LogsAuditoria.Find(id);
        //    if (logs == null)
        //        return NoContent();

        //    logs.Acao = auditoria.Acao;
        //    logs.EntidadeAfetada = auditoria.EntidadeAfetada;
        //    logs.EntidadeAfetadaId = auditoria.EntidadeAfetadaId;
        //    logs.DadosNovos = auditoria.DadosNovos;
        //    logs.DadosAnteriores = auditoria.DadosAnteriores;
        //    logs.DataOrigem = auditoria.DataOrigem;

        //    _appDbContext.LogsAuditoria.Update(logs);
        //    _appDbContext.SaveChanges();

        //    return Ok("Auditoria atualizada!");
        //}
    }
}