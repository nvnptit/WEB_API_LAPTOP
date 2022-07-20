using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("QUYEN")]
    public class Quyen
    {
        [Key]
        public int MAQUYEN { get; set; }
        public String? TENQUYEN { get; set; }
    }
}