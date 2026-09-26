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
        public required string SenhaHash { get; set; }
        public required bool ConsentimentoLGPD { get; set; }
        public required bool ParticipaFidelidade { get; set; }
        public required bool UsuarioAtivo { get; set; }
        public required DateTime DataCadastro { get; set; }


        public Usuario(string nome, string email, string cpf, string telefone, TipoPerfil perfil, string rua, string numero, string? complemento, string bairro, string cidade, string estado, string cep, string senhaHash, bool consentimentoLGPD, bool participaFidelidade)
        {
            Nome = nome;
            Email = email;
            CPF = cpf;
            Telefone = telefone;
            Perfil = perfil;
            Rua = rua;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            CEP = cep;
            SenhaHash = senhaHash;
            ConsentimentoLGPD = consentimentoLGPD;
            ParticipaFidelidade = participaFidelidade;
        }
    }
}
