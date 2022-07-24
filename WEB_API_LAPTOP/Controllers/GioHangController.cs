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
    [Route("api/gio-hang")]
    [ApiController]
    public class GioHangController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public GioHangController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getGioHang(int? idGioHang)
        {
            //Kiểm tra xem có get bằng idGioHang hay không
            //Không thì lấy toàn bộ 
            if (idGioHang == null)
            {
                var lstGioHangs = context.GioHangs.ToList();
                return Ok(new { success = true, data = lstGioHangs });
            }
            //Lấy thì lấy ra giỏ hàng có idGioHang là giá trị cần tìm
            var gioHang = context.GioHangs.FirstOrDefault(x => x.IDGIOHANG.Equals(idGioHang));
            if (gioHang != null)
                return Ok(new { success = true, data = gioHang });
            return Ok(new { success = true, message = "Không tồn tại giỏ hàng này" });
        }

        [HttpGet]
        [Route("gio-hang-byKH")]
        public ActionResult getGioHangbyKH(String cmnd)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@cmnd", cmnd));
                var data = new SQLHelper(_configuration).ExecuteQuery("sp_Get_GIOHANG_BYKHV2", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<GioHangViewModel>>(json);
                return Ok(new { success = true, data = dataRet });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    message = ex.InnerException
                })
                { StatusCode = StatusCodes.Status403Forbidden };
            }
        }


        [HttpPost]
        public ActionResult themGioHang(GioHangAdd model1)
        {
            GioHang model = new GioHang();
            model.IDGIOHANG = model1.IDGIOHANG;
            model.NGAYLAPGIOHANG = DateTime.Now;
            model.TONGGIATRI = model1.TONGGIATRI;
            model.MATRANGTHAI = model1.MATRANGTHAI;
            model.CMND = model1.CMND;
            model.MANVGIAO = model1.MANVGIAO;
            model.NGUOINHAN = model1.NGUOINHAN;
            model.DIACHI = model1.DIACHI;
            model.SDT = model1.SDT;
            model.EMAIL = model1.EMAIL;

            String maLSP = model1.MALSP;
            var checkPK = context.GioHangs.Where(x => x.IDGIOHANG == model.IDGIOHANG).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại khoá chính" });
            }
            var checkCMND = context.KhachHangs.Where(x => x.CMND == model.CMND).FirstOrDefault();
            if (checkCMND == null)
            {
                return Ok(new { success = false, message = "Người dùng không tồn tại" });
            }
            context.GioHangs.Add(model);
            context.SaveChanges();

            // Lấy ra serial + cập nhật id giỏ hàng và loại sản phẩm vào
            Console.WriteLine(model.IDGIOHANG);
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@maLSP", maLSP));
                param.Add(new SqlParameter("@ID_GIO_HANG", model.IDGIOHANG));
              //  var data = new SQLHelper(_configuration).ExecuteQuery("sp_Update_MuaSP", param);
                int kq = new SQLHelper(_configuration).ExecuteNoneQuery("sp_Update_MuaSP", param);
                if (kq != 1)
                {
                    var gioHangold = context.GioHangs.FirstOrDefault(x => x.IDGIOHANG.Equals(model.IDGIOHANG));
                    if (gioHangold != null)
                    {
                        context.GioHangs.Remove(gioHangold);
                        context.SaveChanges();
                    }
                    return Ok(new { success = false, message = "Thêm giỏ hàng thất bại" });

                }else
                {
                    return Ok(new { success = true, message = "Thành công" });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    message = ex.InnerException
                })
                { StatusCode = StatusCodes.Status403Forbidden };
            }

            return Ok(new { success = true, data = model });

        }

        [HttpPut]
        public async Task<ActionResult> editGioHang(GioHangEditModel gioHang)
        {
            if (gioHang != null)
            {

                var exist = context.GioHangs.Where(x => x.IDGIOHANG == gioHang.IDGIOHANG).FirstOrDefault();
                exist.MANVDUYET = gioHang.MANVDUYET;
                exist.MANVGIAO=gioHang.MANVGIAO;

                context.Entry(exist).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công {gioHang.IDGIOHANG}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại {gioHang.IDGIOHANG}" });
            }
            return BadRequest();
        }
        [HttpDelete]
        public async Task<ActionResult> delGioHang(int idGioHang)
        {
            if (idGioHang != null)
            {
                var gioHang = context.GioHangs.FirstOrDefault(x => x.IDGIOHANG.Equals(idGioHang));
                if (gioHang == null)
                {
                    return Ok(new { success = false, message = "Giỏ hàng không tồn tại" });
                }
                if (gioHang.MATRANGTHAI== -1)
                {
                    try
                    {
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(new SqlParameter("@ID_GIO_HANG", idGioHang));
                        var data = new SQLHelper(_configuration).ExecuteQuery("sp_Delete_MuaSP", param);
                        return Ok(new { success = true, message = "Xoá giỏ hàng thành công" });
                    }
                    catch (Exception ex)
                    {
                        return new JsonResult(new
                        {
                            message = ex.InnerException
                        })
                        { StatusCode = StatusCodes.Status403Forbidden };
                    }
                }
                return Ok(new { success = true, message = $"Không thể xoá hoá đơn này" });
            }
            return BadRequest();
        }

    }
}
