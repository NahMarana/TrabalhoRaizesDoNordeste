using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public PagamentosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarPagamentoPorId(int id)
        {
            var pagamento = _appDbContext.Pagamentos.Find(id);

            if (pagamento == null)
                return NoContent();

            return Ok(pagamento);
        }

        [HttpGet]
        public IActionResult ListarPagamentos()
        {
            var pagamento = _appDbContext.Pagamentos.ToList();

            return Ok(pagamento);
        }

        [HttpPost]
        public IActionResult CriarPagamento(Pagamento pagamento)
        {
            _appDbContext.Pagamentos.Add(pagamento);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult PagamentoUpdate(int id, Pagamento pagamento)
        {
            var pagamentos = _appDbContext.Pagamentos.Find(id);
            if (pagamentos == null)
                return NoContent();

            pagamentos.TipoPagamento = pagamento.TipoPagamento;
            pagamentos.StatusPagamento = pagamento.StatusPagamento;
            pagamentos.ValorPagamento = pagamento.ValorPagamento;
            pagamentos.DataPagamento = pagamento.DataPagamento;
            pagamentos.DataConfirmaPg = pagamento.DataConfirmaPg;

            _appDbContext.Pagamentos.Update(pagamentos);
            _appDbContext.SaveChanges();

            return Ok("Pagamento atualizado!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarPagamento(int id)
        {
            var pagamento = _appDbContext.Pagamentos.Find(id);
            if (pagamento == null)
                return NoContent();

            _appDbContext.Pagamentos.Remove(pagamento);
            _appDbContext.SaveChanges();

            return Ok("Pagamento deletado com sucesso!");
        }
    }
}