using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("CHITIETDONHANG")]
    public class CHITIETDONHANG
    {
        public int? IDDONHANG { get; set; }
        public String? SERIAL { get; set; }
        public int? MAPHIEUTRA { get; set; }
        public int? GIABAN { get; set; }
    }
}