namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public required string NomeCategoria { get; set; }
        public bool CategoriaAtiva { get; set; } = true;
        public ICollection<Produto>? Produtos { get; set; }
    }
}
