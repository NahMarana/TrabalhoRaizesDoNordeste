using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstoqueUnidadeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public EstoqueUnidadeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}