using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WEB_API_LAPTOP.Models
{
    [Table("LoaiSanPham")]
    public class LoaiSanPham
    { 
        [Key]
        public String MALSP { get; set; }
        public String TENLSP { get; set; }
        public int SOLUONG { get; set; }
        public String ANHLSP { get; set; }
        public String MOTA { get; set; }
        public String CPU { get; set; }
        public String RAM { get; set; }
        public String HARDDRIVE { get; set; }
        public String CARDSCREEN { get; set; }
        public String OS { get; set; }
        public int MAHANG { get; set; }
        public Boolean ISNEW { get; set; }
        public Boolean ISGOOD { get; set; }


    }
    public class LoaiSanPhamAddModel
    {
        public String MALSP { get; set; }
        public String TENLSP { get; set; }
        public int SOLUONG { get; set; }
        public IFormFile? ANHLSP { get; set; }
        public String MOTA { get; set; }
        public String CPU { get; set; }
        public String RAM { get; set; }
        public String HARDDRIVE { get; set; }
        public String CARDSCREEN { get; set; }
        public String OS { get; set; }
        public int MAHANG { get; set; }
        public Boolean ISNEW { get; set; }
        public Boolean ISGOOD { get; set; }
        public int GIAMOI { get; set; }
        public String MANV { get; set; }


    }

    public class LoaiSanPhamViewModel
    {
        public String MALSP { get; set; }
        public String TENLSP { get; set; }
        public int SOLUONG { get; set; }
        public string ANHLSP { get; set; }
        public String MOTA { get; set; }
        public String CPU { get; set; }
        public String RAM { get; set; }
        public String HARDDRIVE { get; set; }
        public String CARDSCREEN { get; set; }
        public String OS { get; set; }
        public int MAHANG { get; set; }
        public Boolean ISNEW { get; set; }
        public Boolean ISGOOD { get; set; }
        public int GIAMOI { get; set; }
        public int? PTGG { get; set; }
        public int? GIAGIAM { get; set; }


    }
}
