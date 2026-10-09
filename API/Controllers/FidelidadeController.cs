using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FidelidadeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly FidelidadeService _service;

        public FidelidadeController(AppDbContext appDbContext, FidelidadeService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarFidelizacaoPorId(int id)
        {
            try
            {
                var fidelidade = _service.BuscaPorId(id);

                if (fidelidade == null)
                    return NotFound("Fidelidade não encontrada.");

                return Ok(fidelidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public IActionResult ListarFidelidades()
        {
            try
            {
                var fidelidade = _service.ListarFidelidades();

                return Ok(fidelidade);
            } 
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarFidelizacao(UsuarioFidelidadeDTO usuario)
        {
            try
            {
                var fidelidadeCriada = _service.CriarFidelidade(usuario);

                return Created();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult FidelizacaoUpdate(int id, Fidelidade fidelidade)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarFidelidade(int id)
        {
            throw new NotImplementedException();
        }
    }
}