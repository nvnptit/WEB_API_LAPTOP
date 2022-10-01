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
                var exist = context.DotGiamGias.Where(x => x.MADOTGG == model.MADOTGG).FirstOrDefault();
                if (exist != null)
                {
                    exist.MOTA = model.MOTA;
                    exist.MANV = model.MANV;

                    context.Entry(exist).State = EntityState.Modified;
                    int count = await context.SaveChangesAsync();
                    if (count > 0)
                        return Ok(new { success = true, message = $"Chỉnh sửa thành công" });
                    return Ok(new { success = false, message = $"Chỉnh sửa thất bại" });
                } else
                {
                    return Ok(new { success = false, message = "Đợt giảm giá không tồn tại" });
                }
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

        // CHI TIET DOT GIAM GIA
        [HttpGet]
        [Route("CHI-TIET")]
        public ActionResult getCTDotGG(string? maDot)
        {
            if (maDot != null)
            {
                var lstCTDotGGs = context.CTGiamGias.Where(x => x.MADOTGG == maDot).ToList();
                return Ok(new { success = true, data = lstCTDotGGs });
            }else
            {
                var lstCTDotGGs = context.CTGiamGias.ToList();
                return Ok(new { success = true, data = lstCTDotGGs });
            }

        }

        [HttpPost]
        [Route("CHI-TIET")]
        public ActionResult themCTDotGG(CTGiamGia model)
        {
            var checkPK = context.CTGiamGias.Where(x => x.MADOTGG == model.MADOTGG && x.MALSP == model.MALSP).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại sản phẩm này trong đợt giảm giá" });
            }

            context.CTGiamGias.Add(model);
            context.SaveChanges();
            return Ok(new { success = true, message = "Thêm sản phẩm vào đợt giảm giá thành công!" });

        }

        [HttpDelete]
        [Route("CHI-TIET")]
        public async Task<ActionResult> delCTDotGG(String malsp, String madotgg)
        {
            var ctDotGG = context.CTGiamGias.Where(x => x.MADOTGG == madotgg && x.MALSP == malsp).FirstOrDefault();
            if (ctDotGG == null)
                return Ok(new { success = false, message = "Sản phẩm không nằm trong đợt giảm giá" });
            try
            {
                context.CTGiamGias.Remove(ctDotGG);
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = "Xoá sản phẩm khỏi đợt giảm giá thành công!" });
                return Ok(new { success = false, message = "Đã có lỗi xảy ra khi xoá!" });
            }
            catch (Exception e)
            {
                return Ok(new { success = false, message = "Không thể xoá sản phẩm khỏi đợt giảm giá này" });
            }

        }

        [HttpPut]
        [Route("CHI-TIET")]
        public async Task<ActionResult> editCTDotGG(CTGiamGia model)
        {
            if (model != null)
            {
                var exist = context.CTGiamGias.Where(x => x.MADOTGG == model.MADOTGG && x.MALSP == model.MALSP).FirstOrDefault();
                if (exist != null)
                {
                    exist.PHANTRAMGG = model.PHANTRAMGG;

                    context.Entry(exist).State = EntityState.Modified;
                    int count = await context.SaveChangesAsync();
                    if (count > 0)
                        return Ok(new { success = true, message = $"Chỉnh sửa thành công" });
                    return Ok(new { success = false, message = $"Chỉnh sửa thất bại" });
                }
                else
                {
                    return Ok(new { success = false, message = "Sản phẩm giảm giá không tồn tại" });
                }
            }
            return BadRequest();
        }

    }
}
