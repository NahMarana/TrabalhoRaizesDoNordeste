using TrabalhoRaizesDoNordeste.Application.DTOs;
using TrabalhoRaizesDoNordeste.Domain.Models;
using TrabalhoRaizesDoNordeste.Infrastructure.Repository;

namespace TrabalhoRaizesDoNordeste.Application.Services;

public class CategoriaService(CategoriaRepository repository)
{
  public CategoriaDTO BuscaPorId(int id)
    {
        if (id < 0)
        {
            throw new ArgumentException("ID fornecido é inválido.");
        }

        var categoria = repository.BuscaCagetoriaPorId(id);
        var categoriaDTO = CriarObjeto(categoria);

        return categoriaDTO;
    }

    public CategoriaDTO BuscarPorNome(string nome)
    {
        if (nome == null)
        {
            throw new ArgumentException("Nome da categoria não informada.");
        }

        var categoria = repository.BuscarCategoriaPorNome(nome);
        var categoriaDTO = CriarObjeto(categoria);

        return categoriaDTO;
    }
    public CategoriaDTO ListarCategorias()
    {
        var categoria = repository.ListarCategorias();
        var categoriaDTO = CriarObjeto(categoria);

        return categoriaDTO;
    }

    public Categoria CriarCategoria(Categoria categoria)
    {
        if (categoria == null)
        {
            throw new ArgumentNullException("Categoria vazia");
        }

        if (string.IsNullOrEmpty(categoria.NomeCategoria))
        {
            throw new ArgumentNullException("Nome da categoria requerida");
        }

        var categoriaCriada = repository.CriarCategoria(categoria);
        return categoriaCriada;
    }

    public Categoria? AtualizarCategoria(int id, Categoria categoria)
    {
        if (id < 0)
        {
            throw new ArgumentException("ID fornecido é inválido.");
        }

        var categoriaAtualizada = repository.AtualizarCategoria(id, categoria);
        return categoriaAtualizada;
    }

    public bool DeletarCategoria(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("ID fornecido é inválido.");
        }

        var categoriaDeletada = repository.DeletarCategoria(id);
        return categoriaDeletada;
    }

    public CategoriaDTO CriarObjeto(Categoria categoria)
    {
        var categoriaDTO = new CategoriaDTO
        {
            Id = categoria.Id,
            NomeCategoria = categoria.NomeCategoria,
            CategoriaAtiva = categoria.CategoriaAtiva,
            Produtos = categoria.Produtos.Select(p => new ProdutoDTO
            {
                Id = p.Id,
                NomeProduto = p.NomeProduto
            }).ToList()
        }; 
        return categoriaDTO;
    }
}
