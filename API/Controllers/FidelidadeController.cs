using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FidelidadeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public FidelidadeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarFidelizacaoPorId(int id)
        {
            var fidelidade = _appDbContext.Fidelidades.Find(id);

            if (fidelidade == null)
                return NoContent();

            return Ok(fidelidade);
        }

        [HttpGet]
        public IActionResult ListarFidelidades()
        {
            var fidelidade = _appDbContext.Fidelidades.ToList();

            return Ok(fidelidade);
        }

        [HttpPost]
        public IActionResult CriarFidelizacao(Fidelidade fidelidade)
        {
            _appDbContext.Fidelidades.Add(fidelidade);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult FidelizacaoUpdate(int id, Fidelidade fid)
        {
            var fidelizacao = _appDbContext.Fidelidades.Find(id);
            if (fidelizacao == null)
                return NoContent();

            fidelizacao.QtdPontos = fid.QtdPontos;

            _appDbContext.Fidelidades.Update(fidelizacao);
            _appDbContext.SaveChanges();

            return Ok("Fidelização atualizada!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarFidelidade(int id)
        {
            var fidelizacao = _appDbContext.Fidelidades.Find(id);
            if (fidelizacao == null)
                return NoContent();

            _appDbContext.Fidelidades.Remove(fidelizacao);
            _appDbContext.SaveChanges();

            return Ok("Fidelização deletada com sucesso!");
        }
    }
}