using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("DOTGIAMGIA")]
    public class DotGiamGia
    {
        [Key]
        public String MADOTGG { get; set; }
        public DateTime? NGAYBATDAU { get; set; }
        public DateTime? NGAYKETTHUC { get; set; }
        public String? MOTA { get; set; }
        public String? MANV { get; set; }
    }
}