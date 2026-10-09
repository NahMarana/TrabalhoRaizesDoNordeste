using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;


namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class FidelidadeService(FidelidadeRepository repository)
    {
        public FidDTO? BuscaPorId(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var fidelidade = repository.BuscaFidelidadePorId(id);

            if (fidelidade == null)
            {
                return null;
            }

            var fidDTO = CriarObjeto(fidelidade);

            return fidDTO;
        }

        public ICollection<FidDTO> ListarFidelidades()
        {
            var fidelidade = repository.ListarFidelidades();
            var fidDTO = CriarObjetoCollection(fidelidade);

            return fidDTO;
        }

        public Fidelidade CriarFidelidade(UsuarioFidelidadeDTO usuario)
        {
            if (usuario.Perfil != TipoPerfil.CLIENTE && usuario.Perfil != TipoPerfil.ATENDENTE && usuario.Perfil != TipoPerfil.GERENTE)
            {
                throw new ArgumentException("Este Perfil não pode ser participar da fidelidade.");
            }

            if (!usuario.ParticipaFidelidade)
            {
                throw new ArgumentException("O usuário não participa do programa de fidelidade.");
            }

            if (repository.UsuarioPossuiFidelidade(usuario.Id))
            {
                throw new ArgumentException("Este usuário já possui uma fidelidade cadastrada.");
            }
                
            var fidelidade = new Fidelidade
            {
                UsuarioId = usuario.Id,
                QtdPontos = 0
            };

            return repository.CriarFidelidade(fidelidade);
        }

        public FidDTO CriarObjeto(Fidelidade fidelidade)
        {
            var fidDTO = new FidDTO
            {
                Id = fidelidade.Id,
                QtdPontos = fidelidade.QtdPontos,
                PontosFidelidade = fidelidade.PontosFidelidade.Select(pf => new PontosFidDTO
                {
                    Id = pf.Id,
                    TipoMovimentacaoPontos = pf.TipoMovimentacaoPontos,
                    Pontos = pf.Pontos,

                }).ToList(),
                Usuarios = fidelidade.Usuarios == null
                ? null : new UsuarioFidelidadeDTO
                {
                    Id = fidelidade.Usuarios.Id,
                    Nome = fidelidade.Usuarios.Nome,
                    Perfil = fidelidade.Usuarios.Perfil,
                    ParticipaFidelidade = fidelidade.Usuarios.ParticipaFidelidade,
                }
            };
            return fidDTO;
        }

        public ICollection<FidDTO> CriarObjetoCollection(ICollection<Fidelidade> fidelidade)
        {
            var auxiliaFidelidade = new List<FidDTO>();

            foreach (var item in fidelidade)
            {
                var fidDTO = new FidDTO
                {
                    Id = item.Id,
                    QtdPontos = item.QtdPontos,
                    PontosFidelidade = item.PontosFidelidade.Select(pf => new PontosFidDTO
                    {
                        Id = pf.Id,
                        TipoMovimentacaoPontos = pf.TipoMovimentacaoPontos,
                        Pontos = pf.Pontos,

                    }).ToList(),
                    Usuarios = item.Usuarios == null
                        ? null : new UsuarioFidelidadeDTO
                    {
                        Id = item.Usuarios.Id,
                        Nome = item.Usuarios.Nome,
                        Perfil = item.Usuarios.Perfil,
                        ParticipaFidelidade = item.Usuarios.ParticipaFidelidade,
                    }
                };

                auxiliaFidelidade.Add(fidDTO);
            }

            return auxiliaFidelidade;
        }
    }
}
