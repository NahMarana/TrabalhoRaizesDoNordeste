namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Categorias
    {
        public int Id { get; set; }
        public string? NomeCategoria { get; set; }
        public bool CategoriaAtiva { get; set; } = true;
        public ICollection<Produtos>? Produtos { get; set; }
    }
}
