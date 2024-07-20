using System.ComponentModel.DataAnnotations;

namespace Sayim.Api.Models
{
    public class Kullanici
    {
        [Key]
        public string? KullaniciKodu { get; set; }
        public string? KullaniciAdi { get; set; }
        public string? Sifre { get; set; }
        public string? KullaniciTanimi { get; set; }
    }
}
