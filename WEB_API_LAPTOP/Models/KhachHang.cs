using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("KHACHHANG")]
    public class KhachHang
    {
        [Key]
        public String CMND { get; set; }
        public String? EMAIL { get; set; }
        public String? TEN { get; set; }
        public String? DIACHI { get; set; }
        public DateOnly? NGAYSINH { get; set; }
        public String? SDT { get; set; }
        public String? TENDANGNHAP { get; set; }
    }
}