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

    public class PagamentosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly PagamentoService _service;

        public PagamentosController(AppDbContext appDbContext, PagamentoService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}")]
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

        [Authorize(Roles = $"{nameof(TipoPerfil.GERENTE)}, {nameof(TipoPerfil.ADMIN)}, {nameof(TipoPerfil.ATENDENTE)}")]
        [HttpGet]
        public IActionResult ListarPagamentos()
        {
            try
            {
                return Ok(_service.ListarPagamentos());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = $"{nameof(TipoPerfil.CLIENTE)}, {nameof(TipoPerfil.ATENDENTE)}")]
        [HttpPost]
        public IActionResult CriarPagamento(Pagamento pagamento, bool usarPontos = false, bool simularRecusa = false)
        {
            try
            {
                var payload = _service.CriarPagamento(pagamento, usarPontos, simularRecusa);

                return Ok(payload);
            }
            catch(ArgumentException ex)
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
    }
}