using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("CTDONDATHANG")]
    public class CTDonDatHang
    {
        public String MADONDH { get; set; }
        public String MALSP { get; set; }
        public int? SOLUONG { get; set; }
        public int? GIA { get; set; }
    }
}