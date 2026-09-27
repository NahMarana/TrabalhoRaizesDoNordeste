using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogAuditoriaController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public LogAuditoriaController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}