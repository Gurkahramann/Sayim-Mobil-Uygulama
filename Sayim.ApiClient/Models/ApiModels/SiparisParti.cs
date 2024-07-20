using System.ComponentModel.DataAnnotations;

namespace Sayim.ApiClient.Models.ApiModels
{
    public class SiparisParti
    {
        [Key]
        public string PartiNo { get; set; }

        public string SiparisNo { get; set; }
        public decimal? Miktar1 { get; set; }
    }

}
