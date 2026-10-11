using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id:int}")]
        public IActionResult BuscarPedidoPorId(int id)
        {
            try
            {
                var pedido = _service.BuscaPorId(id);

                if (pedido == null)
                    return NotFound("Produto não encontrado.");

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarPedidos()
        {
            try
            {
                var pedido = _service.ListarPedidos();

                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("canal")]
        public IActionResult ListarPedidosPorCanal(CanalPedido canalPedido)
        {
            try
            {
                return Ok(_service.ListarPedidosPorCanal(canalPedido));
            }
            catch (ArgumentException ex) { return BadRequest(ex.Message); }
        }

        [HttpPost]
        public IActionResult CriarPedido(Pedido pedido)
        {
            try
            {
                var pedidoCriado = _service.CriarPedido(pedido);

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PedidoUpdate(int id, Pedido pedido)
        {
            try
            {
                var pedidos = _service.AtualizarPedido(id, pedido);

                if (pedidos == null)
                    return NotFound("Pedido não encontrado.");

                return Ok("Pedido atualizado!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id:int}/statusPedido")]
        public IActionResult AtualizarStatusPedido(int id, StatusPedido statusPedido)
        {
            try
            {
                var pedido = _service.AtualizarStatusPedido(id, statusPedido);
                return Ok(_service.CriarObjeto(pedido));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult CancelarPedido(int id)
        {
            try
            {
                var pedido = _service.CancelarPedido(id);
                return Ok("Pedido cancelado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}