using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class ProdutoServiceDTO
    {
        public int Id { get; set; }
        public required string NomeProduto { get; set; }
        public string? DescricaoProduto { get; set; }
        public required decimal PrecoUnitario { get; set; }
        public required bool ProdutoSazonal { get; set; }
        public DateOnly? DataInicioSazonal { get; set; }
        public DateOnly? DataFimSazonal { get; set; }
        public bool ProdutoAtivo { get; set; }

        public CategoriaServiceDTO? Categoria { get; set; }
    }
}
