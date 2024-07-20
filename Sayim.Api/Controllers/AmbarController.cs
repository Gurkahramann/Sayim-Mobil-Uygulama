using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayim.Api.Data;
using Sayim.Api.Models;

namespace Sayim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmbarController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public AmbarController(AppDbContext appDbContext)=>_appDbContext = appDbContext;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ambar>>> GetAmbar()
        {
            return await _appDbContext.Ambar
                .Where(a => a.KullanimdaMi)
                .OrderBy(a => a.AmbarAdi)
                .ToListAsync();
        }
    }
}
