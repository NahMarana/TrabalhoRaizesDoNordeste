namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class MovEstoqueDTO
    {
        public int Id { get; set; }
        public required int ProdutoId { get; set; }
        public required int EstabelecimentoId { get; set; }
        public required int QtdEstoqueDisponivel { get; set; }
    }
}
