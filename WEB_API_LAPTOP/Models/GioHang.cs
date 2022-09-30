using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API_LAPTOP.Models
{
    [Table("GIOHANG")]
    public class GioHang
    {
        [Key]
        public int? IDGIOHANG { get; set; }
        public DateTime? NGAYLAPGIOHANG { get; set; }
        public DateTime? NGAYDUKIEN { get; set; }
        public int? TONGGIATRI { get; set; }
        public int? MATRANGTHAI { get; set; }
        public String? CMND { get; set; }
        public String? MANVGIAO { get; set; }
        public String? MANVDUYET { get; set; }
        public String? NGUOINHAN { get; set; }
        public String? DIACHI { get; set; }
        public String? SDT { get; set; }
        public String? EMAIL { get; set; }
        public DateTime? NGAYNHAN { get; set; }
        public String? PHUONGTHUC { get; set; }

    }
    public class GioHangEditModel {

        public int? IDGIOHANG { get; set; }
        public DateTime? NGAYLAPGIOHANG { get; set; }
        public DateTime? NGAYDUKIEN { get; set; }
        public int? TONGGIATRI { get; set; }
        public int? MATRANGTHAI { get; set; }
        public String? MANVGIAO { get; set; }
        public String? MANVDUYET { get; set; }
        public String? NGUOINHAN { get; set; }
        public String? DIACHI { get; set; }
        public String? SDT { get; set; } 
        public String? EMAIL { get; set; }
        public DateTime? NGAYNHAN { get; set; }
        public String? PHUONGTHUC { get; set; }

    }
    public class GioHangViewModel
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
        public String SERIAL { get; set; }
        public int? IDGIOHANG { get; set; }

    }
    public class GioHangAdd
    {
        public int? IDGIOHANG { get; set; }
        public DateTime? NGAYLAPGIOHANG { get; set; }
        public DateTime? NGAYDUKIEN { get; set; }
        public int? TONGGIATRI { get; set; }
        public int? MATRANGTHAI { get; set; }
        public String? CMND { get; set; }
        public String? MANVGIAO { get; set; }
        public String? MANVDUYET { get; set; }
        public String? NGUOINHAN { get; set; }
        public String? DIACHI { get; set; }
        public String? SDT { get; set; }
        public String? EMAIL { get; set; }
        public String? MALSP { get; set; }
        public DateTime? NGAYNHAN { get; set; }
        public String? PHUONGTHUC { get; set; }
    }
    public class HistoryOrder
    {
        public int? IDGIOHANG { get; set; }
        public DateTime? NGAYLAPGIOHANG { get; set; }
        public DateTime? NGAYDUKIEN { get; set; }
        public int? TONGGIATRI { get; set; }
        public String? TENTRANGTHAI { get; set; }
        public String? NVGIAO { get; set; }
        public String? NVDUYET { get; set; }
        public String? NGUOINHAN { get; set; }
        public String? DIACHI { get; set; }
        public String? SDT { get; set; }
        public String? EMAIL { get; set; }
        public DateTime? NGAYNHAN { get; set; }
        public String? PHUONGTHUC { get; set; }

        //LSP

        public String? SERIAL { get; set; }
        public String TENLSP { get; set; }
        public string ANHLSP { get; set; }
        public String MOTA { get; set; }
        public String CPU { get; set; }
        public String RAM { get; set; }
        public String HARDDRIVE { get; set; }
        public String CARDSCREEN { get; set; }
        public String OS { get; set; }

    }
    public class DoanhThu
    {
        public int? THANG { get; set; }
        public int? NAM { get; set; }
        public int? DOANHTHU { get; set; }
    }

    public class HistoryOrder1
    {
        public int? IDGIOHANG { get; set; }
        public DateTime? NGAYLAPGIOHANG { get; set; }
        public DateTime? NGAYDUKIEN { get; set; }
        public int? TONGGIATRI { get; set; }
        public String? TENTRANGTHAI { get; set; }
        public String? NVGIAO { get; set; }
        public String? SDTNVG { get; set; }
        public String? NVDUYET { get; set; }
        public String? NGUOINHAN { get; set; }
        public String? DIACHI { get; set; }
        public String? SDT { get; set; }
        public String? EMAIL { get; set; }
        public DateTime? NGAYNHAN { get; set; }
        public String? PHUONGTHUC { get; set; }

    }

    public class HistoryOrder1Detail
    {
        public String? SERIAL { get; set; }
        public String TENLSP { get; set; }
        public string ANHLSP { get; set; }
        public String MOTA { get; set; }
        public String CPU { get; set; }
        public String RAM { get; set; }
        public String HARDDRIVE { get; set; }
        public String CARDSCREEN { get; set; }
        public String OS { get; set; }
        public int? GIABAN { get; set; }
        
    }

    public class GioHangAdd1
    {
        public int? IDGIOHANG { get; set; }
        public DateTime? NGAYLAPGIOHANG { get; set; }
        public DateTime? NGAYDUKIEN { get; set; }
        public int? TONGGIATRI { get; set; }
        public int? MATRANGTHAI { get; set; }
        public String? CMND { get; set; }
        public String? MANVGIAO { get; set; }
        public String? MANVDUYET { get; set; }
        public String? NGUOINHAN { get; set; }
        public String? DIACHI { get; set; }
        public String? SDT { get; set; }
        public String? EMAIL { get; set; }
        public String? MALSP { get; set; }
        public List<LoaiSanPhamViewModel> dslsp { get; set; }

        public DateTime? NGAYNHAN { get; set; }
        public String? PHUONGTHUC { get; set; }

    }
}