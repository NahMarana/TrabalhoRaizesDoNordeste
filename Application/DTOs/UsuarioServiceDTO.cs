using System.Text.Json.Serialization;
using TrabalhoRaizesDoNordeste.Domain.Enums;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class UsuarioServiceDTO
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
        public required bool ConsentimentoLGPD { get; set; }
        public required bool ParticipaFidelidade { get; set; }
        public bool UsuarioAtivo { get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public EstabelecimentoDTO? UnidadesEstabelecimento { get; set; }
        public FidelidadeDTO? Fidelidade { get; set; }
    }
}
