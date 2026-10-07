using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesEstabelecimentoController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UnidadeEstabelecimentoService _service;

        public UnidadesEstabelecimentoController(AppDbContext appDbContext, UnidadeEstabelecimentoService service)
        {
            _appDbContext = appDbContext;
            _service = service;
        }


        [HttpGet("buscar")]
        public IActionResult BuscarEstabelecimentoPorNome(string nome)
        {
            //nome = String.Concat(char.ToUpper(nome[0]), nome.Substring(1));
            var unidade = _service.BuscarPorNome(nome);

            if (unidade == null)
                return NotFound("Categoria não encontrada.");

            return Ok(unidade);
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarUnidadePorId(int id)
        {
            var unidade = _service.BuscaPorId(id);

            if (unidade == null)
                return NotFound("Categoria não encontrada.");

            return Ok(unidade);
        }

        [HttpGet]
        public IActionResult ListarUnidades()
        {
            var unidade = _service.ListarUnidades();

            return Ok(unidade);
        }

        [HttpPost]
        public IActionResult CriarUnidade(UnidadesEstabelecimento unidade)
        {
            var unidadeEstabelecimentoCriado = _service.CriarUnidade(unidade);

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult UnidadeUpdate(int id, UnidadesEstabelecimento unidades)
        {
            var unidade = _service.AtualizarUnidade(id, unidades);

            if (unidade == null)
                return NotFound("Unidade Estabelecimento não encontrada.");

            return Ok("Unidade Atualizada!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUnidade(int id)
        {
            var unidade = _service.DeletarUnidade(id);
            if (unidade == null)

                return NoContent();

            return Ok("Unidade Estabelecimento deletado com sucesso!");
        }

    }
}
