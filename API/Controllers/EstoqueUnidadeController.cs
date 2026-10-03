using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueUnidadeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public EstoqueUnidadeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarEstoqueDeUnidadePorId(int id)
        {
            var unidadeEstoque = _appDbContext.EstoquesUnidade.Find(id);

            if (unidadeEstoque == null)
                return NoContent();

            return Ok(unidadeEstoque);
        }

        [HttpGet]
        public IActionResult ListarEstoqueDeUnidades()
        {
            var unidadeEstoque = _appDbContext.EstoquesUnidade.ToList();

            return Ok(unidadeEstoque);
        }

        [HttpPost]
        public IActionResult CriarEstoqueDeUnidade(EstoqueUnidade unidadeEstoque)
        {
            _appDbContext.EstoquesUnidade.Add(unidadeEstoque);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult EstoqueDeUnidadeUpdate(int id, EstoqueUnidade unidadeEstoque)
        {
            var estUnidade = _appDbContext.EstoquesUnidade.Find(id);
            if (estUnidade == null)
                return NoContent();

            estUnidade.ProdutoId = unidadeEstoque.ProdutoId;
            estUnidade.QtdEstoqueDisponivel = unidadeEstoque.QtdEstoqueDisponivel;
            estUnidade.VendaDisponivel = unidadeEstoque.VendaDisponivel;

            _appDbContext.EstoquesUnidade.Update(estUnidade);
            _appDbContext.SaveChanges();

            return Ok("Unidade de estoque atualizado!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarUnidadeDeEstoque(int id)
        {
            var unidadeEstoque = _appDbContext.EstoquesUnidade.Find(id);
            if (unidadeEstoque == null)
                return NoContent();

            _appDbContext.EstoquesUnidade.Remove(unidadeEstoque);
            _appDbContext.SaveChanges();

            return Ok("Unidade de Estoque foi deletado com sucesso!");
        }
    }
}