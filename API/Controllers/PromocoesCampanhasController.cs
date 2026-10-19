using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocoesCampanhasController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly PromocoesCampanhasService _service;

        public PromocoesCampanhasController(AppDbContext appDbContext, PromocoesCampanhasService service)
        {
            _appDbContext = appDbContext;
            _service = service;

        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarPromocoesECampanhasPorId(int id)
        {
            try
            {
                var promo = _service.BuscaPorId(id);

                if (promo == null)
                    return NotFound("Promoção não encontrada.");

                return Ok(promo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarPromocoesECampanhasAtivas()
        {
            try
            {
                var promo = _service.ListarPromocoesCampanhasAtivas();

                return Ok(promo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("promocoesCampanhas/inativos")]
        public IActionResult ListarPromocoesCampanhasInativas()
        {
            try
            {
                return Ok(_service.ListarPromocoesCampanhasInativas());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarPromocoesECampanhas(PromocoesCampanhas promo)
        {
            try
            {
                var promoCriada = _service.CriarPromoCampanha(promo);

                return Created();
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
            catch (KeyNotFoundException ex) { return NotFound(ex.Message); }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PromoUpdate(int id, PromocoesCampanhas promos)
        {
            try
            {
                var promo = _service.AtualizarPromoCampanha(id, promos);

                if (promo == null)
                    return NotFound("Promoção não encontrada.");

                return Ok("Promoção Atualizada!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/desativar")]
        public IActionResult DesativarPromo(int id)
        {
            try
            {
                var promo = _service.DesativarPromo(id);

                if (promo == null)
                    return NotFound("Promoção não encontrada.");

                return Ok("Promoção desativada com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}