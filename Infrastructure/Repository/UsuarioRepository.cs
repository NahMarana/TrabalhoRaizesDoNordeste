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

        public Usuario? BuscarUsuarioPorNome(string nome)
        {
            return _context.Usuarios
                .Include(ue => ue.UnidadesEstabelecimento)
                .Include(f => f.Fidelidade)
                .FirstOrDefault(u => u.Nome.ToLower().Contains(nome.ToLower()));
        }

        public ICollection<Usuario>? ListarUsuarios()
        {
            return _context.Usuarios
                .Include(ue => ue.UnidadesEstabelecimento)
                .Include(f => f.Fidelidade)
                .ToList();
        }

        public Usuario CriarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return usuario;
        }

        public bool BuscarPerfilGerente(int estabelecimentoId)
        {
            return _context.Usuarios.Any(u => u.EstabelecimentoId == estabelecimentoId &&
                u.Perfil == Domain.Enums.TipoPerfil.GERENTE &&
                u.UsuarioAtivo);
        }

        public bool BuscarPerfilAdmin(int estabelecimentoId)
        {
            return _context.Usuarios.Any(u => u.EstabelecimentoId == estabelecimentoId && u.Perfil == Domain.Enums.TipoPerfil.ADMIN && u.UsuarioAtivo);
        }

        public Usuario? AtualizarUsuario(int id, Usuario usuario)
        {
            var usuarioEncontrado = _context.Usuarios.Find(id);


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
            //usuarioEncontrado.UnidadesEstabelecimento.Id = usuario.UnidadesEstabelecimento.Id;

            _context.Usuarios.Update(usuarioEncontrado);
            _context.SaveChanges();

            return usuarioEncontrado;
        }

        public bool DeletarUsuario(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            _context.Usuarios.Remove(usuario);
            var response = _context.SaveChanges();

            return response > 0;
        }
    }
}
