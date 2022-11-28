using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("SANPHAM")]
    public class SanPham
    {
        [Key]
        public String SERIAL { get; set; }
        public int? MAPHIEUNHAP { get; set; }
        public String? MALSP { get; set; }
        public Boolean? TRANGTHAIMUA { get; set; }
    }
    public class SLSERI
    {
        public String? MALSP { get; set; }
        public String? TENLSP { get; set; }
        public int? SOLUONG { get; set; }
        public int? SL_SERI { get; set; }
    }
}