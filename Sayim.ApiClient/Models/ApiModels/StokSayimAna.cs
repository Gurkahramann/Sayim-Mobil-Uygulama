using System;
using System.ComponentModel.DataAnnotations;

namespace Sayim.ApiClient.Models.ApiModels
{
    public class StokSayimAna
    {
        [Key]
        public int SayimNo { get; set; }
        public DateTime SayimTarihi { get; set; }
        public string SayimYapan { get; set; }
        public bool SayimiKapat { get; set; }
        public string Aciklama { get; set; }  
    }
}
