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
        public bool KICHHOAT { get; set; }
    }
    public class TaiKhoanLogin
    {
        public String TENDANGNHAP { get; set; }
        public String MATKHAU { get; set; }

    }
    public class TaiKhoanNew
    {
        public String TENDANGNHAP { get; set; }
        public String MATKHAU { get; set; }
        public int MAQUYEN { get; set; }

    }
    public class TaiKhoanKH
    {
        public String CMND { get; set; }
        public String EMAIL { get; set; }
        public String TEN { get; set; }
        public String DIACHI { get; set; }
        public DateTime NGAYSINH { get; set; }
        public String SDT { get; set; }
        public String? TENDANGNHAP { get; set; }
        public int MAQUYEN { get; set; }

        public void setData(string cmnd, string email, string ten, string diachi, DateTime date, string sdt, string tendn, int maquyen)
        {
            this.CMND = cmnd;
            EMAIL = email;
            TEN = ten;
            DIACHI = diachi;
            NGAYSINH = date;
            SDT = sdt;
            TENDANGNHAP = tendn;
            MAQUYEN = maquyen;
        }
    }
    public class TaiKhoanNV
    {
        public String MANV { get; set; }
        public String EMAIL { get; set; }
        public String TEN { get; set; }
        public DateTime NGAYSINH { get; set; }
        public String SDT { get; set; }
        public String? TENDANGNHAP { get; set; }
        public int MAQUYEN { get; set; }

        public void setData(string manv, string email, string ten, DateTime date, string sdt, string tendn, int maquyen)
        {
            MANV = manv;
            EMAIL = email;
            TEN = ten;
            NGAYSINH = date;
            SDT = sdt;
            TENDANGNHAP = tendn;
            MAQUYEN = maquyen;
        }
    }
    public class TaiKhoanQuyenKichHoat
    {
        public String TENDANGNHAP { get; set; }
        public int MAQUYEN { get; set; }
        public bool KICHHOAT { get; set; }
    }
}