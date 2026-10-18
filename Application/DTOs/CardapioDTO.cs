using System.Text.Json.Serialization;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class CardapioDTO
    {
        public string NomeCategoria { get; set; } = "";
        public string NomeProduto { get; set; } = "";
        public string? DescricaoProduto { get; set; }
        public decimal PrecoUnitario { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? ProdutoSazonal { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? ValorPromocional { get; set; }
    }
}
