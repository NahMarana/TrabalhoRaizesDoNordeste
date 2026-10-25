using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Enums;
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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}")]
        [HttpGet("buscar")]
        [ProducesResponseType(typeof(List<UsuarioServiceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(UsuarioServiceDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}")]
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<UsuarioServiceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}")]
        [HttpGet("funcionarios/inativos")]
        [ProducesResponseType(typeof(ICollection<UsuarioServiceDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
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
        [ProducesResponseType(typeof(UsuarioServiceDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        public IActionResult CriarUsuario(Usuario usuario)
        {
            try
            {
                if (usuario.Perfil != TipoPerfil.CLIENTE
                    && !User.IsInRole(nameof(TipoPerfil.GERENTE))
                    && !User.IsInRole(nameof(TipoPerfil.ADMIN)))
                {
                    return StatusCode(403, "Somente Gerente e Administrador podem cadastrar funcionários.");
                }

                var UsuarioCriado = _service.CriarUsuario(usuario);

                return Ok(_service.CriarObjeto(UsuarioCriado));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.CLIENTE)}")]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult UsuarioUpdate(int id, Usuario usuario)
        {
            try
            {
                if (User.IsInRole(nameof(TipoPerfil.CLIENTE)))
                {
                    var idDoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!int.TryParse(idDoToken, out var idLogado) || idLogado != id)
                        return StatusCode(403, "Você só pode alterar o seu próprio cadastro.");

                    usuario.Perfil = TipoPerfil.CLIENTE;
                }

                var usuarios = _service.AtualizarUsuario(id, usuario);

                if (usuarios == null)
                    return NotFound("Usuário não encontrado(a).");

                return Ok("Usuário(a) Atualizado(a)!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}")]
        [HttpPatch("{id}/desativar")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
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
