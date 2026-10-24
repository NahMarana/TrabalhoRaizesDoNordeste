using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}")]

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
                return Ok(_service.ListarPontosFidelidade());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}