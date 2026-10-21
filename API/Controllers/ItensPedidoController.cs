using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensPedidoController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ItensPedidoService _service;

        public ItensPedidoController(AppDbContext appDbContext, ItensPedidoService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("pedido/{pedidoId:int}")]
        public IActionResult BuscarItensPorIdPedido(int pedidoId)
        {
            try
            {
                return Ok(_service.BuscaItensPorIdPedido(pedidoId));
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

        [HttpGet]
        public IActionResult ListarItensPedidos()
        {
            try
            {
                var item = _service.ListarItensPedidos();

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult ItemPedidoUpdate(int id, ItensPedido item)
        {
            try
            {
                var itemAtualizado = _service.AtualizarItemPedido(id, item);

                if (itemAtualizado == null)
                    return Ok("Item removido do pedido.");

                return Ok("Os itens desse pedido foram atualizados!");
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