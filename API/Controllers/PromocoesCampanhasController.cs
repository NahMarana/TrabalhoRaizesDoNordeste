using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocoesCampanhasController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public PromocoesCampanhasController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarPromocoesECampanhasPorId(int id)
        {
            try
            {
                var pc = _appDbContext.PromocoesCampanha.Find(id);

                if (pc == null)
                    return NoContent();

                return Ok(pc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarPromocoesECampanhas()
        {
            try
            {
                var pc = _appDbContext.PromocoesCampanha.ToList();

                return Ok(pc);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarPromocoesECampanhas(PromocoesCampanhas pc)
        {
            try
            {
                _appDbContext.PromocoesCampanha.Add(pc);
                _appDbContext.SaveChanges();

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PromoECampanhasUpdate(int id, PromocoesCampanhas promoCampanha)
        {
            try
            {
                var pc = _appDbContext.PromocoesCampanha.Find(id);
                if (pc == null)
                    return NoContent();

                pc.EstabelecimentoId = promoCampanha.EstabelecimentoId;
                pc.ProdutoId = promoCampanha.ProdutoId;
                pc.Descricao = promoCampanha.Descricao;
                pc.ValorPromocional = promoCampanha.ValorPromocional;
                pc.StatusPromoCampanha = promoCampanha.StatusPromoCampanha;
                pc.DataInicio = promoCampanha.DataInicio;
                pc.DataFim = promoCampanha.DataFim;


                _appDbContext.PromocoesCampanha.Update(pc);
                _appDbContext.SaveChanges();

                return Ok("Promoções ou Campanha atualizada!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarPromoECampanha(int id)
        {
            var pc = _appDbContext.PromocoesCampanha.Find(id);
            if (pc == null)
                return NoContent();

            _appDbContext.PromocoesCampanha.Remove(pc);
            _appDbContext.SaveChanges();

            return Ok("Promoção ou Campanha deletada com sucesso!");
        }
    }
}