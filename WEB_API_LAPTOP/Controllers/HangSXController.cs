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
            return Ok(new { success = true, message = "Không tồn tại hãng này" });
        }
        [HttpPost]
        public ActionResult themHang(HangSX model)
        {
            var checkPK = context.HangSXs.Where(x => x.MAHANG == model.MAHANG).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
            }

            var checkName = context.HangSXs.Where(x => x.TENHANG.ToLower().Trim() == model.TENHANG.ToLower().Trim()).FirstOrDefault();
            if (checkName != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại tên hãng này" });
            }

            var checkEmail = context.HangSXs.Where(x => x.EMAIL.ToLower().Trim() == model.EMAIL.ToLower().Trim()).FirstOrDefault();
            if (checkEmail != null)
            {
                return Ok(new { success = false, message = "Lỗi trùng email hãng khác" });
            }

            var checkSDT = context.HangSXs.Where(x => x.SDT == model.SDT).FirstOrDefault();
            if (checkSDT != null)
            {
                return Ok(new { success = false, message = "Lỗi trùng số điện thoại hãng khác" });
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
                var checkName = context.HangSXs.Where(x => x.TENHANG.ToLower().Trim() == hangSX.TENHANG.ToLower().Trim() && x.MAHANG != hangSX.MAHANG).FirstOrDefault();
                if (checkName != null)
                {
                    return Ok(new { success = false, message = "Đã tồn tại tên hãng này" });
                }

                var checkEmail = context.HangSXs.Where(x => x.EMAIL.ToLower().Trim() == hangSX.EMAIL.ToLower().Trim() && x.MAHANG != hangSX.MAHANG).FirstOrDefault();
                if (checkEmail != null)
                {
                    return Ok(new { success = false, message = "Lỗi trùng email hãng khác" });
                }

                var checkSDT = context.HangSXs.Where(x => x.SDT == hangSX.SDT && x.MAHANG != hangSX.MAHANG).FirstOrDefault();
                if (checkSDT != null)
                {
                    return Ok(new { success = false, message = "Lỗi trùng số điện thoại hãng khác" });
                }



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
            if (maHang == null)
            {
                   return Ok(new { success = false, message = "Mã hãng sản xuất không được để trống" });
            }
            var hangSX = context.HangSXs.FirstOrDefault(x => x.MAHANG.Equals(maHang));
                if (hangSX == null)
                return Ok(new { success = false, message = "Mã hãng sản xuất không tồn tại" });
            try
                {
                    context.HangSXs.Remove(hangSX);

                    int count = await context.SaveChangesAsync();
                    if (count > 0)
                        return Ok(new { success = true, message = "Xoá hãng thành công!" });
                    return Ok(new { success = false, message = "Đã có lỗi xảy ra khi xoá!" });
                }
                catch (Exception e) {
                    return Ok(new { success = false, message = "Không thể xoá hãng sản xuất này" });
                }
        }

    }
}
