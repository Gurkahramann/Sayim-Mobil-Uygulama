using System.ComponentModel.DataAnnotations;

namespace Sayim.Api.Models
{
    public class StokSayimAna
    {
        [Key]
        public int SayimNo { get; set; }
        public DateTime SayimTarihi { get; set; }
        public string? SayimYapan { get; set; }
        public bool SayimiKapat { get; set; }
        public string? Aciklama { get; set; }  
    }
}
