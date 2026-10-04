using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogAuditoriaController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public LogAuditoriaController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarAuditoriaPorId(int id)
        {
            var auditoria = _appDbContext.LogsAuditoria.Find(id);

            if (auditoria == null)
                return NoContent();

            return Ok(auditoria);
        }

        [HttpGet]
        public IActionResult ListarAuditorias()
        {
            var auditoria = _appDbContext.LogsAuditoria.ToList();

            return Ok(auditoria);
        }

        [HttpPost]
        public IActionResult CriarLogAuditoria(LogAuditoria auditoria)
        {
            _appDbContext.LogsAuditoria.Add(auditoria);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult AuditoriaUpdate(int id, LogAuditoria auditoria)
        {
            var logs = _appDbContext.LogsAuditoria.Find(id);
            if (logs == null)
                return NoContent();

            logs.Acao = auditoria.Acao;
            logs.EntidadeAfetada = auditoria.EntidadeAfetada;
            logs.EntidadeAfetadaId = auditoria.EntidadeAfetadaId;
            logs.DadosNovos = auditoria.DadosNovos;
            logs.DadosAnteriores = auditoria.DadosAnteriores;
            logs.DataOrigem = auditoria.DataOrigem;

            _appDbContext.LogsAuditoria.Update(logs);
            _appDbContext.SaveChanges();

            return Ok("Auditoria atualizada!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarAuditoria(int id)
        {
            var auditoria = _appDbContext.LogsAuditoria.Find(id);
            if (auditoria == null)
                return NoContent();

            _appDbContext.LogsAuditoria.Remove(auditoria);
            _appDbContext.SaveChanges();

            return Ok("Auditoria deletada com sucesso!");
        }
    }
}