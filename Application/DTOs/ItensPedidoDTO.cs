using System.Text.Json.Serialization;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class ItensPedidoDTO
    {
        public int Id { get; set; }
        public int ProdutoId { get; set; }
        public required string NomeProduto { get; set; }
        public required int QtdItens { get; set; }
        public required decimal PrecoUnitario { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public decimal? ValorPromocional { get; set; }
    }
}
