using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueMovimentacaoController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly EstoqueMovimentacaoService _service;

        public EstoqueMovimentacaoController(AppDbContext appDbContext, EstoqueMovimentacaoService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarMovimentacaoDoEstoquePorId(int id)
        {
            try
            {
                var movimentacao = _service.BuscaPorId(id);

                if (movimentacao == null)
                    return NoContent();

                return Ok(movimentacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarMovimentacoesDoEstoque()
        {
            try
            {
                var movimentacaoEst = _service.ListarEstoqueMovimentacao();

                return Ok(movimentacaoEst);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("pedido/{pedidoId:int}")]
        public IActionResult ListarMovimentacoesPorPedido(int pedidoId)
        {
            try
            {
                return Ok(_service.ListarMovimentacoesPorPedido(pedidoId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}