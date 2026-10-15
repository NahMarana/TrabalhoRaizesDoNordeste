using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class LogAuditoriaService(LogAuditoriaRepository repository)
    {
        public LogAuditoriaDTO BuscaPorId(int id, int usuarioId)
        {
            ValidarAcesso(usuarioId);

            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var log = repository.BuscaAuditoriaPorId(id);

            if (log == null)
            {
                return null;
            }


            var logAuditoriaDTO = CriarObjeto(log);

            return logAuditoriaDTO;
        }

        public ICollection<LogAuditoriaDTO> ListarAuditorias(int usuarioId)
        {
            ValidarAcesso(usuarioId);

            var log = repository.ListarAuditorias();
            var logAuditoriaDTO = CriarObjetoCollection(log);

            return logAuditoriaDTO;
        }

        public void RegistrarLog(int usuarioId, string acao, string entidadeAfetada, int entidadeAfetadaId, string dadosAnteriores, string dadosNovos)
        {
            var log = new LogAuditoria
            {
                UsuarioId = usuarioId,
                Acao = acao,
                EntidadeAfetada = entidadeAfetada,
                EntidadeAfetadaId = entidadeAfetadaId,
                DadosAnteriores = dadosAnteriores,
                DadosNovos = dadosNovos,
                DataOrigem = DateTime.Now
            };

            repository.CriarAuditoria(log);
        }

        private void ValidarAcesso(int usuarioId)
        {
            var usuario = repository.BuscaUsuarioPorId(usuarioId);

            if (!usuario.UsuarioAtivo == true)
            {
                throw new KeyNotFoundException("Usuário tem que estar ativo.");
            }

            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            if (usuario.Perfil != TipoPerfil.GERENTE && usuario.Perfil != TipoPerfil.ADMIN)
            {
                throw new UnauthorizedAccessException("Somente Gerente e Administrador podem acessar as auditorias.");
            }
        }

        public LogAuditoriaDTO CriarObjeto(LogAuditoria auditoria)
        {
            var logAuditoriaDTO = new LogAuditoriaDTO
            {
                Id = auditoria.Id,
                Acao = auditoria.Acao,
                EntidadeAfetada = auditoria.EntidadeAfetada,
                EntidadeAfetadaId = auditoria.EntidadeAfetadaId,
                DadosNovos = auditoria.DadosNovos,
                DadosAnteriores = auditoria.DadosAnteriores,
                DataOrigem = auditoria.DataOrigem,
                Usuarios = auditoria.Usuarios == null
                ? null : new UsuarioDTO
                {
                    Id = auditoria.Usuarios.Id,
                    Nome = auditoria.Usuarios.Nome,
                    Perfil = auditoria.Usuarios.Perfil,
                    UsuarioAtivo = auditoria.Usuarios.UsuarioAtivo,
                }
            };
            return logAuditoriaDTO;
        }

        public ICollection<LogAuditoriaDTO> CriarObjetoCollection(ICollection<LogAuditoria> auditoria)
        {
            var auxiliaLogAuditoria = new List<LogAuditoriaDTO>();

            foreach (var item in auditoria)
            {
                var logAuditoriaDTO = new LogAuditoriaDTO
                {
                    Id = item.Id,
                    Acao = item.Acao,
                    EntidadeAfetada = item.EntidadeAfetada,
                    EntidadeAfetadaId = item.EntidadeAfetadaId,
                    DadosNovos = item.DadosNovos,
                    DadosAnteriores = item.DadosAnteriores,
                    DataOrigem = item.DataOrigem,
                    Usuarios = item.Usuarios == null
                    ? null : new UsuarioDTO
                    {
                        Id = item.Usuarios.Id,
                        Nome = item.Usuarios.Nome,
                        Perfil = item.Usuarios.Perfil,
                        UsuarioAtivo = item.Usuarios.UsuarioAtivo,
                    }
                };
                auxiliaLogAuditoria.Add(logAuditoriaDTO);
            }
            return auxiliaLogAuditoria;
        }
    }
}