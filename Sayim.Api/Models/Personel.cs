using System.ComponentModel.DataAnnotations;

namespace Sayim.Api.Models
{
    public class Personel
    {
        [Key]
        public string PersonelNo { get; set; }
        public string PersonelAdi { get; set; }
        public string KullaniciKodu { get; set; }
    }
}
