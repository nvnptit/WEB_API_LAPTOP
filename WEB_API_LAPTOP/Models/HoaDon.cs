using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("HOADON")]
    public class HoaDon
    {
        [Key]
        public String SOHD { get; set; }
        public DateTime NGAYLAPHD { get; set; }
        public String? MASOTHUE { get; set; }
        public int? IDGIOHANG { get; set; }
        public String? MANV { get; set; }
    }
}