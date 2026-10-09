using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PontosFidelidadeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly PontosFidelidadeService _service;

        public PontosFidelidadeController(AppDbContext appDbContext, PontosFidelidadeService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarPontosPorId(int id)
        {
            try
            {
                var pontos = _service.BuscaPorId(id);

                if (pontos == null)
                    return NotFound("Pontos por fidelidade não encontrada.");

                return Ok(pontos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarPontosPorFidelidade()
        {
            try
            {
                var pontos = _service.ListarPontosFidelidade();

                return Ok(pontos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarPontosPorFidelidade(PontosFidelidade pontos)
        {
            try
            {
                var pontosCriados = _service.CriarPontos(pontos);

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPatch("{id}")]
        public IActionResult PontosUpdate(int id, PontosFidelidade ponto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarPontos(int id)
        {
            throw new NotImplementedException();
        }
    }
}