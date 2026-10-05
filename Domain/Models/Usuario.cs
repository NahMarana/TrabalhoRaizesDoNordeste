using System.Text.Json.Serialization;
using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string CPF { get; set; }
        public required string Telefone { get; set; }
        public required TipoPerfil Perfil { get; set; }
        public required string Rua { get; set; }
        public required string Numero { get; set; }
        public string? Complemento { get; set; }
        public required string Bairro { get; set; }
        public required string Cidade { get; set; }
        public required string Estado { get; set; }   
        public required string CEP { get; set; }
        [JsonPropertyName("senha")]
        public required string SenhaHash { get; set; }
        public required bool ConsentimentoLGPD { get; set; }
        public required bool ParticipaFidelidade { get; set; }
        public bool UsuarioAtivo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public int EstabelecimentoId { get; set; }
        public UnidadesEstabelecimento? UnidadesEstabelecimento { get; set; }
        public ICollection<LogAuditoria>? LogAuditorias { get; set; }
        public Fidelidade? Fidelidade { get; set; }
        public ICollection<Pedidos>? Pedidos { get; set; }
        public ICollection<EstoqueMovimentacao>? EstoquesMovimentacao { get; set; }
    }
}
