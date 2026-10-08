using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class UsuarioService(UsuarioRepository repository)
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

        public UsuarioServiceDTO BuscarPorNome(string nome)
        {
            if (nome == null)
            {
                throw new ArgumentException("Nome do usuário não informado.");
            }

            var usuario = repository.BuscarUsuarioPorNome(nome);

            if (usuario == null)
            {
                return null;
            }

            var usuarioServiceDTO = CriarObjeto(usuario);

            return usuarioServiceDTO;
        }

        public ICollection<UsuarioServiceDTO> ListarUsuarios()
        {
            var usuario = repository.ListarUsuarios();
            var usuarioServiceDTO = CriarObjetoCollection(usuario);

            return usuarioServiceDTO;
        }

        public Usuario CriarUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException("Usuário inexistente.");
            }

            if (string.IsNullOrEmpty(usuario.Nome))
            {
                throw new ArgumentNullException("Nome do usuário é obrigatório");
            }

            if (usuario.EstabelecimentoId <= 0)
            {
                throw new ArgumentException("Unidade de Estabelecimento é obrigatória.");
            }

            if (!Enum.IsDefined(typeof(TipoPerfil), usuario.Perfil))
            {
                throw new ArgumentException("Tipo de perfil inválido.");
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
            return usuarioCriado;
        }

        private  bool BuscarPerfilGerente(int estabelecimentoid)
        {
            return repository.BuscarPerfilGerente(estabelecimentoid);
        }

        private bool BuscarPerfilAdmin(int estabelecimentoid)
        {
            return repository.BuscarPerfilAdmin(estabelecimentoid);
        }

        public Usuario? AtualizarUsuario(int id, Usuario usuario)
        {
            if (!Enum.IsDefined(typeof(TipoPerfil), usuario.Perfil))
            {
                throw new ArgumentException("Tipo de perfil usuário inválido.");
            }

            if (id < 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
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

            var usuarioAtualizado = repository.AtualizarUsuario(id, usuario);
            return usuarioAtualizado;
        }

        public bool DeletarUsuario(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var UsuarioDeletado = repository.DeletarUsuario(id);
            return UsuarioDeletado;
        }


        public UsuarioServiceDTO CriarObjeto(Usuario usuario)
        {
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
                ? null : new EstabelecimentoDTO
                {
                    Id = usuario.UnidadesEstabelecimento.Id,
                    NomeEstabelecimento = usuario.UnidadesEstabelecimento.NomeEstabelecimento,
                    CNPJ = usuario.UnidadesEstabelecimento.CNPJ,
                    Estado = usuario.UnidadesEstabelecimento.Estado,
                    Cidade = usuario.UnidadesEstabelecimento.Cidade,
                },
                Fidelidade = usuario.Perfil == TipoPerfil.CLIENTE && usuario.ParticipaFidelidade == true &&
                usuario.Fidelidade == null
                ? new FidelidadeDTO
                {
                    Id = usuario.Fidelidade.Id,
                    QtdPontos = usuario.Fidelidade.QtdPontos,
                } : null
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
