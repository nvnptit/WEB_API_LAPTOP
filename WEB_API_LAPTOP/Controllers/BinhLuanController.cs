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
    [Route("api/binh-luan")]
    [ApiController]
    public class BinhLuanController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public BinhLuanController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getBinhLuan(String? seri)
        {
            if (seri == null)
            {
                var lstBinhLuans = context.BinhLuans.ToList();
                return Ok(new { success = true, data = lstBinhLuans });
            } else
            {
                var data = context.BinhLuans.Where(x => x.SERIAL == seri).FirstOrDefault();
                if (data != null)
                {
                    return Ok(new { success = false, message = "Bạn đã đánh giá "+data.DIEM+"sao\nBình luận: " + data.MOTA }); ;
                }
            }
            return BadRequest();
        }

        [HttpPost]
        public ActionResult AddComment(BinhLuan model)
        {
            var checkPK = context.BinhLuans.Where(x => x.CMND == model.CMND && x.SERIAL == model.SERIAL).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại bình luận" });
            }
            var serial = context.SanPhams.Where(x => x.SERIAL == model.SERIAL).FirstOrDefault();
            if (serial == null)
            {
                return Ok(new { success = false, message = "Sản phẩm không tồn tại" });
            }
            var user = context.KhachHangs.Where(x => x.CMND == model.CMND).FirstOrDefault();
            if (user == null)
            {
                return Ok(new { success = false, message = "Khách hàng không tồn tại" });
            }

            context.BinhLuans.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, message = "Đánh giá thành công!" });
        }

        [HttpPut]
        public async Task<ActionResult> editComment(BinhLuan model)
        {
            if (model != null)
            {
                var exist = context.BinhLuans.Where(x => x.CMND == model.CMND && x.SERIAL == model.SERIAL).FirstOrDefault();
                if (exist != null)
                {
                    exist.DIEM = model.DIEM;
                    exist.MOTA = model.MOTA;

                    context.Entry(exist).State = EntityState.Modified;
                    int count = await context.SaveChangesAsync();
                    if (count > 0)
                        return Ok(new { success = true, message = $"Chỉnh sửa thành công" });
                    return Ok(new { success = false, message = $"Chỉnh sửa thất bại" });
                }
                else
                {
                    return Ok(new { success = false, message = "Bình luận không tồn tại" });
                }
            }
            return BadRequest();
        }

    }
}
