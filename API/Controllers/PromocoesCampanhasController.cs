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
            var pc = _appDbContext.PromocoesCampanha.Find(id);

            if (pc == null)
                return NoContent();

            return Ok(pc);
        }

        [HttpGet]
        public IActionResult ListarPromocoesECampanhas()
        {
            var pc = _appDbContext.PromocoesCampanha.ToList();

            return Ok(pc);
        }

        [HttpPost]
        public IActionResult CriarPromocoesECampanhas(PromocoesCampanhas pc)
        {
            _appDbContext.PromocoesCampanha.Add(pc);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult PromoECampanhasUpdate(int id, PromocoesCampanhas promoCampanha)
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