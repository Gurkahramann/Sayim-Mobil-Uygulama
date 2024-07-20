using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayim.Api.Data;
using Sayim.Api.Models;

namespace Sayim.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZZZ_StokSayim_SeriNo_Controller : Controller
    {
        private readonly AppDbContext _appDbContext;
        public ZZZ_StokSayim_SeriNo_Controller(AppDbContext appDbContext) => _appDbContext = appDbContext;
        // GET: api/ZZZ_StokSayim_SeriNo/Kontrol?seriNo=xyz
        [HttpGet("SeriNoKontrol")]
        public async Task<ActionResult<bool>> SeriNoKontrol(string seriNo)
        {
            var adet = await _appDbContext.ZZZ_STOKSAYIM_SERINO_KONTROL
                .CountAsync(sp => sp.SeriNo == seriNo);

            return Ok(adet > 0);
        }

        // GET: api/ZZZ_StokSayim_SeriNo/Miktar1?seriNo=xyz
        [HttpGet("SeriNoMiktar1")]
        public async Task<ActionResult<decimal>> SeriNoMiktar1(string seriNo)
        {
            var miktar1 = await _appDbContext.ZZZ_STOKSAYIM_SERINO_KONTROL
                .Where(sp => sp.SeriNo == seriNo)
                .Select(sp => sp.Miktar1)
                .FirstOrDefaultAsync();

            return Ok(miktar1);
        }
        [HttpGet("GetMaxSayimNo")]
        public async Task<ActionResult<int>> GetMaxSayimNo()
        {
            var maxSayimNo = await _appDbContext.StokSayimAna.MaxAsync(sa => (int?)sa.SayimNo) ?? 0;
            return Ok(maxSayimNo + 1); // Yeni sayım numarası için mevcut en yüksek numarayı 1 artırarak döndür
        }

        [HttpPost("InsertStokSayimAna")]
        public async Task<ActionResult<bool>> InsertStokSayimAna(StokSayimAna stokSayimAna)
        {
            _appDbContext.StokSayimAna.Add(stokSayimAna);
            var result = await _appDbContext.SaveChangesAsync() > 0;
            return Ok(result);
        }

        [HttpPost("InsertStokSayim")]
        public async Task<ActionResult<bool>> InsertStokSayim(StokSayim stokSayim)
        {
            _appDbContext.StokSayim.Add(stokSayim);
            var result = await _appDbContext.SaveChangesAsync() > 0;
            return Ok(result);
        }
    }
}
