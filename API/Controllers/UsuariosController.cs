using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public UsuariosController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet("buscar")]
        public IActionResult BuscarUsuarioPorNome(string nome)
        {
            nome = String.Concat(char.ToUpper(nome[0]), nome.Substring(1));
            var usuario = _appDbContext.Usuarios
              .Where(c => c.Nome.Contains(nome))
              .ToList();

            if (!usuario.Any())
                return NotFound("Usuário não encontrado.");

            return Ok(usuario);
        }

        [HttpGet("{id:int}")]
        public IActionResult BuscarUsuarioPorId(int id)
        {
            var usuario = _appDbContext.Usuarios.Find(id);

            if (usuario == null)
                return NoContent();

            return Ok(usuario);
        }

        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            var usuarios = _appDbContext.Usuarios.ToList();

            return Ok(usuarios);
        }

        [HttpPost]
        public IActionResult CriarUsuario(Usuario usuario)
        {
            _appDbContext.Usuarios.Add(usuario);
            _appDbContext.SaveChanges();

            return Created();
        }

        [HttpPatch("{id}")]
        public IActionResult UsuarioUpdate(int id, Usuario usuario)
        {
            var usuarios = _appDbContext.Usuarios.Find(id);
            if (usuarios == null)
                return NoContent();

            usuarios.Nome = usuario.Nome;
            usuarios.Email = usuario.Email;
            usuarios.CPF = usuario.CPF;
            usuarios.Telefone = usuario.Telefone;
            usuarios.Perfil = usuario.Perfil;
            usuarios.Rua = usuario.Rua;
            usuarios.Numero = usuario.Numero;
            usuarios.Complemento = usuario.Complemento;
            usuarios.Bairro = usuario.Bairro;
            usuarios.Cidade = usuario.Cidade;
            usuarios.Estado = usuario.Estado;
            usuarios.CEP = usuario.CEP;
            usuarios.SenhaHash = usuario.SenhaHash;
            usuarios.ConsentimentoLGPD = usuario.ConsentimentoLGPD;
            usuarios.ParticipaFidelidade = usuario.ParticipaFidelidade;
            usuarios.UsuarioAtivo = usuario.UsuarioAtivo;

            _appDbContext.Usuarios.Update(usuarios);
            _appDbContext.SaveChanges();

            return Ok("Usuário atualizado!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            var usuario = _appDbContext.Usuarios.Find(id);
            if (usuario == null)
                return NoContent();

            _appDbContext.Usuarios.Remove(usuario);
            _appDbContext.SaveChanges();

            return Ok("Usuário deletado com sucesso!");
        }
    }
}
