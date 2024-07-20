using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayim.Api.Data;
using System.Linq.Dynamic.Core;

namespace Sayim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiparisPartiController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public SiparisPartiController(AppDbContext appDbContext) => _appDbContext = appDbContext;

        [HttpGet("PartiNoKontrol")]
        public async Task<ActionResult<bool>> PartiNoKontrol(string partiNo)
        {
            var exists = await _appDbContext.SiparisParti
                .AnyAsync(sp => sp.PartiNo == partiNo);

            return Ok(exists);
        }


        // GET: api/SiparisParti/SiparisPartiMiktar1?partiNo=xyz
        [HttpGet("SiparisPartiMiktar1")]
        public async Task<ActionResult<decimal>> SiparisPartiMiktar1(string partiNo)
        {
            var result = await _appDbContext.SiparisParti
                .Where(sp => sp.PartiNo == partiNo)
                .Select(sp => sp.Miktar1)
                .FirstOrDefaultAsync();

            if (result == default)
            {
                return Ok(0); // or you can use NotFound() if you want to return 404 when not found
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<int>> CountAsync(string tableName, string columnName, string? filter = null, bool where = false)
        {
            string sql = $"SELECT COUNT({columnName}) AS Sonuc FROM {tableName} WITH (NOLOCK)";
            if (where && !string.IsNullOrEmpty(filter))
            {
                sql += $" WHERE {filter}";
            }

            var result = await _appDbContext.Database.ExecuteScalarAsync<int>(sql);
            return Ok(result);
        }

    }
}

