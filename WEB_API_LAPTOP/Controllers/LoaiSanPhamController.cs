using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using WEB_API_LAPTOP.Helper;
using WEB_API_LAPTOP.Models;

namespace WEB_API_LAPTOP.Controllers
{
    [Route("api/loai-san-pham")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        [HttpGet]
        public ActionResult getLoaiSanPham()
        {
            using(var context=new BanLaptopEntities())
            {
                BanLaptopEntities.connectionString = context.Database.GetDbConnection().ConnectionString;
                var list = (from loaiSP in context.LoaiSanPhams select loaiSP).ToArray();
               
                return Ok(new { success = true, data=list});
            }
           
            /* var list = new SQLHelper().ExecuteString("SELECT * FROM LoaiSanPham");*/
           

        }
        [HttpPost]
        public ActionResult themLoaiSanPham(LoaiSanPham model)
        {
            using (var context = new BanLaptopEntities())
            {
                context.LoaiSanPhams.Add(model);
                context.SaveChanges();
                return Ok(new { success = true, data =model});
            }

            /* var list = new SQLHelper().ExecuteString("SELECT * FROM LoaiSanPham");*/


        }
    }
}
