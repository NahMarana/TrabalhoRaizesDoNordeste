using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;


namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class UsuarioRepository(AppDbContext _context)
    {
        public Usuario? BuscaUsuarioPorId(int id)
        {
            return _context.Usuarios
                .Include(ue => ue.UnidadesEstabelecimento)
                .Include(f => f.Fidelidade)
                .FirstOrDefault(u => u.Id == id);
        }

        public ICollection<Usuario>? BuscarUsuarioPorNome(string nome)
        {
            return _context.Usuarios
                .Include(ue => ue.UnidadesEstabelecimento)
                .Include(f => f.Fidelidade)
                .Where(u => u.Nome.ToLower().Contains(nome.ToLower()))
                .ToList();
        }

        public UnidadesEstabelecimento? BuscaUnidadePorId(int id)
        {
            return _context.UnidadesEstabelecimento.Find(id);
        }

        public ICollection<Usuario>? ListarUsuarios()
        {
            return _context.Usuarios
                .Include(ue => ue.UnidadesEstabelecimento)
                .Include(f => f.Fidelidade)
                .Where(u => u.UsuarioAtivo)
                .ToList();
        }

        public ICollection<Usuario>? ListarFuncionariosInativos()
        {
            return _context.Usuarios
                .Include(ue => ue.UnidadesEstabelecimento)
                .Where(u => !u.UsuarioAtivo && u.Perfil != Domain.Enums.TipoPerfil.CLIENTE)
                .ToList();
        }

        public Usuario? BuscarEmailSenha(string email, string senha)
        {
            return _context.Usuarios
                .Where(u => u.Email == email && u.SenhaHash == senha)
                .FirstOrDefault();
        }

        public Usuario CriarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return usuario;
        }

        public bool BuscarPerfilGerente(int estabelecimentoId, int usuarioProprioId = 0)
        {
            return _context.Usuarios.Any(u => u.EstabelecimentoId == estabelecimentoId &&
                u.Perfil == Domain.Enums.TipoPerfil.GERENTE &&
                u.UsuarioAtivo  && u.Id != usuarioProprioId);
        }

        public bool BuscarPerfilAdmin(int estabelecimentoId, int usuarioProprioId = 0)
        {
            return _context.Usuarios.Any(u => u.EstabelecimentoId == estabelecimentoId && 
                u.Perfil == Domain.Enums.TipoPerfil.ADMIN && 
                u.UsuarioAtivo && u.Id != usuarioProprioId);
        }

        public Usuario? AtualizarUsuario(int id, Usuario usuario)
        {
            var usuarioEncontrado = _context.Usuarios.Find(id);

            if (usuarioEncontrado == null)
                return null;

            usuarioEncontrado.Nome = usuario.Nome;
            usuarioEncontrado.Email = usuario.Email;
            usuarioEncontrado.Telefone = usuario.Telefone;
            usuarioEncontrado.Perfil = usuario.Perfil;
            usuarioEncontrado.Rua = usuario.Rua;
            usuarioEncontrado.Numero = usuario.Numero;
            usuarioEncontrado.Complemento = usuario.Complemento;
            usuarioEncontrado.Bairro = usuario.Bairro;
            usuarioEncontrado.Cidade = usuario.Cidade;
            usuarioEncontrado.Estado = usuario.Estado;
            usuarioEncontrado.CEP = usuario.CEP;
            usuarioEncontrado.EstabelecimentoId = usuario.EstabelecimentoId;
            usuarioEncontrado.ConsentimentoLGPD = usuario.ConsentimentoLGPD;
            usuarioEncontrado.ParticipaFidelidade = usuario.ParticipaFidelidade;

            _context.Usuarios.Update(usuarioEncontrado);
            _context.SaveChanges();

            return usuarioEncontrado;
        }

        public Usuario? AtualizarEmailSenha(int id, string email, string senha)
        {
            var usuarioEncontrado = _context.Usuarios.Find(id);

            if (usuarioEncontrado == null)
                return null;

            usuarioEncontrado.Email = email;
            usuarioEncontrado.SenhaHash = senha;

            _context.Usuarios.Update(usuarioEncontrado);
            _context.SaveChanges();

            return usuarioEncontrado;
        }

        public bool EmailJaPossuiCadastro(string email, int usuarioProprioId)
        {
            return _context.Usuarios.Any(u => u.Email == email && u.Id != usuarioProprioId);
        }

        public Usuario? DesativarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if (usuario == null)
                return null;

            usuario.UsuarioAtivo = false;

            _context.Usuarios.Update(usuario);
            _context.SaveChanges();

            return usuario;
        }
    }
}
