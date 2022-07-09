using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("SanPham")]
    public class SANPHAM
    {
        [Key]
        public String SERIAL { get; set; }
        public String IDGIOHANG { get; set; }
        public String MAPHIEUTRA { get; set; }
        public String MAPHIEUNHAP { get; set; }
        public String MALSP { get; set; }
        public String MASOBH { get; set; }
    }
}
