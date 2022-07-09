using Microsoft.EntityFrameworkCore;
using WEB_API_LAPTOP.Models;

namespace WEB_API_LAPTOP
{
    public class BanLaptopEntities : DbContext
    {

        public static String connectionString;
        public DbSet<LoaiSanPham>? LoaiSanPhams { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

    }
}
