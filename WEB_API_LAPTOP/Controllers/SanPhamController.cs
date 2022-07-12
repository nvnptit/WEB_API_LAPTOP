using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using WEB_API_LAPTOP.Helper;
using WEB_API_LAPTOP.Models;


namespace WEB_API_LAPTOP.Controllers
{
    [Route("api/san-pham")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public SanPhamController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

    /*    [HttpGet]
        public ActionResult getSanPham()
        {
            BanLaptopEntities.connectionString = _configuration.GetConnectionString("DefaultConnection");
          *//*  using (var context = new BanLaptopEntities())
            {
                *//*   var x = context.LoaiSanPhams.Where(x => x.RAM == "ff").OrderBy(x => x.MALSP).ToList();*//*
                return Ok(new { success = true, data = data });
            }*//*


        }*/
        [HttpPost]
        public ActionResult themSanPham(SanPham model)
        {
            using (var context = new BanLaptopEntities())
            {
                var checkPK = context.SanPhams.Where(x => x.SERIAL == model.SERIAL).FirstOrDefault();
                if (checkPK != null)
                {
                    return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
                }
                
                context.SanPhams.Add(model);
                context.SaveChanges();
                return Ok(new { success = true, data = model });
            }

            /* var list = new SQLHelper().ExecuteString("SELECT * FROM SanPham");*/


        }
}
}
