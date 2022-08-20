using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("CTGIAMGIA")]
    public class CTGiamGia
    {
        public String MALSP { get; set; }
        public String? MADOTGG { get; set; }
        public int? PHANTRAMGG { get; set; }
    }
}