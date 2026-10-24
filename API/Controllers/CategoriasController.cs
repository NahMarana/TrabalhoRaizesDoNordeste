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
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.COZINHA)}, " +
        $"{nameof(TipoPerfil.ATENDENTE)}")]

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
                    return NotFound("Categoria por ID não encontrada.");

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("buscar")]
        public IActionResult BuscarCategoriaPorNome([FromQuery] string? nome)
        {
            try
            {
                var categorias = _service.BuscarPorNome(nome);

                if (categorias.Count == 0)
                    return NotFound("Categoria por nome não encontrada.");

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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
        [HttpDelete("{id}")]
        public ActionResult DeleteCategoria(int id)
        {
            try
            {
                var categoriaDeletada = _service.DeletarCategoria(id);

                if (!categoriaDeletada)
                    return NotFound("Categoria ID não encontrada");

                return Ok("Categoria deletada com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}