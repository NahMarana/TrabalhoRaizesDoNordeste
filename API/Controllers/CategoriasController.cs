using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CategoriasController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id}")]
        public IActionResult BuscarCategoria(int id)
        {
            var categoria = _appDbContext.Categorias.Find(id);

            if (categoria == null)
                return NoContent();

            return Ok(categoria);
        }

        [HttpGet]
        public IActionResult ListarCategorias() 
        {
            var categorias = _appDbContext.Categorias.ToList();

            return Ok(categorias);
        }

        [HttpPost]
        public IActionResult CriarCategoria(Categorias categorias)
        {
            _appDbContext.Categorias.Add(categorias);
            _appDbContext.SaveChanges();
            
            return Created();                
        }

        [HttpPatch("{id}")]
        public IActionResult CategoriaUpdate(int id, Categorias categorias)
        {
            var categoria = _appDbContext.Categorias.Find(id);
            if (categoria == null)
                return NoContent();

            categoria.NomeCategoria = categorias.NomeCategoria;
            categoria.CategoriaAtiva = categorias.CategoriaAtiva;

            _appDbContext.Categorias.Update(categoria);
            _appDbContext.SaveChanges();

            return Ok("Categoria Atualizada!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategoria(int id)
        {
            var categoria = _appDbContext.Categorias.Find(id);
            if (categoria == null)
                return NoContent();

            _appDbContext.Categorias.Remove(categoria);
            _appDbContext.SaveChanges();

            return Ok("Categoria deletada com sucesso!");
        }

    }
}