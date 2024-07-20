using System.ComponentModel.DataAnnotations;

namespace Sayim.ApiClient.Models.ApiModels

{
    public class Ambar
    {
        [Key]
        public string AmbarNo { get; set; }
        public string AmbarAdi { get; set; }
        public bool KullanimdaMi { get; set; }

    }

}
