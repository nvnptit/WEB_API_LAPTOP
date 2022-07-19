using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("GIATHAYDOI")]
    public class GiaThayDoi
    {
        public String MALSP { get; set; }
        public DateTime? NGAYAPDUNG { get; set; }
        public String? MANV { get; set; }
        public int? GIAMOI { get; set; }
    }
}