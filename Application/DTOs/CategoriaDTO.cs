using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public required string NomeCategoria { get; set; }
        public bool CategoriaAtiva { get; set; } = true;
        public ICollection<ProdutoDTO>? Produtos { get; set; }
    }
}
