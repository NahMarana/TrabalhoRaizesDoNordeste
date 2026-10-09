namespace TrabalhoRaizesDoNordeste.Application.DTOs
{
    public class FidDTO
    {
        public int Id { get; set; }
        public required decimal QtdPontos { get; set; }

        public UsuarioFidelidadeDTO? Usuarios { get; set; }
        public ICollection<PontosFidDTO>? PontosFidelidade { get; set; }
    }
}
