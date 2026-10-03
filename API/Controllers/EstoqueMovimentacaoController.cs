using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueMovimentacaoController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public EstoqueMovimentacaoController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarMovimentacaoDoEstoquePorId(int id)
        {
            var movimentacaoEst = _appDbContext.EstoquesMovimentacao.Find(id);

            if (movimentacaoEst == null)
                return NoContent();

            return Ok(movimentacaoEst);
        }

        [HttpGet]
        public IActionResult ListarMovimentacoesDoEstoque()
        {
            var movimentacaoEst = _appDbContext.EstoquesMovimentacao.ToList();

            return Ok(movimentacaoEst);
        }

        [HttpPost]
        public IActionResult CriarMovimentacaoDoEstoque(EstoqueMovimentacao movimentacaoEst)
        {
            _appDbContext.EstoquesMovimentacao.Add(movimentacaoEst);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult MovimentacaoEstoqueUpdate(int id, EstoqueMovimentacao movimentacoesEst)
        {
            var movimentacaoEst = _appDbContext.EstoquesMovimentacao.Find(id);
            if (movimentacaoEst == null)
                return NoContent();

            movimentacaoEst.PedidoId = movimentacoesEst.PedidoId;
            movimentacaoEst.TipoMovimento = movimentacoesEst.TipoMovimento;
            movimentacaoEst.QtdEmEstoque = movimentacoesEst.QtdEmEstoque;
            movimentacaoEst.MotivoMovimentacao = movimentacoesEst.MotivoMovimentacao;
            movimentacaoEst.DataMovimentacao = movimentacoesEst.DataMovimentacao;

            _appDbContext.EstoquesMovimentacao.Update(movimentacaoEst);
            _appDbContext.SaveChanges();

            return Ok("Movimentação do estoque foi atualizado!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarMovimentaçãoDoEstoque(int id)
        {
            var movimentacaoEst = _appDbContext.EstoquesMovimentacao.Find(id);
            if (movimentacaoEst == null)
                return NoContent();

            _appDbContext.EstoquesMovimentacao.Remove(movimentacaoEst);
            _appDbContext.SaveChanges();

            return Ok("Movimentação do estoque foi deletado com sucesso!");
        }
    }
}