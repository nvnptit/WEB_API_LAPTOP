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
    [Route("api/loai-san-pham")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly BanLaptopEntities context;
        public LoaiSanPhamController(IConfiguration configuration, BanLaptopEntities _context)
        {
            _configuration = configuration;
            this.context = _context;
        }

        [HttpGet]
        public ActionResult getFullLoaiSanPham()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var data = new SQLHelper(_configuration).ExecuteQuery("sp_Get_LoaiSPFull", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<LoaiSanPhamViewModel>>(json);
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
        [Route("get-new-lsp")]
        public ActionResult getLoaiSanPhamNew()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var data = new SQLHelper(_configuration).ExecuteQuery("sp_Get_LoaiSPNew", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<LoaiSanPhamViewModel>>(json);
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
        [Route("get-km-lsp")]
        public ActionResult getLoaiSanPhamKM()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var data = new SQLHelper(_configuration).ExecuteQuery("sp_Get_LoaiSPKhuyenMai", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<LoaiSanPhamViewKhuyenMaiModel>>(json);
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
        [Route("get-good-lsp")]
        public ActionResult getLoaiSanPhamGoodDeal()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var data = new SQLHelper(_configuration).ExecuteQuery("sp_Get_LoaiSPGood", param);
                var json = JsonConvert.SerializeObject(data);
                var dataRet = JsonConvert.DeserializeObject<List<LoaiSanPhamViewModel>>(json);
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
        public IActionResult themLoaiSanPham([FromForm]LoaiSanPhamAddModel model)
        {
            LoaiSanPham modelAdd = new LoaiSanPham();
  
            modelAdd.MALSP = model.MALSP;

            modelAdd.TENLSP = model.TENLSP;
            modelAdd.SOLUONG = model.SOLUONG;
            // xử lý upload hình
            var file = model.ANHLSP;
            if (file != null)
            {
                String FileName = Guid.NewGuid().ToString() + file.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                modelAdd.ANHLSP = "/images/" + FileName;
            }
            else
            {
                modelAdd.ANHLSP = "/images/noimage.png";
            }

            modelAdd.MOTA = model.MOTA;
            modelAdd.CPU = model.CPU;
            modelAdd.RAM = model.RAM;
            modelAdd.HARDDRIVE = model.HARDDRIVE;
            modelAdd.CARDSCREEN = model.CARDSCREEN;
            modelAdd.OS = model.OS;
            modelAdd.MAHANG = model.MAHANG;
            modelAdd.ISNEW = model.ISNEW;
            modelAdd.ISGOOD = model.ISGOOD;


            var checkPK = context.LoaiSanPhams.Where(x => x.MALSP == model.MALSP).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại loại sản phầm này" });
            }
            var checkName = context.LoaiSanPhams.Where(x => x.TENLSP.ToLower().Trim() == model.TENLSP.ToLower().Trim()).FirstOrDefault();
            if (checkName != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại tên sản phẩm" });
            }
            context.LoaiSanPhams.Add(modelAdd);
            context.SaveChanges();
            // Xử lý giá
            GiaThayDoi giaThayDoi = new GiaThayDoi();
            giaThayDoi.GIAMOI = model.GIAMOI;
            giaThayDoi.NGAYAPDUNG = DateTime.Now;
            giaThayDoi.MALSP = model.MALSP;
            giaThayDoi.MANV = "NV2";
            
            context.GiayThayDois.Add(giaThayDoi);

            context.SaveChanges();
            return Ok(new { success = true, data = modelAdd });
        }
        [HttpPut]
        public ActionResult chinhSuaLoaiSanPham(String maLSP, LoaiSanPham model)
        {
            //maLSP la maLSP cũ
            var checkPK = context.LoaiSanPhams.Where(x => x.MALSP == model.MALSP && x.MALSP != maLSP).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại mã loại sản phẩm này" });
            }
            var checkName = context.LoaiSanPhams.Where(x => x.TENLSP.ToLower().Trim() == model.TENLSP.ToLower().Trim() && x.MALSP != maLSP).FirstOrDefault();
            if (checkName != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại tên loại sản phẩm này" });
            }
            var lsp = context.LoaiSanPhams.Where(x => x.MALSP == maLSP).FirstOrDefault();
            lsp.HARDDRIVE = model.HARDDRIVE;
            lsp.ANHLSP = model.ANHLSP;
            lsp.CPU = model.CPU;
            lsp.TENLSP = model.TENLSP;
            lsp.RAM = model.RAM;
            lsp.MOTA = model.MOTA;
            lsp.OS = model.OS;
            lsp.CARDSCREEN = model.CARDSCREEN;
            lsp.MAHANG = model.MAHANG;
            lsp.SOLUONG = model.SOLUONG;
            context.Entry(lsp).State = EntityState.Modified;
            context.SaveChanges();
            if (maLSP != model.MALSP.Trim())
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@pk", maLSP));
                param.Add(new SqlParameter("@pk_Update", model.MALSP));
                param.Add(new SqlParameter("@table_Name", "LOAISANPHAM"));
                var execute = new SQLHelper(_configuration).ExecuteQuery("sp_Update_PK", param);
            }

            return Ok(new { success = true, data = model });

        }
        [HttpDelete]
        public ActionResult xoaLoaiSanPham(String maLSP)
        {
            var checkSP = context.SanPhams.Where(x => x.MALSP == maLSP.Trim()).FirstOrDefault();
            if (checkSP != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại sản phẩm của Loại SP này" });
            }
            var loaiSP = context.LoaiSanPhams.Where(x => x.MALSP == maLSP.Trim()).FirstOrDefault();
            context.Entry(loaiSP).State = EntityState.Deleted;
            context.SaveChanges();
            return Ok(new { success = true, data = loaiSP });

        }

    }
}
