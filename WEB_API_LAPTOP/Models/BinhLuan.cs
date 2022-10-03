using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("BINHLUAN")]
    public class BinhLuan
    {
        [Key]
        public String? CMND { get; set; }
        public String? SERIAL { get; set; }
        public DateTime? NGAYBINHLUAN { get; set; }
        public int? DIEM { get; set; }
        public String? MOTA { get; set; }
    }
    public class BinhLuanModel
    {
        public String? CMND { get; set; }
        public String? SERIAL { get; set; }
        public DateTime? NGAYBINHLUAN { get; set; }
        public int? DIEM { get; set; }
        public String? MOTA { get; set; }
    }
}
