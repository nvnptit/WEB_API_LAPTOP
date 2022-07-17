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
    [Route("api/hang-sx")]
    [ApiController]
    public class HangSXController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public HangSXController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getHangSX(int? maHang)
        {
            //Kiểm tra xem có get bằng serial hay không
            //Không thì lấy toàn bộ sản phẩm
            if (maHang == null)
            {
                var lstHangSXs = context.HangSXs.ToList();
                return Ok(new { success = true, data = lstHangSXs });
            }
            //Lấy thì lấy ra sản phẩm có serial là giá trị cần tìm
            var hangSX = context.HangSXs.FirstOrDefault(x => x.MAHANG.Equals(maHang));
            if (hangSX != null)
                return Ok(new { success = true, data = hangSX });
            return Ok(new { success = true, message = "Không tồn tại sản phẩm này" });
        }
        [HttpPost]
        public ActionResult themHang(HangSX model)
        {
            var checkPK = context.HangSXs.Where(x => x.MAHANG == model.MAHANG).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
            }

            context.HangSXs.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, data = model });


        }

        [HttpPut]
        public async Task<ActionResult> editHang(HangSX hangSX)
        {
            if (hangSX != null)
            {
                context.Entry(hangSX).State = EntityState.Modified; // ý nghĩa dòng này e 31 32
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công {hangSX.TENHANG}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại {hangSX.TENHANG}" });
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> delHang(int? maHang) // , IFormFile file
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
            if (maHang != null)
            {
                var hangSX = context.HangSXs.FirstOrDefault(x => x.MAHANG.Equals(maHang));
                if (hangSX == null)
                    return NotFound();
                context.HangSXs.Remove(hangSX);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá thành công {maHang}" });
                return Ok(new { success = false, message = $"xoá thất bại {maHang}" });
            }
            return BadRequest();
        }

    }
}
