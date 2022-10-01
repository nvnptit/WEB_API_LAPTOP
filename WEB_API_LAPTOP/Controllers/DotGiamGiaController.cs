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
    [Route("api/dot-gg")]
    [ApiController]
    public class DotGiamGiaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public DotGiamGiaController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getDotGG()
        {
            var lstDotGGs = context.DotGiamGias.ToList();
            return Ok(new { success = true, data = lstDotGGs });
        }
        [HttpPost]
        public ActionResult themDotGG(DotGiamGia model)
        {
            var checkPK = context.DotGiamGias.Where(x => x.MADOTGG == model.MADOTGG).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
            }

            var dt = context.DotGiamGias.OrderByDescending(x => x.NGAYKETTHUC).FirstOrDefault();
            if (dt.NGAYKETTHUC >= model.NGAYBATDAU)
            {
                return Ok(new { success = false, message = "Đã có đợt giảm giá khác trong thời gian này" });
            }
            if (model.NGAYKETTHUC <= model.NGAYBATDAU)
            {
                return Ok(new { success = false, message = "Ngày kết thúc phải lớn hơn ngày bắt đầu" });
            }
            context.DotGiamGias.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, message = "Thêm đợt giảm giá thành công!" });

        }

        [Route("MADGG")]
        [HttpGet]
        public ActionResult getMaLSP()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@BANG", "DOTGIAMGIA"));
                var data = new SQLHelper(_configuration).ExecuteQuery("LAY_MA", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<MaSo>>(json);
                return Ok(new { success = true, data = dataRet });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, data = "Đã có lỗi xảy ra!" });
            }
        }


        [HttpPut]
        public async Task<ActionResult> editDotGG(DotGiamGia model)
        {
            if (model != null)
            {
                var dt = context.DotGiamGias.OrderByDescending(x => x.NGAYKETTHUC).FirstOrDefault();
                if (dt.NGAYKETTHUC >= model.NGAYBATDAU)
                {
                    return Ok(new { success = false, message = "Đã có đợt giảm giá khác trong thời gian này" });
                }

                if (model.NGAYKETTHUC <= model.NGAYBATDAU)
                {
                    return Ok(new { success = false, message = "Ngày kết thúc phải lớn hơn ngày bắt đầu" });
                }
                var exist = context.DotGiamGias.Where(x => x.MADOTGG == model.MADOTGG).FirstOrDefault();
                exist.NGAYBATDAU = model.NGAYBATDAU;
                exist.NGAYKETTHUC = model.NGAYKETTHUC;
                exist.MOTA = model.MOTA;
                exist.MANV = model.MANV;

                context.Entry(model).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại" });
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> delHang(String? maDotGG) // , IFormFile file
        {
            if (maDotGG == null)
            {
                return Ok(new { success = false, message = "Mã đợt giảm giá không được để trống" });
            }
            var dotGG = context.DotGiamGias.FirstOrDefault(x => x.MADOTGG.Equals(maDotGG));
            if (dotGG == null)
                return Ok(new { success = false, message = "Mã đợt giảm giá không tồn tại" });
            try
            {
                context.DotGiamGias.Remove(dotGG);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = "Xoá đợt giảm giá thành công!" });
                return Ok(new { success = false, message = "Đã có lỗi xảy ra khi xoá!" });
            }
            catch (Exception e)
            {
                return Ok(new { success = false, message = "Không thể xoá đợt giảm giá này" });
            }
        }
    }
}
