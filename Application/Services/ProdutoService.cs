using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services
{
    public class ProdutoService(ProdutoRepository repository)
    {

        public ProdutoServiceDTO BuscaPorId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var produto = repository.BuscaProdutoPorId(id);

            if (produto == null)
            {
                return null;
            }


            var prodServiceDTO = CriarObjeto(produto);

            return prodServiceDTO;
        }

        public ProdutoServiceDTO BuscarPorNome(string nome)
        {
            if (nome == null)
            {
                throw new ArgumentException("Nome do Produto não informado.");
            }

            var produto = repository.BuscarProdutoPorNome(nome);

            if(produto == null)
            {
                return null;
            }

            var prodServiceDTO = CriarObjeto(produto);

            return prodServiceDTO;
        }

        public ICollection<ProdutoServiceDTO> ListarProdutos()
        {
            var produto = repository.ListarProdutos();
            var prodServiceDTO = CriarObjetoCollection(produto);

            return prodServiceDTO;
        }

        public Produto CriarProduto(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException("Produto Inexistente");
            }

            if (string.IsNullOrEmpty(produto.NomeProduto))
            {
                throw new ArgumentNullException("Nome do produto é obrigatório");
            }

            if (produto.CategoriaId <= 0)
            {
                throw new ArgumentException("Categoria do produto é obrigatória.");
            }

            if (produto.ProdutoSazonal == true)
            {
                if(produto.DataInicioSazonal == null)
                    throw new ArgumentException("Data de início sazonal requerido.");

                if (produto.DataFimSazonal == null)
                    throw new ArgumentException("Data de fim do produto sazonal.");

                if (produto.DataFimSazonal < produto.DataInicioSazonal)
                    throw new ArgumentException("A data final do produto sazonal não pode ser menor que a data início.");
            }

            var produtoCriado = repository.CriarProduto(produto);
            return produtoCriado;
        }

        public Produto? AtualizarProduto(int id, Produto produto)
        {
            if (id < 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var produtoAtualizado = repository.AtualizarProduto(id, produto);
            return produtoAtualizado;
        }

        public bool DeletarProduto(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID fornecido é inválido.");
            }

            var produtoDeletado = repository.DeletarProduto(id);
            return produtoDeletado;
        }


        public ProdutoServiceDTO CriarObjeto(Produto produto)
        {
            var prodServiceDTO = new ProdutoServiceDTO
            {
                Id = produto.Id,
                NomeProduto = produto.NomeProduto,
                DescricaoProduto = produto.DescricaoProduto,
                PrecoUnitario = produto.PrecoUnitario,
                ProdutoSazonal = produto.ProdutoSazonal,
                ProdutoAtivo = produto.ProdutoAtivo,
                DataInicioSazonal = produto.DataInicioSazonal,
                DataFimSazonal = produto.DataFimSazonal,

                Categoria = produto.Categoria == null
                ? null : new CategoriaServiceDTO
                {
                    Id = produto.Categoria.Id,
                    NomeCategoria = produto.Categoria.NomeCategoria,
                }
            };
            return prodServiceDTO;
        }

        public ICollection<ProdutoServiceDTO> CriarObjetoCollection(ICollection<Produto> produtos)
        {
            var auxiliaProduto = new List<ProdutoServiceDTO>();

            foreach (var item in produtos)
            {
                var prodServiceDTO = new ProdutoServiceDTO
                {
                    Id = item.Id,
                    NomeProduto = item.NomeProduto,
                    DescricaoProduto = item.DescricaoProduto,
                    PrecoUnitario = item.PrecoUnitario,
                    ProdutoSazonal = item.ProdutoSazonal,
                    ProdutoAtivo = item.ProdutoAtivo,
                    DataInicioSazonal = item.DataInicioSazonal,
                    DataFimSazonal = item.DataFimSazonal,

                    Categoria = item.Categoria == null
                        ? null : new CategoriaServiceDTO
                        {
                            Id = item.Categoria.Id,
                            NomeCategoria = item.Categoria.NomeCategoria
                        }
                };

                auxiliaProduto.Add(prodServiceDTO);
            }

            return auxiliaProduto;
        }
    }
}
