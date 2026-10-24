using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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