using Microsoft.EntityFrameworkCore;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Infrastructure.Repository;

public class CategoriaRepository(AppDbContext _context)
{

    public Categoria? BuscaCagetoriaPorId(int id)
    {
        return _context.Categorias
            .Include(p => p.Produtos)
            .FirstOrDefault(c => c.Id == id);
    }

    public Categoria? BuscarCategoriaPorNome(string nome)
    {
        return _context.Categorias
            .Include(p => p.Produtos)
            .FirstOrDefault(c => c.NomeCategoria.ToLower().Contains(nome.ToLower()));
    }

    public ICollection<Categoria>? ListarCategorias()
    {
        return _context.Categorias
            .Include(c => c.Produtos)
            .ToList();
    }

    public Categoria CriarCategoria(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        _context.SaveChanges();
        return categoria;
    }

    public Categoria? AtualizarCategoria(int id, Categoria categoria)
    {
        var categoriaEncontrada = _context.Categorias.Find(id);

        categoriaEncontrada.NomeCategoria = categoria.NomeCategoria;
        categoriaEncontrada.CategoriaAtiva = categoria.CategoriaAtiva;

        _context.Categorias.Update(categoriaEncontrada);
        _context.SaveChanges();

        return categoriaEncontrada;
    }

    public bool DeletarCategoria(int id)
    {
        var categoria = _context.Categorias.Find(id);

        _context.Categorias.Remove(categoria);
        var response = _context.SaveChanges();

        return response > 0;
    }

}
