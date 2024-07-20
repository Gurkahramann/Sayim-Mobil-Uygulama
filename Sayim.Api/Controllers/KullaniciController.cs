using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayim.Api.Data;
using Sayim.Api.Models;

namespace Sayim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public KullaniciController(AppDbContext appDbContext) => _appDbContext = appDbContext;
        [HttpGet]
        public async Task<ActionResult<Kullanici>> GetKullanici(string kullaniciKodu, string sifre)
        {
            var query = $@"
                SELECT *
                FROM Kullanici
                WHERE KullaniciKodu = @p0 AND Sifre = @p1";

            var kullanici = await _appDbContext.Kullanici
                .FromSqlRaw(query, kullaniciKodu, sifre)
                .FirstOrDefaultAsync();

            if (kullanici == null)
            {
                return NotFound();
            }

            return Ok(kullanici);
        }
    }
}
