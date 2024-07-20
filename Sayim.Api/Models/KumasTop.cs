using System.ComponentModel.DataAnnotations;

namespace Sayim.Api.Models
{
    public class KumasTop
    {
        [Key]
        public string TopNo { get; set; }
        public decimal? NetMt { get; set; }

    }
}
