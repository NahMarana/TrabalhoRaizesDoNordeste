using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class UsuarioService(UsuarioRepository repository, FidelidadeService fidelidadeService, LogAuditoriaService auditoriaService)
    {
        public UsuarioServiceDTO BuscaPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var usuario = repository.BuscaUsuarioPorId(id);

            if (usuario == null)
            {
                return null;
            }


            var usuarioServiceDTO = CriarObjeto(usuario);

            return usuarioServiceDTO;
        }

        public ICollection<UsuarioServiceDTO>?BuscarPorNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("Nome do usuário não informado.");
            }

            var usuarios = repository.BuscarUsuarioPorNome(nome);

            return CriarObjetoCollection(usuarios);
        }

        public ICollection<UsuarioServiceDTO> ListarUsuarios()
        {
            var usuario = repository.ListarUsuarios();
            var usuarioServiceDTO = CriarObjetoCollection(usuario);

            return usuarioServiceDTO;
        }

        public ICollection<UsuarioServiceDTO> ListarFuncionariosInativos()
        {
            var usuarios = repository.ListarFuncionariosInativos();
            return CriarObjetoCollection(usuarios);
        }

        public Usuario CriarUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentException("Usuário inexistente.");
            }

            if (string.IsNullOrEmpty(usuario.Nome))
            {
                throw new ArgumentException("Nome do usuário é obrigatório");
            }

            if (usuario.ParticipaFidelidade && !usuario.ConsentimentoLGPD)
            {
                throw new ArgumentException("Para participar do programa de fidelidade é necessário aceitar os termos da LGPD.");
            }

            if (usuario.ParticipaFidelidade && usuario.Perfil != TipoPerfil.CLIENTE)
            {
                throw new ArgumentException("Somente clientes podem participar do programa de fidelidade.");
            }

            if (usuario.EstabelecimentoId <= 0)
            {
                throw new ArgumentException("Unidade de Estabelecimento é obrigatória.");
            }

            if (!Enum.IsDefined(typeof(TipoPerfil), usuario.Perfil))
            {
                throw new ArgumentException("Tipo de perfil inválido.");
            }

            var unidade = repository.BuscaUnidadePorId(usuario.EstabelecimentoId);

            if (unidade == null)
            {
                throw new ArgumentException("Unidade não encontrada.");
            }

            if (usuario.Perfil == TipoPerfil.GERENTE)
            {
                if (BuscarPerfilGerente(usuario.EstabelecimentoId))
                {
                    throw new ArgumentException(
                        "Não foi possível cadastrar o gerente, pois já existe um gerente ativo nesta unidade.");
                }
            }

            if (usuario.Perfil == TipoPerfil.ADMIN)
            {
                if (BuscarPerfilAdmin(usuario.EstabelecimentoId))
                {
                    throw new ArgumentException(
                        "Não foi possível cadastrar o administrador, pois já existe um administrador ativo nesta unidade.");
                }
            }

            var usuarioCriado = repository.CriarUsuario(usuario);

            if (usuario.ParticipaFidelidade)
            {
                var usuarioDTO = new UsuarioFidelidadeDTO
                { 
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    ParticipaFidelidade = usuario.ParticipaFidelidade,
                    Perfil = usuario.Perfil
                };
                fidelidadeService.CriarFidelidade(usuarioDTO);
            }

            if (usuarioCriado.ConsentimentoLGPD)
            {
                auditoriaService.RegistrarLog(
                    usuarioCriado.Id,
                    "ACEITA_LGPD",
                    "Usuario",
                    usuarioCriado.Id,
                    "",
                    $"Consentimento LGPD aceito em {usuarioCriado.DataCadastro}"
                );
            }

            return usuarioCriado;
        }

        private  bool BuscarPerfilGerente(int estabelecimentoid, int usuarioProprioId = 0)
        {
            return repository.BuscarPerfilGerente(estabelecimentoid, usuarioProprioId);
        }

        private bool BuscarPerfilAdmin(int estabelecimentoid, int usuarioProprioId = 0)
        {
            return repository.BuscarPerfilAdmin(estabelecimentoid, usuarioProprioId);
        }

        public Usuario? AtualizarUsuario(int id, Usuario usuario)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            if (!Enum.IsDefined(typeof(TipoPerfil), usuario.Perfil))
            {
                throw new ArgumentException("Tipo de perfil usuário inválido.");
            }

            if (usuario.ParticipaFidelidade && !usuario.ConsentimentoLGPD)
            {
                throw new ArgumentException("Para participar do programa de fidelidade é necessário aceitar os termos da LGPD.");
            }

            if (usuario.ParticipaFidelidade && usuario.Perfil != TipoPerfil.CLIENTE)
            {
                throw new ArgumentException("Somente clientes podem participar do programa de fidelidade.");
            }

            var usuarioEncontrado = repository.BuscaUsuarioPorId(id);

            if (usuarioEncontrado == null)
            {
                return null;
            }

            if (usuario.Perfil == TipoPerfil.GERENTE)
            {
                if (BuscarPerfilGerente(usuario.EstabelecimentoId, id))
                {
                    throw new ArgumentException(
                        "Não foi possível cadastrar o gerente, pois já existe um gerente ativo nesta unidade.");
                }
            }

            if (usuario.Perfil == TipoPerfil.ADMIN)
            {
                if (BuscarPerfilAdmin(usuario.EstabelecimentoId, id))
                {
                    throw new ArgumentException(
                        "Não foi possível cadastrar o administrador, pois já existe um administrador ativo nesta unidade.");
                }
            }

            var perfilAnterior = usuarioEncontrado.Perfil;
            var unidadeAnterior = usuarioEncontrado.EstabelecimentoId;
            var consentimentoAnterior = usuarioEncontrado.ConsentimentoLGPD;
            var possuiFidelidade = usuarioEncontrado.Fidelidade != null;

            var usuarioAtualizado = repository.AtualizarUsuario(id, usuario);

            if (usuario.ParticipaFidelidade && !possuiFidelidade)
            {
                var usuarioDTO = new UsuarioFidelidadeDTO
                {
                    Id = id,
                    Nome = usuario.Nome,
                    ParticipaFidelidade = usuario.ParticipaFidelidade,
                    Perfil = usuario.Perfil
                };
                fidelidadeService.CriarFidelidade(usuarioDTO);
            }

            auditoriaService.RegistrarLog(
                id,
                "ATUALIZAR_USUARIO",
                "Usuario",
                id,
                $"Perfil: {perfilAnterior}; Unidade: {unidadeAnterior}",
                $"Perfil: {usuario.Perfil}; Unidade: {usuario.EstabelecimentoId}"
            );

            if (!consentimentoAnterior && usuario.ConsentimentoLGPD)
            {
                auditoriaService.RegistrarLog(
                    id,
                    "ACEITA_LGPD",
                    "Usuario",
                    id,
                    "Consentimento: não",
                    $"Consentimento LGPD aceito em {DateTime.Now}"
                );
            }

            return usuarioAtualizado;
        }

        public Usuario? DesativarUsuario(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var usuario = repository.BuscaUsuarioPorId(id);

            if (usuario == null)
            {
                return null;
            }

            if (!usuario.UsuarioAtivo)
            {
                throw new InvalidOperationException("Este usuário está inativo.");
            }

            var usuarioDesativado = repository.DesativarUsuario(id);

            auditoriaService.RegistrarLog(
                id,
                "DESATIVAR_USUARIO",
                "Usuario",
                id,
                "Ativo: true",
                "Ativo: false"
            );

            return usuarioDesativado;
        }


        public UsuarioServiceDTO CriarObjeto(Usuario usuario)
        {
            FidelidadeDTO? fidelidadeDTO = null;

            if (usuario.Perfil == TipoPerfil.CLIENTE && usuario.ParticipaFidelidade)
            {
                if (usuario.Fidelidade != null)
                {
                    fidelidadeDTO = new FidelidadeDTO
                    {
                        Id = usuario.Fidelidade.Id,
                        QtdPontos = usuario.Fidelidade.QtdPontos
                    };
                }
            }

            var usuarioServiceDTO = new UsuarioServiceDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                CPF = usuario.CPF,
                Telefone = usuario.Telefone,
                Perfil = usuario.Perfil,
                Rua = usuario.Rua,
                Numero = usuario.Numero,
                Complemento = usuario.Complemento,
                Bairro = usuario.Bairro,
                Cidade = usuario.Cidade,
                Estado = usuario.Estado,
                CEP = usuario.CEP,
                ConsentimentoLGPD = usuario.ConsentimentoLGPD,
                ParticipaFidelidade = usuario.ParticipaFidelidade,
                DataCadastro = usuario.DataCadastro,
                UsuarioAtivo = usuario.UsuarioAtivo,

                UnidadesEstabelecimento = usuario.UnidadesEstabelecimento == null
                        ? null
                        : new EstabelecimentoDTO
                        {
                            Id = usuario.UnidadesEstabelecimento.Id,
                            NomeEstabelecimento = usuario.UnidadesEstabelecimento.NomeEstabelecimento,
                            CNPJ = usuario.UnidadesEstabelecimento.CNPJ,
                            Estado = usuario.UnidadesEstabelecimento.Estado,
                            Cidade = usuario.UnidadesEstabelecimento.Cidade
                        },

                Fidelidade = fidelidadeDTO
            };
            return usuarioServiceDTO;
        }

        public ICollection<UsuarioServiceDTO> CriarObjetoCollection(ICollection<Usuario> usuarios)
        {
            var auxiliaUsuario = new List<UsuarioServiceDTO>();

            foreach (var item in usuarios)
            {
                FidelidadeDTO? fidelidadeDTO = null;

                if (item.Perfil == TipoPerfil.CLIENTE && item.ParticipaFidelidade)
                {
                    if (item.Fidelidade != null)
                    {
                        fidelidadeDTO = new FidelidadeDTO
                        {
                            Id = item.Fidelidade.Id,
                            QtdPontos = item.Fidelidade.QtdPontos
                        };
                    }
                }

                var usuarioServiceDTO = new UsuarioServiceDTO
                {
                    Id = item.Id,
                    Nome = item.Nome,
                    Email = item.Email,
                    CPF = item.CPF,
                    Telefone = item.Telefone,
                    Perfil = item.Perfil,
                    Rua = item.Rua,
                    Numero = item.Numero,
                    Complemento = item.Complemento,
                    Bairro = item.Bairro,
                    Cidade = item.Cidade,
                    Estado = item.Estado,
                    CEP = item.CEP,
                    ConsentimentoLGPD = item.ConsentimentoLGPD,
                    ParticipaFidelidade = item.ParticipaFidelidade,
                    DataCadastro = item.DataCadastro,
                    UsuarioAtivo = item.UsuarioAtivo,

                    UnidadesEstabelecimento = item.UnidadesEstabelecimento == null
                        ? null
                        : new EstabelecimentoDTO
                        {
                            Id = item.UnidadesEstabelecimento.Id,
                            NomeEstabelecimento = item.UnidadesEstabelecimento.NomeEstabelecimento,
                            CNPJ = item.UnidadesEstabelecimento.CNPJ,
                            Estado = item.UnidadesEstabelecimento.Estado,
                            Cidade = item.UnidadesEstabelecimento.Cidade
                        },

                    Fidelidade = fidelidadeDTO
                };

                auxiliaUsuario.Add(usuarioServiceDTO);
            }

            return auxiliaUsuario;
        }
    }
}
