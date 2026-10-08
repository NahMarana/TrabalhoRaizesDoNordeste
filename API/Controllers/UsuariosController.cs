using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UsuarioService _service;

        public UsuariosController(AppDbContext appDbContext, UsuarioService service)
        {
            _appDbContext = appDbContext;
            _service = service;

        }

        [HttpGet("buscar")]
        public IActionResult BuscarUsuarioPorNome(string nome)
        {
            //nome = String.Concat(char.ToUpper(nome[0]), nome.Substring(1));
            var usuario = _service.BuscarPorNome(nome);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            return Ok(usuario);
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarUsuarioPorId(int id)
        {
            var usuario = _service.BuscaPorId(id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            return Ok(usuario);
        }

        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            var usuario = _service.ListarUsuarios();

            return Ok(usuario);
        }

        [HttpPost]
        public IActionResult CriarUsuario(Usuario usuario)
        {
            try
            {
                var UsuarioCriado = _service.CriarUsuario(usuario);

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UsuarioUpdate(int id, Usuario usuario)
        {
            try
            {
                var usuarios = _service.AtualizarUsuario(id, usuario);

                if (usuario == null)
                    return NotFound("Usuário não encontrado(a).");

                return Ok("Usuário(a) Atualizado(a)!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            var usuario = _service.DeletarUsuario(id);

            if (usuario == null)
                return NoContent();

            return Ok("Usuário deletado com sucesso!");
        }
    }
}
