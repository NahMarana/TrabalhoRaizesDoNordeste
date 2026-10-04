using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PontosFidelidadeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public PontosFidelidadeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarPontosPorId(int id)
        {
            var pontos = _appDbContext.PontosFidelidade.Find(id);

            if (pontos == null)
                return NoContent();

            return Ok(pontos);
        }

        [HttpGet]
        public IActionResult ListarPontosPorFidelidade()
        {
            var pontos = _appDbContext.PontosFidelidade.ToList();

            return Ok(pontos);
        }

        [HttpPost]
        public IActionResult CriarPontosPorFidelidade(PontosFidelidade pontos)
        {
            _appDbContext.PontosFidelidade.Add(pontos);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult PontosUpdate(int id, PontosFidelidade ponto)
        {
            var pontos = _appDbContext.PontosFidelidade.Find(id);
            if (pontos == null)
                return NoContent();

            pontos.PedidoId = ponto.PedidoId;
            pontos.TipoMovimentacaoPontos = ponto.TipoMovimentacaoPontos;
            pontos.Pontos = ponto.Pontos;
            pontos.DataPontos = ponto.DataPontos;

            _appDbContext.PontosFidelidade.Update(pontos);
            _appDbContext.SaveChanges();

            return Ok("Pontos por Fidelidade atualizada!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarPontos(int id)
        {
            var pontos = _appDbContext.PontosFidelidade.Find(id);
            if (pontos == null)
                return NoContent();

            _appDbContext.PontosFidelidade.Remove(pontos);
            _appDbContext.SaveChanges();

            return Ok("Pontos por Fidelidade deletada com sucesso!");
        }
    }
}