using Microsoft.AspNetCore.Mvc;
using TrabalhoRaizesDoNordeste.Context;
using TrabalhoRaizesDoNordeste.Domain.Models;

namespace TrabalhoRaizesDoNordeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocoesCampanhasController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public PromocoesCampanhasController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}