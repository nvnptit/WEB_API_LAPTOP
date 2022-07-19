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
    [Route("api/hoa-don")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public HoaDonController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getHoaDon(String maHD)
        {
            //Kiểm tra xem có get bằng idGioHang hay không
            //Không thì lấy toàn bộ 
            if (string.IsNullOrEmpty(maHD))
            {
                var lstHoaDons = context.HoaDons.ToList();
                return Ok(new { success = true, data = lstHoaDons });
            }
            //Lấy thì lấy ra giỏ hàng có idGioHang là giá trị cần tìm
            var hoaDon = context.HoaDons.FirstOrDefault(x => x.SOHD.Trim().Equals(maHD));
            if (hoaDon != null)
                return Ok(new { success = true, data = hoaDon });
            return Ok(new { success = true, message = "Không tồn tại hoá đơn này" });
        }
        [HttpPost]
        public ActionResult themHoaDon(HoaDon model)
        {
            var checkPK = context.HoaDons.Where(x => x.SOHD == model.SOHD).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
            }
            context.HoaDons.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, data = model });

        }

        [HttpPut]
        public async Task<ActionResult> editHoaDon(HoaDon hoaDon)
        {
            if (hoaDon != null)
            {
                context.Entry(hoaDon).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công hoá đơn {hoaDon.SOHD}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại hoá đơn {hoaDon.SOHD}" });
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> delHoaDon(String soHD)
        {
            if (!string.IsNullOrEmpty(soHD))
            {
                var hoaDon = context.HoaDons.FirstOrDefault(x => x.SOHD.Trim().Equals(soHD));
                if (hoaDon == null)
                    return NotFound();
                context.HoaDons.Remove(hoaDon);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá thành công" });
                return Ok(new { success = false, message = $"xoá thất bại" });
            }
            return BadRequest();
        }

    }
}
