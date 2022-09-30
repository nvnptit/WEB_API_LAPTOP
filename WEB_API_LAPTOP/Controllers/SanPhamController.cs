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
    [Route("api/san-pham")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public SanPhamController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getSanPham(string? SERIAL)
        {
            //Kiểm tra xem có get bằng serial hay không
            //Không thì lấy toàn bộ sản phẩm
            if (string.IsNullOrEmpty(SERIAL))
            {
                var lstSanPhams = context.SanPhams.ToList();
                return Ok(new { success = true, data = lstSanPhams });
            }
            //Lấy thì lấy ra sản phẩm có serial là giá trị cần tìm
            var SanPham = context.SanPhams.FirstOrDefault(x => x.SERIAL.Trim().Equals(SERIAL.Trim()));
            if(SanPham!=null)
                 return Ok(new { success = true, data = SanPham });
            return Ok(new { success = true, message = "Không tồn tại sản phẩm này" });
        }

        [HttpGet]
        [Route("SL-SERI")]
        public ActionResult getLSP_SERI(string? SERIAL)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var data = new SQLHelper(_configuration).ExecuteQuery("SP_GET_SLSERI", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<SLSERI>>(json);
                return Ok(new { success = true, data = dataRet });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, message = "Đã có lỗi xảy ra!" });
            }
        }
        [HttpPost]

        [HttpPost]

        public ActionResult themSanPham(SanPham model)
        {
                var checkPK = context.SanPhams.Where(x => x.SERIAL.ToLower().Trim() == model.SERIAL.ToLower().Trim()).FirstOrDefault();
                if (checkPK != null)
                {
                    return Ok(new { success = false, message = "Đã tồn tại số SERIAL này" });
                }
                
                context.SanPhams.Add(model);
                context.SaveChanges();
                return Ok(new { success = true, data = model });


        }

        [HttpPut]
        public async Task<ActionResult> editSanPham(SanPham sanPham)
        {
            if (sanPham != null)
            {
                context.Entry(sanPham).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công {sanPham.SERIAL}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại {sanPham.SERIAL}" });
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> delSanPham(string serial) // , IFormFile file
        {
            if (!string.IsNullOrEmpty(serial))
            {
                var sanpham = context.SanPhams.FirstOrDefault(x => x.SERIAL.Trim().Equals(serial.Trim()));
                if (sanpham == null)
                    return NotFound();
                context.SanPhams.Remove(sanpham);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá thành công {serial}" });
                return Ok(new { success = false, message = $"xoá thất bại {serial}" });
            }
            return BadRequest();
        }
        [HttpGet]
        [Route("list-serial")]
        public ActionResult getListSerial(string? maLSP)
        {
            //Kiểm tra xem có get bằng serial hay không
            //Không thì lấy toàn bộ sản phẩm
            if (!string.IsNullOrEmpty(maLSP))
            {
                var lstSanPhams = context.SanPhams.Where(x => x.MALSP.ToLower().Trim() == maLSP.ToLower().Trim() && x.IDGIOHANG == null).ToList(); ;
                return Ok(new { success = true, data = lstSanPhams });
            }
            return Ok(new { success = false, message = "Bạn phải truyền mã loại sản phẩm!" });
        }
    }
}
