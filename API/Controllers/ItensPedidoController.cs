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

        [HttpGet("{id:int}")]
        public IActionResult BuscarItemPedidoPorId(int id)
        {
            try
            {
                var item = _service.BuscaPorId(id);

                if (item == null)
                    return NotFound("Item do Produto não encontrado.");

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

        [HttpPost]
        public IActionResult CriarItensPedido(ItensPedido itens)
        {
            try
            {
                var ItemPedidoCriado = _service.CriarItemPedido(itens);
                return Created();
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
                    return NotFound("Item não encontrado.");

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