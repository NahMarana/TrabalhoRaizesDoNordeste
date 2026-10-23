using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository
{
    public class LogAuditoriaRepository(AppDbContext _context)
    {
        public LogAuditoria? BuscaAuditoriaPorId(int id)
        {
            return _context.LogsAuditoria
                .Include(l => l.Usuarios)
                .FirstOrDefault(l => l.Id == id);
        }

        public ICollection<LogAuditoria>? ListarAuditorias()
        {
            return _context.LogsAuditoria
                .Include(l => l.Usuarios)
                .OrderByDescending(l => l.DataOrigem)
                .ToList();
        }

        public Usuario? BuscaUsuarioPorId(int usuarioId)
        {
            return _context.Usuarios.Find(usuarioId);
        }

        public LogAuditoria CriarAuditoria(LogAuditoria auditoria)
        {
            _context.LogsAuditoria.Add(auditoria);
            _context.SaveChanges();

            return auditoria;
        }
    }
}
