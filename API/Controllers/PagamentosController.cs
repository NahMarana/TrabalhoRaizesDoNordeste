using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly PagamentoService _service;

        public PagamentosController(AppDbContext appDbContext, PagamentoService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarPagamentoPorId(int id)
        {
            try
            {
                var pagamento = _service.BuscaPorId(id);

                if (pagamento == null)
                    return NotFound("Pagamento não encontrado.");

                return Ok(pagamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarPagamentos()
        {
            try
            {
                var pagamento = _service.ListarPagamentos();

                return Ok(pagamento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarPagamento(Pagamento pagamento, bool usarPontos = false)
        {
            try
            {
                var pagamentoCriado = _service.CriarPagamento(pagamento, usarPontos);
                return Created();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult PagamentoUpdate(int id, Pagamento pagamento)
        {
            var pagamentos = _service.AtualizarPagamento(id, pagamento);
            if (pagamentos == null)
                return NoContent();

            return Ok("Pagamento atualizado!");
        }
    }
}