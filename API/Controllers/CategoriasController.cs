using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly CategoriaService _service;

        public CategoriasController(AppDbContext appDbContext, CategoriaService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("{id}")]
        public IActionResult BuscarCategoriaPorId(int id)
        {
            try
            {
                var categoria = _service.BuscaPorId(id);

                if (categoria == null)
                    return NotFound("Categoria não encontrada.");

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("buscar")]
        public IActionResult BuscarCategoriaPorNome(string nome)
        {
            try
            {
                var categorias = _service.BuscarPorNome(nome);

                if (categorias == null)
                    return NotFound("Categoria não encontrada.");

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarCategorias() 
        {
            try
            {
                var categorias = _service.ListarCategorias();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarCategoria(Categoria categoria)
        {
           try
           {
               var categorias = _service.CriarCategoria(categoria);

               return Created();
           }
           catch (Exception ex)
           {
               return BadRequest(ex.Message);
           }
        }

        [HttpPatch("{id}")]
        public IActionResult CategoriaUpdate(int id, Categoria categoria)
        {
           try
           {
               var categorias = _service.AtualizarCategoria(id, categoria);

               if (categorias == null)
                    return NotFound("Categoria não encontrada.");

               return Ok("Categoria Atualizada!");
           }
           catch (Exception ex)
           {
               return BadRequest(ex.Message);
           }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCategoria(int id)
        {
            var categoria = _service.DeletarCategoria(id);

            if (categoria == null)
                return NoContent();

            return Ok("Categoria deletada com sucesso!");
        }
    }
}