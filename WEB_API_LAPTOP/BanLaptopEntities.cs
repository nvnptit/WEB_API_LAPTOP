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

        public static String connectionString;
        public DbSet<LoaiSanPham>? LoaiSanPhams { get; set; }
        public DbSet<SanPham>? SanPhams { get; set; }

    }
}
