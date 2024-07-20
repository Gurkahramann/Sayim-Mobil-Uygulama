using System.ComponentModel.DataAnnotations;

namespace Sayim.Api.Models
{
    public class AmbarAdres
    {
        [Key]
        public string AmbarNo { get; set; }
        public string? X { get; set; }
        public string? Y { get; set; }
        public string? Z { get; set; }
        public string? Adres => $"{X.Trim()}.{Y.Trim()}.{Z.Trim()}";
    }
}
