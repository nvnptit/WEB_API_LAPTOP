using Microsoft.EntityFrameworkCore;
using WEB_API_LAPTOP.Models;

namespace WEB_API_LAPTOP
{
    public class BanLaptopEntities : DbContext
    {
        public BanLaptopEntities()
        {

        }
        public BanLaptopEntities(DbContextOptions<BanLaptopEntities> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GiaThayDoi>().HasKey(c => new { c.NGAYAPDUNG, c.MALSP });
        }
        public static String connectionString;
        public DbSet<LoaiSanPham>? LoaiSanPhams { get; set; }
        public DbSet<SanPham>? SanPhams { get; set; }
        public DbSet<HangSX>? HangSXs { get; set; }
        public DbSet<GiaThayDoi>? GiaThayDois { get; set; }
        public DbSet<GioHang>? GioHangs { get; set; }
        public DbSet<HoaDon>? HoaDons { get; set; }
        public DbSet<TaiKhoan>? TaiKhoans { get; set; }
        public DbSet<Quyen>? Quyens { get; set; }
        public DbSet<NhanVien>? NhanViens { get; set; }
        public DbSet<KhachHang>? KhachHangs { get; set; }
        public DbSet<TyGia>? TyGias { get; set; }
        public DbSet<PhieuNhap>? PhieuNhaps { get; set; }

    }
}
