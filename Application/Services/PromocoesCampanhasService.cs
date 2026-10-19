using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class PromocoesCampanhasService(PromocoesCampanhasRepository repository)
    {
        public PromoCampanhaDTO BuscaPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var promo = repository.BuscaPromocoesCampanhaPorId(id);

            if(promo == null)
            {
                return null;
            }

            var promoCampanhaDTO =  CriarObjeto(promo);
            return promoCampanhaDTO;
        }

        public ICollection<PromoCampanhaDTO> ListarPromocoesCampanhasAtivas()
        {
            var promo = repository.ListarPromocoesCampanhasAtivas();
            var promoCampanhaDTO = CriarObjetoCollection(promo);

            return promoCampanhaDTO;
        }

        public ICollection<PromoCampanhaDTO> ListarPromocoesCampanhasInativas()
        {
            var promo = repository.ListarPromocoesCampanhasInativas();
            var promoCampanhaDTO = CriarObjetoCollection(promo);

            return promoCampanhaDTO;
        }

        public PromoCampanhaDTO CriarPromoCampanha(PromocoesCampanhas promo)
        {
            if (promo == null)
            {
                throw new ArgumentException("Promoção ou Campanha inexistente.");
            }

            if (promo.ProdutoId <= 0)
            {
                throw new ArgumentException("Produto é obrigatório.");
            }

            if (promo.EstabelecimentoId <= 0)
            {
                throw new ArgumentException("Unidade de Estabelecimento é obrigatória.");
            }

            if (promo.ValorPromocional <= 0)
            {
                throw new ArgumentException("O valor promocional não pode ser zerada.");
            }

            if (promo.DataFim < promo.DataInicio)
            {
                throw new ArgumentException("A data final da promoção não pode ser menor que a data início.");
            }

            var produto = repository.BuscaProdutoPorId(promo.ProdutoId);

            if (produto == null)
            {
                throw new KeyNotFoundException("Produto não encontrado.");
            }

            if (!produto.ProdutoAtivo)
            {
                throw new InvalidOperationException("Produto inativado! Promoção não pode ser aplicado.");
            }

            var unidade = repository.BuscaEstabelecimentoPorId(promo.EstabelecimentoId);

            if (unidade == null)
            {
                throw new KeyNotFoundException("Unidade não encontrada.");
            }

            if (!unidade.UnidadeAtiva)
            {
                throw new InvalidOperationException("Unidade inativada! Promoção não pode ser aplicada.");
            }

            if (promo.ValorPromocional >= produto.PrecoUnitario)
            {
                throw new InvalidOperationException("O valor promocional deve ser menor que o preço do produto.");
            }

            var novaPromo = new PromocoesCampanhas
            {
                EstabelecimentoId = unidade.Id,
                ProdutoId = produto.Id,
                Descricao = promo.Descricao,
                ValorPromocional = promo.ValorPromocional,
                StatusPromoCampanha = StatusPromoCampanha.ATIVO,
                DataInicio = promo.DataInicio,
                DataFim = promo.DataFim
            };

            var promoCriada = repository.CriarPromoCampanha(novaPromo);

            return CriarObjeto(repository.BuscaPromocoesCampanhaPorId(promoCriada.Id)!);
        }

        public PromoCampanhaDTO? AtualizarPromoCampanha(int id, PromocoesCampanhas promo)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            if (!Enum.IsDefined(typeof(StatusPromoCampanha), promo.StatusPromoCampanha))
            {
                throw new ArgumentException("Tipo de Status da Promoção inválido.");
            }

            if (promo.ValorPromocional <= 0)
            {
                throw new ArgumentException("O valor promocional não pode ser negativa.");
            }

            var promoAtualizada = repository.AtualizarPromoCampanha(id, promo);

            if (promoAtualizada == null)
            {
                return null;
            }

            return CriarObjeto(repository.BuscaPromocoesCampanhaPorId(id)!);
        }

        public PromoCampanhaDTO? DesativarPromo(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var promo = repository.BuscaPromocoesCampanhaPorId(id);

            if (promo == null)
            {
                return null;
            }

            if (promo.StatusPromoCampanha != StatusPromoCampanha.ATIVO)
            {
                throw new InvalidOperationException("Esta promoção já está inativa.");
            }

            repository.DesativarPromo(id);

            return CriarObjeto(promo);
        }

        public PromoCampanhaDTO CriarObjeto(PromocoesCampanhas promo)
        {
            var promoCampanhaDTO = new PromoCampanhaDTO
            {
                Id = promo.Id,
                Descricao = promo.Descricao,
                ValorPromocional = promo.ValorPromocional,
                StatusPromoCampanha = promo.StatusPromoCampanha,
                DataInicio = promo.DataInicio,
                DataFim = promo.DataFim,
                Produtos = promo.Produtos == null
                ? null : new ProdutoDTO 
                {
                    Id = promo.Produtos.Id,
                    NomeProduto = promo.Produtos.NomeProduto,
                },
                UnidadesEstabelecimento = promo.UnidadesEstabelecimento == null
                ? null : new EstabelecimentoDTO
                {
                    Id = promo.UnidadesEstabelecimento.Id,
                    NomeEstabelecimento = promo.UnidadesEstabelecimento.NomeEstabelecimento,
                    CNPJ = promo.UnidadesEstabelecimento.CNPJ,
                    Cidade = promo.UnidadesEstabelecimento.Cidade,
                    Estado = promo.UnidadesEstabelecimento.Estado,
                },
            };
            return promoCampanhaDTO;
        }

        public ICollection<PromoCampanhaDTO> CriarObjetoCollection(ICollection<PromocoesCampanhas> promo)
        {
            var auxiliaPromo = new List<PromoCampanhaDTO>();

            foreach (var item in promo)
            {
                var promoCampanhaDTO = new PromoCampanhaDTO
                {
                    Id = item.Id,
                    Descricao = item.Descricao,
                    ValorPromocional = item.ValorPromocional,
                    StatusPromoCampanha = item.StatusPromoCampanha,
                    DataInicio = item.DataInicio,
                    DataFim = item.DataFim,
                    Produtos = item.Produtos == null
                    ? null : new ProdutoDTO
                    {
                        Id = item.Produtos.Id,
                        NomeProduto = item.Produtos.NomeProduto,
                    },
                    UnidadesEstabelecimento = item.UnidadesEstabelecimento == null
                    ? null : new EstabelecimentoDTO
                    {
                        Id = item.UnidadesEstabelecimento.Id,
                        NomeEstabelecimento = item.UnidadesEstabelecimento.NomeEstabelecimento,
                        CNPJ = item.UnidadesEstabelecimento.CNPJ,
                        Cidade = item.UnidadesEstabelecimento.Cidade,
                        Estado = item.UnidadesEstabelecimento.Estado,
                    },
                };

                auxiliaPromo.Add(promoCampanhaDTO);
            }

            return auxiliaPromo;
        }
    }
}
