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
    [Route("api/gio-hang")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public GioHangController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getGioHang(int? idGioHang)
        {
            //Kiểm tra xem có get bằng idGioHang hay không
            //Không thì lấy toàn bộ 
            if (idGioHang == null)
            {
                var lstGioHangs = context.GioHangs.ToList();
                return Ok(new { success = true, data = lstGioHangs });
            }
            //Lấy thì lấy ra giỏ hàng có idGioHang là giá trị cần tìm
            var gioHang = context.GioHangs.FirstOrDefault(x => x.IDGIOHANG.Equals(idGioHang));
            if (gioHang != null)
                return Ok(new { success = true, data = gioHang });
            return Ok(new { success = true, message = "Không tồn tại giỏ hàng này" });
        }
        [HttpPost]
        public ActionResult themGioHang(GioHang model)
        {
            var checkPK = context.GioHangs.Where(x => x.IDGIOHANG == model.IDGIOHANG).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
            }
            context.GioHangs.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, data = model });

        }

        [HttpPut]
        public async Task<ActionResult> editGioHang(GioHang gioHang)
        {
            if (gioHang != null)
            {
                context.Entry(gioHang).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công {gioHang.IDGIOHANG}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại {gioHang.IDGIOHANG}" });
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> delGioHang(int idGioHang)
        {
            if (idGioHang != null)
            {
                var gioHang = context.GioHangs.FirstOrDefault(x => x.IDGIOHANG.Equals(idGioHang));
                if (gioHang == null)
                    return NotFound();
                context.GioHangs.Remove(gioHang);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá thành công" });
                return Ok(new { success = false, message = $"xoá thất bại" });
            }
            return BadRequest();
        }

    }
}
