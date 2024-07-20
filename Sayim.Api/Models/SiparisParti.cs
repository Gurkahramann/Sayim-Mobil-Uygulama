using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Sayim.Api.Models
{
    public class SiparisParti()
    {
        [Key]
        public string PartiNo { get; set; }

        public string SiparisNo { get; set; }
        public decimal? Miktar1 { get; set; }
    }

}
