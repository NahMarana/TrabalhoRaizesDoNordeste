using TrabalhoRaizesDoNordeste.Domain.Enums;

namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required TipoPerfil Perfil { get; set; }
        public bool UsuarioAtivo { get; set; } = true;
    }
}
