using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("PHIEUNHAP")]
    public class PhieuNhap
    {
        [Key]
        public int MAPN { get; set; }
        public DateTime NGAYTAO { get; set; }
        public int TONGTIEN { get; set; }
        public String MADONDH { get; set; }
        public String MANV { get; set; }
    }
}
