using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Sayim.Api.Models;

namespace Sayim.Api.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Ambar> Ambar { get; set; }
        public DbSet<AmbarAdres> AmbarAdres { get; set; }
        public DbSet<Kullanici> Kullanici { get; set; }
        public DbSet<Personel> Personel { get; set; }
        public DbSet<StokSayim> StokSayim { get; set; }
        public DbSet<StokSayimAna> StokSayimAna { get; set; }
        public DbSet<SiparisParti> SiparisParti { get; set; }
        public DbSet<KumasTop> KumasTop { get; set; }
        public DbSet<ZZZ_STOKSAYIM_SERINO_KONTROL> ZZZ_STOKSAYIM_SERINO_KONTROL { get; set; }
    }
}
