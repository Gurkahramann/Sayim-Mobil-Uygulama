using System.ComponentModel.DataAnnotations;

namespace Sayim.Api.Models
{
    public class Ambar
    {
        [Key]
        public string AmbarNo { get; set; }
        public string AmbarAdi { get; set; }
        public bool KullanimdaMi { get; set; }

    }

}
