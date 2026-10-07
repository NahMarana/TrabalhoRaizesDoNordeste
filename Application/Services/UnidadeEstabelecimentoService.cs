using TrabalhoRaizesDoNordeste.Infrastructure.Repository;
using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Application.Services

{
    public class UnidadeEstabelecimentoService(UnidadeEstabelecimentoRepository repository)
    {
        public UnidadeDTO BuscaPorId(int id)
        {
            if (id < 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var unidade = repository.BuscaUnidadePorId(id);

            if (unidade == null)
            {
                return null;
            }

            var unidadeDTO = CriarObjeto(unidade);

            return unidadeDTO;
        }

        public UnidadeDTO BuscarPorNome(string nome)
        {
            if (nome == null)
            {
                throw new ArgumentException("Nome da Unidade de Estabelecimento não informada.");
            }

            var unidade = repository.BuscarUnidadePorNome(nome);

            if (unidade == null)
            {
                return null;
            }

            var unidadeDTO = CriarObjeto(unidade);

            return unidadeDTO;
        }
        public ICollection<UnidadeDTO> ListarUnidades()
        {
            var unidade = repository.ListarUnidades();
            var unidadeDTO = CriarObjetoCollection(unidade);

            return unidadeDTO;
        }

        public UnidadesEstabelecimento CriarUnidade(UnidadesEstabelecimento unidade)
        {
            if (unidade == null)
            {
                throw new ArgumentNullException("Unidade Estabelecimento Inexistente");
            }

            if (string.IsNullOrEmpty(unidade.NomeEstabelecimento))
            {
                throw new ArgumentNullException("Nome do estabelecimento é obrigatório");
            }

            if (string.IsNullOrEmpty(unidade.CNPJ))
            {
                throw new ArgumentException("CNPJ da Unidade é obrigatório.");
            }

            if (!Enum.IsDefined(typeof(TipoUnidade), unidade.TipoUnidade))
            {
                throw new ArgumentException("Tipo da unidade inválida.");
            }

            var unidadeEstabelecimentoCriado = repository.CriarUnidade(unidade);
            return unidadeEstabelecimentoCriado;
        }

        public UnidadesEstabelecimento? AtualizarUnidade(int id, UnidadesEstabelecimento unidade)
        {
            if (!Enum.IsDefined(typeof(TipoUnidade), unidade.TipoUnidade))
            {
                throw new ArgumentException("Tipo da unidade inválida.");
            }

            if (id < 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var unidadeAtualizada = repository.AtualizarUnidade(id, unidade);
            return unidadeAtualizada;
        }

        public bool DeletarUnidade(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var unidadeDeletada = repository.DeletarUnidade(id);
            return unidadeDeletada;
        }


        public UnidadeDTO CriarObjeto(UnidadesEstabelecimento unidade)
        {
            var unidadeDTO = new UnidadeDTO
            {
                Id = unidade.Id,
                NomeEstabelecimento = unidade.NomeEstabelecimento,
                CNPJ = unidade.CNPJ,
                UnidadeAtiva = unidade.UnidadeAtiva,
                TipoUnidade = unidade.TipoUnidade,
                Rua = unidade.Rua,
                Numero = unidade.Numero,
                Bairro = unidade.Bairro,
                Cidade = unidade.Cidade,
                Estado = unidade.Estado,
                CEP = unidade.CEP,
                TelefoneUnidade = unidade.TelefoneUnidade,
                EstoqueUnidades = unidade.EstoqueUnidades.Select(eu => new EstoqueUnidadeDTO
                {
                    Id = eu.Id,
                    QtdEstoqueDisponivel = eu.QtdEstoqueDisponivel,
                    ProdutoId = eu.ProdutoId,
                    NomeProduto = eu.Produtos!.NomeProduto,
                    DescricaoProduto = eu.Produtos.DescricaoProduto,
                    PrecoUnitario = eu.Produtos.PrecoUnitario,
                    ProdutoSazonal = eu.Produtos.ProdutoSazonal

                }).ToList(),
                Usuarios = unidade.Usuarios.Select(u => new UsuarioDTO 
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Perfil = u.Perfil,
                    UsuarioAtivo = u.UsuarioAtivo,
                }
                ).ToList()
            };
            return unidadeDTO;
        }

        public ICollection<UnidadeDTO> CriarObjetoCollection(ICollection<UnidadesEstabelecimento> unidade)
        {
            var auxiliaUnidade = new List<UnidadeDTO>();
            foreach (var item in unidade)
            {
                var unidadeDTO = new UnidadeDTO
                {
                    Id = item.Id,
                    NomeEstabelecimento = item.NomeEstabelecimento,
                    CNPJ = item.CNPJ,
                    UnidadeAtiva = item.UnidadeAtiva,
                    TipoUnidade = item.TipoUnidade,
                    Rua = item.Rua,
                    Numero = item.Numero,
                    Bairro = item.Bairro,
                    Cidade = item.Cidade,
                    Estado = item.Estado,
                    CEP = item.CEP,
                    TelefoneUnidade = item.TelefoneUnidade,
                    EstoqueUnidades = item.EstoqueUnidades.Select(eu => new EstoqueUnidadeDTO
                    {
                        Id = eu.Id,
                        QtdEstoqueDisponivel = eu.QtdEstoqueDisponivel,
                        ProdutoId = eu.ProdutoId,
                        NomeProduto = eu.Produtos!.NomeProduto,
                        DescricaoProduto = eu.Produtos.DescricaoProduto,
                        PrecoUnitario = eu.Produtos.PrecoUnitario,
                        ProdutoSazonal = eu.Produtos.ProdutoSazonal

                    }).ToList(),
                    Usuarios = item.Usuarios.Select(u => new UsuarioDTO
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Perfil = u.Perfil,
                        UsuarioAtivo = u.UsuarioAtivo,
                    }
                    ).ToList()
                };
                auxiliaUnidade.Add(unidadeDTO);
            }

            return auxiliaUnidade;
        }
    }
}
