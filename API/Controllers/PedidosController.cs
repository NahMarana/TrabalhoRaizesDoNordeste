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
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly PedidoService _service;

        public PedidosController(AppDbContext appDbContext, PedidoService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.COZINHA)}, " +
        $"{nameof(TipoPerfil.ATENDENTE)}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult BuscarPedidoPorId(int id)
        {
            try
            {
                var pedido = _service.BuscaPorId(id);

                if (pedido == null)
                    return NotFound("Pedido não encontrado.");

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.COZINHA)}, " +
        $"{nameof(TipoPerfil.ATENDENTE)}")]
        [HttpGet("ativos")]
        [ProducesResponseType(typeof(ICollection<PedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public IActionResult ListarPedidos()
        {
            try
            {
                var pedido = _service.ListarPedidosAtivos();

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.COZINHA)}, " +
        $"{nameof(TipoPerfil.ATENDENTE)}")]
        [HttpGet("cancelados")]
        [ProducesResponseType(typeof(ICollection<PedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public IActionResult ListarPedidosCancelados()
        {
            try
            {
                var pedido = _service.ListarPedidosCancelados();

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}")]
        [HttpGet("canal")]
        [ProducesResponseType(typeof(ICollection<PedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public IActionResult ListarPedidosPorCanal(int canalPedido)
        {
            try
            {
                return Ok(_service.ListarPedidosPorCanal((CanalPedido)canalPedido));
            }
            catch (ArgumentException ex) 
            { 
                return BadRequest(ex.Message); 
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}, {nameof(TipoPerfil.CLIENTE)}")]
        [HttpGet("cliente/{usuarioId:int}")]
        [ProducesResponseType(typeof(ICollection<RetornoDoPedidoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public IActionResult BuscarPedidosDoCliente(int usuarioId)
        {
            try
            {
                if (User.IsInRole(nameof(TipoPerfil.CLIENTE)))
                {
                    var idDoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!int.TryParse(idDoToken, out var idLogado) || idLogado != usuarioId)
                        return StatusCode(403, "Você só pode consultar os seus próprios pedidos.");
                }

                return Ok(_service.BuscarPedidosDoCliente(usuarioId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, " +
        $"{nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}, {nameof(TipoPerfil.CLIENTE)}")]
        [HttpPost]
        [ProducesResponseType(typeof(RetornoDoPedidoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public IActionResult CriarPedido(Pedido pedido)
        {
            try
            {
                if (User.IsInRole(nameof(TipoPerfil.CLIENTE)))
                {
                    var idDoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!int.TryParse(idDoToken, out var usuarioId))
                        return Unauthorized("Sua sessão expirou. Faça login novamente.");

                    pedido.UsuarioId = usuarioId;
                }

                var pedidoCriado = _service.CriarPedido(pedido);

                return Ok(_service.CriarRetornoCliente(pedidoCriado));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.ATENDENTE)}, {nameof(TipoPerfil.CLIENTE)}")]
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(RetornoDoPedidoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public IActionResult PedidoUpdate(int id, Pedido pedido)
        {
            try
            {
                int? clienteId = null;

                if (User.IsInRole(nameof(TipoPerfil.CLIENTE)))
                {
                    var idDoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!int.TryParse(idDoToken, out var usuarioId))
                        return Unauthorized("Sua sessão expirou. Faça login novamente.");

                    clienteId = usuarioId;
                }

                var pedidoAtualizado = _service.AtualizarPedido(id, pedido, clienteId);

                if (pedidoAtualizado == null)
                    return NotFound("Pedido não encontrado.");

                return Ok(_service.CriarRetornoCliente(pedidoAtualizado));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.ATENDENTE)}, {nameof(TipoPerfil.COZINHA)}")]
        [HttpPatch("{id:int}/statusPedido")]
        [ProducesResponseType(typeof(PedidoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public IActionResult AtualizarStatusPedido(int id, int statusPedido)
        {
            try
            {
                var pedido = _service.AtualizarStatusPedido(id, (StatusPedido)statusPedido);

                if (pedido == null)
                    return NotFound("Pedido não encontrado.");

                return Ok(_service.CriarObjeto(pedido));
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

        [Authorize(Roles = $"{nameof(TipoPerfil.ATENDENTE)}, {nameof(TipoPerfil.CLIENTE)}")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        public IActionResult CancelarPedido(int id)
        {
            try
            {
                int? clienteId = null;

                if (User.IsInRole(nameof(TipoPerfil.CLIENTE)))
                {
                    var idDoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (!int.TryParse(idDoToken, out var usuarioId))
                        return Unauthorized("Sua sessão expirou. Faça login novamente.");

                    clienteId = usuarioId;
                }

                var pedido = _service.CancelarPedido(id, clienteId);

                if (pedido == null)
                    return NotFound("Pedido não encontrado.");

                return Ok("Pedido cancelado com sucesso!");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}