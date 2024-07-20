using System.ComponentModel.DataAnnotations;

namespace Sayim.ApiClient.Models.ApiModels
{
    public class Personel
    {
        [Key]
        public string PersonelNo { get; set; }
        public string PersonelAdi { get; set; }
        public string KullaniciKodu { get; set; }
    }
}
