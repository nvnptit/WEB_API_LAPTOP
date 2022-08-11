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
    [Route("api/ty-gia")]
    [ApiController]
    public class TyGiaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public TyGiaController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getTyGia(String? tyGia)
        {
            //Kiểm tra xem có get maQuyen
            //Không thì lấy toàn bộ 
            if (tyGia == null)
            {
                var lstTyGias = context.TyGias.ToList();
                return Ok(new { success = true, data = lstTyGias });
            }
            //Lấy thì lấy ra giỏ hàng có idGioHang là giá trị cần tìm
            var gia = context.TyGias.FirstOrDefault(x => x.MATG.Equals(tyGia));
            if (gia != null)
                return Ok(new { success = true, data = gia });
            return Ok(new { success = false, message = "Không tồn tại tỷ giá" });
        }
       /* [HttpPost]
        public ActionResult themQuyen(Quyen model)
        {
            var checkPK = context.Quyens.Where(x => x.MAQUYEN == model.MAQUYEN).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
            }
            context.Quyens.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, data = model });

        }

        [HttpPut]
        public async Task<ActionResult> editQuyen(Quyen model)
        {
            if (model != null)
            {
                context.Entry(model).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công {model.TENQUYEN}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại {model.TENQUYEN}" });
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> delQuyen(int maQuyen)
        {
            if (maQuyen != null)
            {
                var quyen = context.Quyens.FirstOrDefault(x => x.MAQUYEN.Equals(maQuyen));
                if (quyen == null)
                    return NotFound();
                context.Quyens.Remove(quyen);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"xoá quyền thành công" });
                return Ok(new { success = false, message = $"xoá quyền thất bại" });
            }
            return BadRequest();
        }*/

    }
}
