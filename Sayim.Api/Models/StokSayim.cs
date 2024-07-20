using System.ComponentModel.DataAnnotations;

namespace Sayim.Api.Models
{
    public class StokSayim
    {
        [Key]
        public int SayimNo { get; set; }
        public int SiraNo { get; set; }
        public string? StokKodu { get; set; }
        public string? LotNo { get; set; }
        public string? SeriNo { get; set; }
        public decimal? Miktar1 { get; set; }
        public string? OlcuBirimi1 { get; set; }
        public decimal? Miktar2 { get; set; }
        public string? OlcuBirimi2 { get; set; }
        public decimal? Miktar3 { get; set; }
        public string? OlcuBirimi3 { get; set; }
        public string? AmbarNo { get; set; }
        public string? AmbarAdresi { get; set; }
        public string? DokumanNo { get; set; }

    }
}
