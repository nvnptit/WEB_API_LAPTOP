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
    [Route("api/tai-khoan")]
    [ApiController]
    public class TaiKhoanController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public TaiKhoanController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [Route("login")]
        [HttpGet]
        public ActionResult checkLogin(String tenDangNhap, String matKhau)
        {
            //Lấy thì lấy ra giỏ hàng có idGioHang là giá trị cần tìm
            var taiKhoan = context.TaiKhoans.FirstOrDefault(x => x.TENDANGNHAP.Trim().Equals(tenDangNhap.Trim()) && x.MATKHAU.Trim().Equals(matKhau.Trim()));
            if (taiKhoan.MAQUYEN == 7)
            {
                var khachHang = context.KhachHangs.FirstOrDefault(x => x.TENDANGNHAP.Trim().Equals(tenDangNhap.Trim()));
                if (khachHang != null)
                    return Ok(new { success = true, data = khachHang });
            } else
            {
                var nhanVien = context.NhanViens.FirstOrDefault(x => x.TENDANGNHAP.Trim().Equals(tenDangNhap.Trim()));
                if (nhanVien != null)
                    return Ok(new { success = true, data = nhanVien });
            }
            return Ok(new { success = true, message = "Không tồn tại tài khoản này" });
        }
        [HttpPost]
        public ActionResult themTaiKhoan(TaiKhoan model)
        {
            var checkPK = context.TaiKhoans.Where(x => x.TENDANGNHAP == model.TENDANGNHAP.Trim()).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại tên đăng nhập này" });
            }
            context.TaiKhoans.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, message = "Tạo tài khoản mới thành công" });

        }

        [Route("thay-matkhau")]
        [HttpPut]
        public async Task<ActionResult> thayMatKhau(TaiKhoan model)
        {
            if (model != null)
            {
                context.Entry(model).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Thay đổi thành công" });
                return Ok(new { success = false, message = $"Thay đổi thất bại " });
            }
            return BadRequest();
        }
        
        [HttpDelete]
        public async Task<ActionResult> delTaiKhoan(String tenDangNhap)
        {
            if (tenDangNhap != null)
            {
                var taiKhoan = context.TaiKhoans.FirstOrDefault(x => x.TENDANGNHAP.Equals(tenDangNhap.Trim()));
                if (taiKhoan == null)
                    return NotFound();
                context.TaiKhoans.Remove(taiKhoan);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá thành công" });
                return Ok(new { success = false, message = $"xoá thất bại" });
            }
            return BadRequest();
        }

    }
}
