using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.Application.Services;

public class AuthService(UsuarioService usuarioService, TokenService tokenService, LogAuditoriaService auditoriaService)
{
    public string GerarToken(string email, string senha)
    {
        var usuario = usuarioService.BuscaEmailSenha(email, senha);

        if (usuario == null)
        {
            throw new KeyNotFoundException("Informações de senha ou email inválida!.");
        }

        return tokenService.GerarToken(usuario);
    }

    public string AtualizaEmailSenha(int id, string email, string senha)
    {
        var usuario = usuarioService.AtualizaEmailSenha(id, email, senha);

        if (usuario == null)
        {
            throw new KeyNotFoundException("Informações de senha ou email inválida!.");
        }

        return "Email e/ou senha atualizados com sucesso. Faça login novamente.";
    }
}
