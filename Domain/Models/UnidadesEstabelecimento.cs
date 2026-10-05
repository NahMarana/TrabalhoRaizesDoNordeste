using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class UnidadesEstabelecimento
    {
        public int Id { get; set; }
        public required string NomeEstabelecimento { get; set; }
        public required string CNPJ { get; set; }
        public required bool UnidadeAtiva { get; set; }
        public required TipoUnidade TipoUnidade { get; set; }
        public required string Rua { get; set; }
        public required string Numero { get; set; }
        public required string Bairro { get; set; }
        public required string Cidade { get; set; }
        public required string Estado { get; set; }
        public required string CEP { get; set; }
        public required string TelefoneUnidade { get; set; }
        public ICollection<Pedido>? Pedidos { get; set; }
        public ICollection<EstoqueUnidade>? EstoqueUnidades { get; set; }
        public ICollection<PromocoesCampanhas>? PromocoesCampanhas { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; }

    }
}
