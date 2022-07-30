using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
        [HttpPost]
        public ActionResult checkLogin(TaiKhoanLogin model)
        {
            String tenDangNhap = model.TENDANGNHAP;
            String matKhau = model.MATKHAU;

            //Lấy thì lấy ra giỏ hàng có idGioHang là giá trị cần tìm
            var taiKhoan = context.TaiKhoans.FirstOrDefault(x => x.TENDANGNHAP.Trim().Equals(tenDangNhap.Trim()) && x.MATKHAU.Trim().Equals(matKhau.Trim()));
            if (taiKhoan != null)
            {
                if (!taiKhoan.KICHHOAT)
                {
                    return Ok(new { success = true, message = "Tài khoản đã bị vô hiệu hoá" });
                }

                if (taiKhoan.MAQUYEN == 7)
                {
                    var khachHang = context.KhachHangs.FirstOrDefault(x => x.TENDANGNHAP.Trim().Equals(tenDangNhap.Trim()));
                    if (khachHang != null)
                        return Ok(new { success = true, data = khachHang });
                }
                else
                {
                    var nhanVien = context.NhanViens.FirstOrDefault(x => x.TENDANGNHAP.Trim().Equals(tenDangNhap.Trim()));
                    if (nhanVien != null)
                        return Ok(new { success = true, data = nhanVien });
                }
            }
            return Ok(new { success = false, message = "Tài khoản hoặc mật khẩu không đúng" });
        }
        [HttpPost]
        public ActionResult themTaiKhoan(TaiKhoanNew model)
        {
            TaiKhoan tk = new TaiKhoan();
            tk.TENDANGNHAP = model.TENDANGNHAP;
            tk.MATKHAU = model.MATKHAU;
            tk.MAQUYEN = model.MAQUYEN;
            tk.KICHHOAT = true;
            if (model.MAQUYEN == 0)
            {
                return Ok(new { success = false, message = "Quyền không tồn tại" });
            }
            var checkPK = context.TaiKhoans.Where(x => x.TENDANGNHAP == model.TENDANGNHAP.Trim()).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại tên đăng nhập này" });
            }
            context.TaiKhoans.Add(tk);
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
