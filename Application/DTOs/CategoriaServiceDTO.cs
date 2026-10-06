using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class CategoriaServiceDTO
    {
        public int Id { get; set; }
        public required string NomeCategoria { get; set; }
    }
}
