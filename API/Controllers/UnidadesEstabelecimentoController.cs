using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnidadesEstabelecimentoController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public UnidadesEstabelecimentoController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        [HttpGet("buscar")]
        public IActionResult BuscarEstabelecimentoPorNome(string nome)
        {
            nome = String.Concat(char.ToUpper(nome[0]), nome.Substring(1));
            var unidade = _appDbContext.UnidadesEstabelecimento
              .Where(c => c.NomeEstabelecimento.Contains(nome))
              .ToList();

            if (!unidade.Any())
                return NotFound("Unidade não encontrada.");

            return Ok(unidade);
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarUnidadePorId(int id)
        {
            var unidade = _appDbContext.UnidadesEstabelecimento.Find(id);

            if (unidade == null)
                return NoContent();

            return Ok(unidade);
        }

        [HttpGet]
        public IActionResult ListarUnidades()
        {
            var unidades = _appDbContext.UnidadesEstabelecimento.ToList();

            return Ok(unidades);
        }

        [HttpPost]
        public IActionResult CriarUnidade(UnidadesEstabelecimento unidade)
        {
            _appDbContext.UnidadesEstabelecimento.Add(unidade);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult UnidadeUpdate(int id, UnidadesEstabelecimento unidades)
        {
            var unidade = _appDbContext.UnidadesEstabelecimento.Find(id);
            if (unidade == null)
                return NoContent();

            unidade.NomeEstabelecimento = unidades.NomeEstabelecimento;
            unidade.CNPJ = unidades.CNPJ;
            unidade.UnidadeAtiva = unidades.UnidadeAtiva;
            unidade.TipoUnidade = unidades.TipoUnidade;
            unidade.Rua = unidades.Rua;
            unidade.Numero = unidades.Numero;
            unidade.Bairro = unidades.Bairro;
            unidade.Cidade = unidades.Cidade;
            unidade.Estado = unidades.Estado;
            unidade.CEP = unidades.CEP;

            _appDbContext.UnidadesEstabelecimento.Update(unidade);
            _appDbContext.SaveChanges();

            return Ok("Unidade atualizada!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUnidade(int id)
        {
            var unidade = _appDbContext.UnidadesEstabelecimento.Find(id);
            if (unidade == null)
                return NoContent();

            _appDbContext.UnidadesEstabelecimento.Remove(unidade);
            _appDbContext.SaveChanges();

            return Ok("Unidade deletada com sucesso!");
        }
    }
}
