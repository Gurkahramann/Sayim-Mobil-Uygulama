using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayim.Api.Data;
using Sayim.Api.Models;

namespace Sayim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmbarAdresController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public AmbarAdresController(AppDbContext appDbContext) => _appDbContext = appDbContext;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmbarAdres>>> GetAmbarAdresler(string ambarNo)
        {
            var query = await _appDbContext.AmbarAdres
                .Where(a => a.AmbarNo == ambarNo)
                .Select(a => new
                {
                    a.AmbarNo,
                    a.X,
                    a.Y,
                    a.Z,
                    Adres = EF.Functions.Like(a.X.Trim() + '.' + a.Y.Trim() + '.' + a.Z.Trim(), "%")
                })
                .Distinct()
                .OrderBy(a => a.Adres)
                .ToListAsync();

            return Ok(query);
        }

        [HttpGet("Kontrol")]
        public async Task<ActionResult<bool>> AmbarAdresKontrol(string ambarNo, string adres)
        {
            var exists = await _appDbContext.AmbarAdres
                .AnyAsync(a => a.AmbarNo == ambarNo && (a.X.Trim() + '.' + a.Y.Trim() + '.' + a.Z.Trim()) == adres);

            return Ok(exists);
        }


    }
}
