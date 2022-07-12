using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("SanPham")]
    public class SanPham
    {
        [Key]
        public String SERIAL { get; set; }
        public int? IDGIOHANG { get; set; }
        public int? MAPHIEUTRA { get; set; }
        public int? MAPHIEUNHAP { get; set; }
        public String? MALSP { get; set; }
    }
}