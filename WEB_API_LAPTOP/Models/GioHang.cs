using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("GIOHANG")]
    public class GioHang
    {
        [Key]
        public int? IDGIOHANG { get; set; }
        public DateTime NGAYLAPGIOHANG { get; set; }
        public int? TONGGIATRI { get; set; }
        public int? MATRANGTHAI { get; set; }
        public String? CMND { get; set; }
        public String? MANVGIAO { get; set; }
        public String? MANVDUYET { get; set; }
        public String? NGUOINHAN { get; set; }
        public String? DIACHI { get; set; }
        public String? SDT { get; set; }
        public String? EMAIL { get; set; }
    }
    public class GioHangEditModel {

        public int? IDGIOHANG { get; set; }
        public DateTime NGAYLAPGIOHANG { get; set; }
        public int? MATRANGTHAI { get; set; }
        public String? MANVGIAO { get; set; }
        public String? MANVDUYET { get; set; }
        public String? NGUOINHAN { get; set; }
        public String? DIACHI { get; set; }
        public String? SDT { get; set; }
        public String? EMAIL { get; set; }

    }

}