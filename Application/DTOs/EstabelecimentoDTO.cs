namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class EstabelecimentoDTO
    {
        public int Id { get; set; }
        public required string NomeEstabelecimento { get; set; }
        public required string CNPJ { get; set; }
        public required string Cidade { get; set; }
        public required string Estado { get; set; }
    }
}
