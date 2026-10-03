using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensPedidoController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public ItensPedidoController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarItemPedidoPorId(int id)
        {
            var item = _appDbContext.ItensPedido.Find(id);

            if (item == null)
                return NoContent();

            return Ok(item);
        }

        [HttpGet]
        public IActionResult ListarItensPedidos()
        {
            var item = _appDbContext.ItensPedido.ToList();

            return Ok(item);
        }

        [HttpPost]
        public IActionResult CriarItensPedido(ItensPedido itens)
        {
            _appDbContext.ItensPedido.Add(itens);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult ItemPedidoUpdate(int id, ItensPedido item)
        {
            var itens = _appDbContext.ItensPedido.Find(id);
            if (itens == null)
                return NotFound("Item não encontrado.");

            itens.ProdutoId = item.ProdutoId;
            itens.QtdItens = item.QtdItens;
            itens.PrecoUnitario = item.PrecoUnitario;
            itens.PrecoTotal = item.PrecoTotal;

            _appDbContext.ItensPedido.Update(itens);
            _appDbContext.SaveChanges();

            if (itens.QtdItens < 0)
                return BadRequest("Não permitido quantidade negativa.");

            if (itens.QtdItens == 0)
            {
                _appDbContext.ItensPedido.Remove(itens);
                _appDbContext.SaveChanges();

                return Ok("Item foi removido do pedido.");
            }

            _appDbContext.SaveChanges();

            return Ok("Os itens desse pedido foram atualizados!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTodosItensPedidos(int id)
        {
            var item = _appDbContext.ItensPedido.Find(id);
            if (item == null)
                return NoContent();

            _appDbContext.ItensPedido.Remove(item);
            _appDbContext.SaveChanges();

            return Ok("Os itens do seu pedido foram deletados com sucesso!");
        }
    }
}