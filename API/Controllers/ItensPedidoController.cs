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
    }
}