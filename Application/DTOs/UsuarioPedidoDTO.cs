using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class UsuarioPedidoDTO
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required TipoPerfil Perfil { get; set; }
        public required bool ParticipaFidelidade { get; set; }
        public int FidelizacaoID { get; set; }
        public required decimal QtdPontos { get; set; } = 0;
        public int EstabelecimentoId { get; set; }
        public required string NomeEstabelecimento { get; set; }
        public required string CNPJ { get; set; }
        public required string Cidade { get; set; }
        public required string Estado { get; set; }
    }
}
