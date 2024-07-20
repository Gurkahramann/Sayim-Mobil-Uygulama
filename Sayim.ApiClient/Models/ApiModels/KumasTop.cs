using System.ComponentModel.DataAnnotations;

namespace Sayim.ApiClient.Models.ApiModels
{
    public class KumasTop
    {
        [Key]
        public string TopNo { get; set; }
        public decimal? NetMt { get; set; }

    }
}
