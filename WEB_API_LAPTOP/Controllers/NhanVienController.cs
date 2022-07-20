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
    [Route("api/nhan-vien")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public NhanVienController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getNhanVien(string? maNV)
        {
            //Kiểm tra xem có get bằng serial hay không
            //Không thì lấy toàn bộ sản phẩm
            if (string.IsNullOrEmpty(maNV))
            {
                var lstNhanViens = context.NhanViens.ToList();
                return Ok(new { success = true, data = lstNhanViens });
            }
            //Lấy thì lấy ra sản phẩm có serial là giá trị cần tìm
            var nhanVien = context.NhanViens.FirstOrDefault(x => x.MANV.Trim().Equals(maNV.Trim()));
            if (nhanVien != null)
                return Ok(new { success = true, data = nhanVien });
            return Ok(new { success = true, message = "Không tồn tại nhân viên này" });
        }

        [HttpPost]
        public ActionResult themNhanVien(NhanVien model)
        {
            var checkPK = context.NhanViens.Where(x => x.MANV == model.MANV.Trim()).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
            }

            context.NhanViens.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, data = model });

            /* var list = new SQLHelper().ExecuteString("SELECT * FROM SanPham");*/


        }

        [HttpPut]
        public async Task<ActionResult> editNhanVien(NhanVien nhanVien)
        {
            if (nhanVien != null)
            {
                context.Entry(nhanVien).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công {nhanVien.TEN}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại {nhanVien.TEN}" });
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> delNhanVien(string maNV) // , IFormFile file
        {
            if (!string.IsNullOrEmpty(maNV))
            {
                var nhanVien = context.NhanViens.FirstOrDefault(x => x.MANV.Trim().Equals(maNV.Trim()));
                if (nhanVien == null)
                    return NotFound();
                context.NhanViens.Remove(nhanVien);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá thành công nhân viên" });
                return Ok(new { success = false, message = $"xoá thất bại nhân viên" });
            }
            return BadRequest();
        }

    }
}
