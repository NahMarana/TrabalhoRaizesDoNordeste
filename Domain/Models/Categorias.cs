namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Categorias
    {
        public required int Id { get; set; }
        public required string NomeCategoria { get; set; }
        public required bool CategoriaAtiva { get; set; }
    }
}
