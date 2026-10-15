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
            try
            {
                //nome = String.Concat(char.ToUpper(nome[0]), nome.Substring(1));
                var unidade = _service.BuscarPorNome(nome);

                if (unidade == null)
                    return NotFound("Categoria não encontrada.");

                return Ok(unidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id:int}")]
        public IActionResult BuscarUnidadePorId(int id)
        {
            try
            {
                var unidade = _service.BuscaPorId(id);

                if (unidade == null)
                    return NotFound("Categoria não encontrada.");

                return Ok(unidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult ListarUnidades()
        {
            try
            {
                var unidade = _service.ListarUnidades();

                return Ok(unidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:int}/cardapio")]
        public IActionResult ConsultarCardapio(int id)
        {
            try
            {
                var unidade = _service.ConsultarCardapio(id);
                return Ok(unidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CriarUnidade(UnidadesEstabelecimento unidade)
        {
            try
            {
                var unidadeEstabelecimentoCriado = _service.CriarUnidade(unidade);

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UnidadeUpdate(int id, UnidadesEstabelecimento unidades)
        {
            try
            {
                var unidade = _service.AtualizarUnidade(id, unidades);

                if (unidade == null)
                    return NotFound("Unidade Estabelecimento não encontrada.");

                return Ok("Unidade Atualizada!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}/desativar")]
        public IActionResult DesativarUnidade(int id)
        {
            try
            {
                var unidade = _service.DesativarUnidade(id);

                if (unidade == null)
                    return NotFound("Unidade não encontrada.");

                return Ok("Unidade desativada com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
