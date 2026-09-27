using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PontosFidelidadeController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public PontosFidelidadeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}