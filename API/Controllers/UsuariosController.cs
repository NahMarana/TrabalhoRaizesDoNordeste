using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.DTOs;
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
        public IActionResult BuscarUsuarioPorNome([FromQuery] string? nome)
        {
            //nome = String.Concat(char.ToUpper(nome[0]), nome.Substring(1));
            try
            {
                var usuario = _service.BuscarPorNome(nome);

                if (usuario.Count == 0)
                    return NotFound("Usuário não encontrado.");

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarUsuarioPorId(int id)
        {
            try
            {
                var usuario = _service.BuscaPorId(id);

                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            try
            {
                var usuario = _service.ListarUsuarios();

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("funcionarios/inativos")]
        public IActionResult ListarFuncionariosInativos()
        {
            try
            {
                return Ok(_service.ListarFuncionariosInativos());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        [HttpPatch("{id}/desativar")]
        public IActionResult DesativarUsuario(int id)
        {
            try
            {
                var usuario = _service.DesativarUsuario(id);

                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                return Ok("Usuário desativado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

    }
}
