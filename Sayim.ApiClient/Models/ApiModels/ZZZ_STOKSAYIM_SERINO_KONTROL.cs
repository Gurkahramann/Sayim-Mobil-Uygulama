using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sayim.ApiClient.Models.ApiModels
{

    public partial class ZZZ_STOKSAYIM_SERINO_KONTROL
    {
        [StringLength(30)]
        public string? StokTurGrubu { get; set; }

        [StringLength(40)]
        public string? SeriNo { get; set; }

        [Column("miktar1", TypeName = "numeric(18, 6)")]
        public decimal? Miktar1 { get; set; }
    }
}
