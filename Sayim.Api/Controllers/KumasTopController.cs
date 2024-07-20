using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayim.Api.Data;

namespace Sayim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KumasTopController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public KumasTopController(AppDbContext appDbContext) => _appDbContext = appDbContext;
        [HttpGet("TopNoKontrol")]
        public async Task<ActionResult<bool>> TopNoKontrol(string topNo)
        {
            var exists = await _appDbContext.KumasTop
                .AnyAsync(k => k.TopNo == topNo);

            return Ok(exists);
        }

        [HttpGet("NetMt")]
        public async Task<ActionResult<decimal>> KumasTopNetMt(string bilgi)
        {
            var result = await _appDbContext.KumasTop
                .Where(k => k.TopNo == bilgi)
                .Select(k => k.NetMt)
                .FirstOrDefaultAsync();

            if (result == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }


    }
}
