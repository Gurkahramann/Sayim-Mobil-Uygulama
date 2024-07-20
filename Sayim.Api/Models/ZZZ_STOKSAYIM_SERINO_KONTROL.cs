using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sayim.Api.Models
{

    [Keyless]
    public partial class ZZZ_STOKSAYIM_SERINO_KONTROL
    {
        [StringLength(30)]
        [Unicode(false)]
        public string? StokTurGrubu { get; set; }

        [StringLength(40)]
        [Unicode(false)]
        public string? SeriNo { get; set; }

        [Column("miktar1", TypeName = "numeric(18, 6)")]
        public decimal? Miktar1 { get; set; }
    }
}
