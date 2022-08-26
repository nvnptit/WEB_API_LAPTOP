using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using WEB_API_LAPTOP.Models;
using WEB_API_LAPTOP.Helper;


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
            var taiKhoan = context.TaiKhoans.FirstOrDefault(x => x.TENDANGNHAP.Trim().Equals(tenDangNhap.Trim()) && x.MATKHAU.Trim().Equals(matKhau.Trim()));
            if (taiKhoan != null)
            {
                if (!taiKhoan.KICHHOAT)
                {
                    return Ok(new { success = false, message = "Tài khoản đã bị vô hiệu hoá" });
                }

                if (taiKhoan.MAQUYEN == 7)
                {
                    TaiKhoanKH tk = new TaiKhoanKH();
                    var khachHang = context.KhachHangs.FirstOrDefault(x => x.TENDANGNHAP.Trim().Equals(tenDangNhap.Trim()));
                    if (khachHang != null)
                    {
                        tk.setData(khachHang.CMND, khachHang.EMAIL, khachHang.TEN, khachHang.DIACHI, khachHang.NGAYSINH, khachHang.SDT, khachHang.TENDANGNHAP, taiKhoan.MAQUYEN);
                    }
                        return Ok(new { success = true, data = tk });
                }
                else
                {
                    TaiKhoanNV tk = new TaiKhoanNV();
                    var nhanVien = context.NhanViens.FirstOrDefault(x => x.TENDANGNHAP.Trim().Equals(tenDangNhap.Trim()));
                    if (nhanVien != null)
                    {
                        tk.setData(nhanVien.MANV, nhanVien.EMAIL, nhanVien.TEN, nhanVien.NGAYSINH, nhanVien.SDT, nhanVien.TENDANGNHAP, taiKhoan.MAQUYEN);
                    }
                    return Ok(new { success = true, data = tk });
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
        [Route("Quyen")]
        [HttpPut]
        public async Task<ActionResult> thayQuyen(TaiKhoanQuyenKichHoat model)
        {

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@tendangnhap", model.TENDANGNHAP));
                param.Add(new SqlParameter("@maquyen", model.MAQUYEN));
                param.Add(new SqlParameter("@kichhoat", model.KICHHOAT));

                var data = new SQLHelper(_configuration).ExecuteQuery("spUpdateQuyen_KichHoat", param);
                return Ok(new { success = true, message = "Cập nhật quyền và trạng thái thành công" });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, data = "Đã có lỗi xảy ra!" });
            }

            /*if (model != null)
            {
                context.Entry(model).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Thay đổi thành công" });
                return Ok(new { success = false, message = $"Thay đổi thất bại " });
            }
            return BadRequest();*/
        }

        [HttpDelete]
        public async Task<ActionResult> delTaiKhoan(String tenDangNhap)
        {
            if (tenDangNhap != null)
            {
                var taiKhoan = context.TaiKhoans.FirstOrDefault(x => x.TENDANGNHAP == tenDangNhap);
                if (taiKhoan == null)
                {
                    return Ok(new { success = false, message = $"xoá thất bại" });
                }
                context.TaiKhoans.Remove(taiKhoan);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá thành công" });
            }
            return Ok(new { success = false, message = $"xoá thất bại" });
        }

    }
}
