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
            if (string.IsNullOrEmpty(cmnd))
            {
                var lstKhachHangs = context.KhachHangs.ToList();
                return Ok(new { success = true, data = lstKhachHangs });
            }
            var khachHang = context.KhachHangs.FirstOrDefault(x => x.CMND.Trim().Equals(cmnd.Trim()));
            if (khachHang != null)
                return Ok(new { success = true, data = khachHang });
            return Ok(new { success = false, message = "Không tồn tại nhân viên này" });
        }

        [HttpPost]
        public ActionResult themKhachHang(KhachHang model)
        {
            var checkPK = context.KhachHangs.Where(x => x.CMND == model.CMND.Trim()).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại số chứng minh nhân dân này" });
            }

            var checkSDT = context.KhachHangs.Where(x => x.SDT == model.SDT ).FirstOrDefault();
            if (checkSDT != null)
            {
                return Ok(new { success = false, message = "Lỗi trùng số điện thoại khách hàng" });
            }

            var checkEmail = context.KhachHangs.Where(x => x.EMAIL.ToLower().Trim() == model.EMAIL.ToLower().Trim()).FirstOrDefault();
            if (checkEmail != null)
            {
                return Ok(new { success = false, message = "Lỗi trùng email khách hàng" });
            }

            context.KhachHangs.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, data = model });

        }

        [HttpPut]
        public async Task<ActionResult> editKhachHang(KhachHangEdit khachHang)
        {

            if (khachHang != null)
            {

                var checkSDT = context.KhachHangs.Where(x => x.SDT == khachHang.SDT && x.CMND != khachHang.CMND).FirstOrDefault();
                if (checkSDT != null)
                {
                    return Ok(new { success = false, message = "Lỗi trùng số điện thoại khách hàng" });
                }

                var checkEmail = context.KhachHangs.Where(x => x.EMAIL.ToLower().Trim() == khachHang.EMAIL.ToLower().Trim() && x.CMND != khachHang.CMND).FirstOrDefault();
                if (checkEmail != null)
                {
                    return Ok(new { success = false, message = "Lỗi trùng email khách hàng" });
                }

                var exist = context.KhachHangs.Where(x => x.CMND == khachHang.CMND).FirstOrDefault();

                exist.EMAIL = khachHang.EMAIL;
                exist.TEN = khachHang.TEN;
                exist.DIACHI = khachHang.DIACHI;
                exist.NGAYSINH = khachHang.NGAYSINH;
                exist.SDT = khachHang.SDT;

                context.Entry(exist).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại" });
            }
            return Ok(new { success = false, message = "Đã có lỗi xảy ra" });
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
