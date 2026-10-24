using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrabalhoRaizesDoNordeste.Application.Services;
using TrabalhoRaizesDoNordeste.Context;

namespace TrabalhoRaizesDoNordeste.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly AuthService _service;

    public AuthController(AppDbContext appDbContext, AuthService authService)
    {
        _service = authService;

    }

    [Route("login")]
    [HttpPost]
    public IActionResult Login(string email, string senha)
    {
        try
        {
            var gerarToken = _service.GerarToken(email, senha);

            return Ok(gerarToken);
        }
        catch (Exception ex)
        {
           return Unauthorized(ex.Message);
        }
    }

    [Authorize]
    [HttpPatch("atualizar-acesso")]
    public IActionResult AtualizarEmailSenha(string email, string senha)
    {
        try
        {
            var idDoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(idDoToken, out var id))
                return Unauthorized("Sua sessão expirou. Faça login novamente.");

            return Ok(_service.AtualizaEmailSenha(id, email, senha));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}