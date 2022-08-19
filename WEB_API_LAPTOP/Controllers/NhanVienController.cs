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

        [Route("MANV")]
        [HttpGet]
        public ActionResult getMaNV()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@BANG", "NHANVIEN"));
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

        [HttpGet]
        public ActionResult getNhanVien()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var data = new SQLHelper(_configuration).ExecuteQuery("spGetNhanVien", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<NhanVienView>>(json);
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

        [HttpGet]
        [Route("NV-Duyet")]
        public ActionResult getNhanVienDuyet()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                var data = new SQLHelper(_configuration).ExecuteQuery("get_NV_Duyet", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<NhanVien>>(json);
                return Ok(new { success = true, data = dataRet });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, data = "Đã có lỗi xảy ra!" });
            }
        }


        [HttpGet]
        [Route("NV-Giaohang")]
        public ActionResult getNhanVienGiaoHang()
        {
            try
             {
                List<SqlParameter> param = new List<SqlParameter>();

                var data = new SQLHelper(_configuration).ExecuteQuery("get_NV_GiaoHang", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<NhanVien>>(json);
                return Ok(new { success = true, data = dataRet });
            }
            catch (Exception ex)
            {
                return Ok(new { success = false, data = "Đã có lỗi xảy ra!" });
            }
        }

        [HttpPost]
        public ActionResult themNhanVien(NhanVien model)
        {
            var checkPK = context.NhanViens.Where(x => x.MANV == model.MANV.Trim()).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại mã nhân viên này" });
            }

            var checkSDT = context.NhanViens.Where(x => x.SDT == model.SDT).FirstOrDefault();
            if (checkSDT != null)
            {
                return Ok(new { success = false, message = "Lỗi trùng số điện thoại nhân viên" });
            }

            var checkEmail = context.NhanViens.Where(x => x.EMAIL.ToLower().Trim() == model.EMAIL.ToLower().Trim()).FirstOrDefault();
            if (checkEmail != null)
            {
                return Ok(new { success = false, message = "Lỗi trùng email nhân viên" });
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

                var checkSDT = context.NhanViens.Where(x => x.SDT == nhanVien.SDT && x.MANV != nhanVien.MANV).FirstOrDefault();
                if (checkSDT != null)
                {
                    return Ok(new { success = false, message = "Lỗi trùng số điện thoại nhân viên" });
                }

                var checkEmail = context.NhanViens.Where(x => x.EMAIL.ToLower().Trim() == nhanVien.EMAIL.ToLower().Trim() && x.MANV != nhanVien.MANV).FirstOrDefault();
                if (checkEmail != null)
                {
                    return Ok(new { success = false, message = "Lỗi trùng email nhân viên" });
                }

                context.Entry(nhanVien).State = EntityState.Modified;
                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = $"Chỉnh sửa thành công {nhanVien.TEN}" });
                return Ok(new { success = false, message = $"Chỉnh sửa thất bại {nhanVien.TEN}" });
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> delNhanVien(string? maNV) // , IFormFile file
        {
            if (maNV == null)
            {
                return Ok(new { success = false, message = "Mã nhân viên không được để trống" });
            }
                var nhanVien = context.NhanViens.FirstOrDefault(x => x.MANV.Trim().Equals(maNV.Trim()));
                if (nhanVien == null)
                    return Ok(new { success = false, message = "Mã nhân viên không tồn tại" });
                try
                {
                    context.NhanViens.Remove(nhanVien);

                    int count = await context.SaveChangesAsync();
                    if (count > 0)
                        return Ok(new { success = true, message = "xoá thành công nhân viên" });
                    return Ok(new { success = false, message = "Đã có lỗi xảy ra khi xoá!" });
                }
                catch (Exception e)
                {
                    return Ok(new { success = false, message = "Không thể xoá nhân viên này" });
                }
            return Ok(new { success = false, message = "Không thể xoá nhân viên này" });
        }

    }
}
