using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayim.Api.Data;
using Sayim.Api.Models;

namespace Sayim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonelController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public PersonelController(AppDbContext appDbContext) => _appDbContext = appDbContext;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personel>>> GetPersoneller(string kullaniciKodu)
        {
            var query = $@"
                SELECT Personelno, PersonelAdi, KullaniciKodu
                FROM Personel
                WHERE KullaniciKodu = @p0 AND IstenCikisTarihi IS NULL
                ORDER BY PersonelAdi
            ";

            var personeller = await _appDbContext.Personel
                .FromSqlRaw(query, kullaniciKodu)
                .ToListAsync();

            return Ok(personeller);
        }



    }
}
