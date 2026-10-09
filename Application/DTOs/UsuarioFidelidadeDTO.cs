using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class UsuarioFidelidadeDTO
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required TipoPerfil Perfil { get; set; }
        public required bool ParticipaFidelidade { get; set; }

    }
}
