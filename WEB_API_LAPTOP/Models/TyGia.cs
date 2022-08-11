using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("TYGIA")]
    public class TyGia
    {
        [Key]
        public String MATG { get; set; }
        public DateTime NGAYAPDUNG { get; set; }
        public int GIATRI { get; set; }
        public String MANV { get; set; }
    }
}
}
