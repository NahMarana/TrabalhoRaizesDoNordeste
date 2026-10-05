using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public PedidosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarPedidoPorId(int id)
        {
            var pedido = _appDbContext.Pedidos.Find(id);

            if (pedido == null)
                return NoContent();

            return Ok(pedido);
        }

        [HttpGet]
        public IActionResult ListarPedidos()
        {
            var pedido = _appDbContext.Pedidos.ToList();

            return Ok(pedido);
        }

        [HttpPost]
        public IActionResult CriarPedido(Pedido pedido)
        {
            _appDbContext.Pedidos.Add(pedido);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult PedidoUpdate(int id, Pedido pedido)
        {
            var pedidos = _appDbContext.Pedidos.Find(id);
            if (pedidos == null)
                return NoContent();

            pedidos.CanalPedido = pedido.CanalPedido;
            pedidos.StatusPedido = pedido.StatusPedido;
            pedidos.ModoReceber = pedido.ModoReceber;
            pedidos.ValorTotalPedido = pedido.ValorTotalPedido;
            pedidos.DataHoraPedido = pedido.DataHoraPedido;
            pedidos.Descricao = pedido.Descricao;

            _appDbContext.Pedidos.Update(pedidos);
            _appDbContext.SaveChanges();

            return Ok("Pedido atualizado!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePedido(int id)
        {
            var pedido = _appDbContext.Pedidos.Find(id);
            if (pedido == null)
                return NoContent();

            _appDbContext.Pedidos.Remove(pedido);
            _appDbContext.SaveChanges();

            return Ok("Pedido deletado com sucesso!");
        }
    }
}