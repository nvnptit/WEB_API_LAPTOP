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
    [Route("api/khach-hang")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public KhachHangController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getkhachHang(string? cmnd)
        {
            //Kiểm tra xem có get bằng serial hay không
            //Không thì lấy toàn bộ sản phẩm
            if (string.IsNullOrEmpty(cmnd))
            {
                var lstKhachHangs = context.KhachHangs.ToList();
                return Ok(new { success = true, data = lstKhachHangs });
            }
            //Lấy thì lấy ra sản phẩm có serial là giá trị cần tìm
            var khachHang = context.KhachHangs.FirstOrDefault(x => x.CMND.Trim().Equals(cmnd.Trim()));
            if (khachHang != null)
                return Ok(new { success = true, data = khachHang });
            return Ok(new { success = true, message = "Không tồn tại nhân viên này" });
        }

        [HttpPost]
        public ActionResult themKhachHang(KhachHang model)
        {
            var checkPK = context.KhachHangs.Where(x => x.CMND == model.CMND.Trim()).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
            }

            context.KhachHangs.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, data = model });

        }

        [HttpPut]
        public async Task<ActionResult> editKhachHang(KhachHang khachHang)
        {
            if (khachHang != null)
            {
                context.Entry(khachHang).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công {khachHang.CMND}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại {khachHang.CMND}" });
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> delKhachHang(string CMND) // , IFormFile file
        {
            if (!string.IsNullOrEmpty(CMND))
            {
                var khachHang = context.KhachHangs.FirstOrDefault(x => x.CMND.Trim().Equals(CMND.Trim()));
                if (khachHang == null)
                    return NotFound();
                context.KhachHangs.Remove(khachHang);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá thành công khách hàng" });
                return Ok(new { success = false, message = $"xoá thất bại khách hàng" });
            }
            return BadRequest();
        }

    }
}
