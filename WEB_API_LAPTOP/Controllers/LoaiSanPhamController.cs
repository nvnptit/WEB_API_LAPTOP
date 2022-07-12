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
    [Route("api/loai-san-pham")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoaiSanPhamController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult getLoaiSanPham()
        {
            BanLaptopEntities.connectionString = _configuration.GetConnectionString("DefaultConnection");
            using(var context=new BanLaptopEntities())
            {
                var data = context.LoaiSanPhams.ToList();
             /*   var x = context.LoaiSanPhams.Where(x => x.RAM == "ff").OrderBy(x => x.MALSP).ToList();*/
                return Ok(new { success = true, data = data });
            }

        }
        [HttpPost]
        public ActionResult themLoaiSanPham(LoaiSanPham model)
        {
            using (var context = new BanLaptopEntities())
            {
                var checkPK = context.LoaiSanPhams.Where(x => x.MALSP == model.MALSP).FirstOrDefault();
                if (checkPK != null)
                {
                    return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
                }
                var checkName= context.LoaiSanPhams.Where(x => x.TENLSP.ToLower().Trim() == model.TENLSP.ToLower().Trim()).FirstOrDefault();
                if (checkName != null)
                {
                    return Ok(new { success = false, message = "Đã tồn tại tên sản phẩm" });
                }
                context.LoaiSanPhams.Add(model);
                context.SaveChanges();
                return Ok(new { success = true, data = model });
            }
        }
        [HttpDelete]
        public ActionResult xoaLoaiSanPham(String maLSP)
        {
            using (var context = new BanLaptopEntities())
            {
                var checkSP = context.SanPhams.Where(x => x.MALSP == maLSP.Trim()).FirstOrDefault();
                if (checkSP!= null){
                    return Ok(new { success = false, message = "Đã tồn tại sản phẩm của Loại SP này" });
                }
                var loaiSP= context.LoaiSanPhams.Where(x => x.MALSP == maLSP.Trim()).FirstOrDefault();
                context.Entry(loaiSP).State = EntityState.Deleted;
                context.SaveChanges();
                return Ok(new { success = true, data = loaiSP });
            }
        }

    }
}
