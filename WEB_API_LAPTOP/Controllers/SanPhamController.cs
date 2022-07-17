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
        private readonly BanLaptopEntities context;
        public SanPhamController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpPut]
        public async Task<ActionResult> editSanPham(SanPham sanPham)
        {
            if (sanPham != null)
            {
                context.Entry(sanPham).State = EntityState.Modified; // ý nghĩa dòng này e 31 32
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công {sanPham.SERIAL}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại {sanPham.SERIAL}" });
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> delSanPham(string serial) // , IFormFile file
        {
 /*           if(file!=null)
            {
                var guid = new Guid();
                string url = "/images/";
                string filename = file.FileName;
                string path = url + guid.ToString() + Path.GetExtension(filename);
                using(var streamfile=new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(streamfile);
                }
            }    */
            if (!string.IsNullOrEmpty(serial))
            {
                var sanpham = context.SanPhams.FirstOrDefault(x => x.SERIAL.Trim().Equals(serial));
                if (sanpham == null)
                    return NotFound();
                 context.SanPhams.Remove(sanpham);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá thành công {serial}" });
                return Ok(new { success = false, message = $"xoá thất bại {serial}" });
            }
            return BadRequest();
        }

        [HttpGet]
        public ActionResult getSanPham(string? SERIAL)
        {
            //Kiểm tra xem có get bằng serial hay không
            //Không thì lấy toàn bộ sản phẩm
            if (string.IsNullOrEmpty(SERIAL))
            {
                var lstSanPhams = context.SanPhams.ToList();
                return Ok(new { success = true, data = lstSanPhams });
            }
            //Lấy thì lấy ra sản phẩm có serial là giá trị cần tìm
            var SanPham = context.SanPhams.FirstOrDefault(x => x.SERIAL.Trim().Equals(SERIAL));
            if(SanPham!=null)
                 return Ok(new { success = true, data = SanPham });
            return Ok(new { success = true, message = "Không tồn tại sản phẩm này" });
        }
        [HttpPost]
        public ActionResult themSanPham(SanPham model)
        {
                var checkPK = context.SanPhams.Where(x => x.SERIAL == model.SERIAL).FirstOrDefault();
                if (checkPK != null)
                {
                    return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
                }
                
                context.SanPhams.Add(model);
                context.SaveChanges();
                return Ok(new { success = true, data = model });

            /* var list = new SQLHelper().ExecuteString("SELECT * FROM SanPham");*/


        }
}
}
