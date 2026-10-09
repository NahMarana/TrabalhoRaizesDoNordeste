using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class FidelidadeRepository(AppDbContext _context)
    {
        public Fidelidade? BuscaFidelidadePorId(int id)
        {
            return _context.Fidelidades
                .Include(f => f.Usuarios)
                .Include(f => f.PontosFidelidade)
                .FirstOrDefault(f => f.Id == id);
        }

        public ICollection<Fidelidade>? ListarFidelidades()
        {
            return _context.Fidelidades
                .Include(f => f.Usuarios)
                .Include(f => f.PontosFidelidade)
                .ToList();
        }

        public Usuario? BuscaUsuarioPorId(int usuarioId)
        {
            return _context.Usuarios.Find(usuarioId);
        }

        public bool UsuarioPossuiFidelidade(int usuarioId)
        {
            return _context.Fidelidades
                .Any(f => f.UsuarioId == usuarioId);
        }

        public Fidelidade? CriarFidelidade(Fidelidade fidelidade)
        {
            _context.Fidelidades.Add(fidelidade);
            _context.SaveChanges();

            return fidelidade;
        }

        public decimal? ConsultarSaldoPontos(int id)
        {
            return _context.Fidelidades
                .Where(f => f.Id == id)
                .Select(f => (decimal?)f.QtdPontos)
                .FirstOrDefault();
        }
    }
}
