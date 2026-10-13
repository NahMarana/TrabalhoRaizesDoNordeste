namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class EstabelecimentoEstoqueDTO
    {
        public int Id { get; set; }
        public required string NomeEstabelecimento { get; set; }
        public required string CNPJ { get; set; }
        public required bool UnidadeAtiva { get; set; }
    }
}
