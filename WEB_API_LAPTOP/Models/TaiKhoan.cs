using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("TaiKhoan")]
    public class TaiKhoan
    {
        [Key]
        public String TENDANGNHAP { get; set; }
        public String MATKHAU { get; set; }
        public int MAQUYEN { get; set; }
    }
}