using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.RegularExpressions;
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

        [Route("MALSP")]
        [HttpGet]
        public ActionResult getMaLSP()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@BANG", "LOAISANPHAM"));
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
        public ActionResult getFullLoaiSanPham()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var data = new SQLHelper(_configuration).ExecuteQuery("sp_Get_LoaiSPFullV2", param);
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
               
                var data = new SQLHelper(_configuration).ExecuteQuery("sp_Get_LoaiSPNewV2", param);
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
        [Route("get-good-lsp")]
        public ActionResult getLoaiSanPhamGoodDeal()
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                var data = new SQLHelper(_configuration).ExecuteQuery("sp_Get_LoaiSPGoodV2", param);
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
        [Route("get-good-lsp-hang")]
        public ActionResult getLoaiSanPhamGoodDealByHangSX(int maHang)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@maHang", maHang));
                var data = new SQLHelper(_configuration).ExecuteQuery("sp_Get_LoaiSPGood_ByHangSXV2", param);
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

        public static String chuanHoa(String s)
        {
            if (s != null)
            {
                s = s.Trim();
                Regex regex = new Regex("\\s+");
                String kq = regex.Replace(s, " ");
                return kq;
            }
            return "";
        }

        [HttpGet]
        [Route("search")]
        public ActionResult search(String? tenLSP,  int? priceMin,long? priceMax, int? maHang)
        {
            
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@tenLSP", chuanHoa(tenLSP)));
                /*param.Add(new SqlParameter("@tenLSP", tenLSP));*/
                param.Add(new SqlParameter("@maHang", maHang));
                param.Add(new SqlParameter("@priceMin", priceMin));
                param.Add(new SqlParameter("@priceMax", priceMax));
                var data = new SQLHelper(_configuration).ExecuteQuery("TimKiemV2", param);
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
        [Route("uploadPicture")]
        public IActionResult upload(IFormFile? fileUpload)
        {
            // xử lý upload hình
            var url = "";
            var file = fileUpload;
            if (file != null)
            {
                String FileName = Guid.NewGuid().ToString() + file.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
               url = "/images/" + FileName;
                return Ok(new { success = true, message = url });
            }
            else
            {
                url = "/images/noimage.png";
                return Ok(new { success = true, message = url });
            }
                return Ok(new { success = false, message = "Tải ảnh thất bại" });
        }

        [HttpPost]
        public IActionResult themLoaiSanPham(LoaiSanPhamAddModel model) //[FromForm] LoaiSanPhamAddModel model
        {

            var checkTenLSP = context.LoaiSanPhams.Where(x => x.TENLSP.ToLower().Trim() == model.TENLSP.ToLower().Trim()).FirstOrDefault();
            if (checkTenLSP != null)
            {
                return Ok(new { success = false, message = "Lỗi đã tồn tại tên sản phẩm này" });
            }

            LoaiSanPham modelAdd = new LoaiSanPham();
  
            modelAdd.MALSP = model.MALSP.Trim();

            modelAdd.TENLSP = model.TENLSP.Trim();
            modelAdd.SOLUONG = model.SOLUONG;

            modelAdd.MOTA = model.MOTA.Trim();
            modelAdd.CPU = model.CPU.Trim();
            modelAdd.RAM = model.RAM.Trim();
            modelAdd.HARDDRIVE = model.HARDDRIVE.Trim();
            modelAdd.CARDSCREEN = model.CARDSCREEN.Trim();
            modelAdd.OS = model.OS.Trim();
            modelAdd.MAHANG = model.MAHANG;
            modelAdd.ISNEW = model.ISNEW;
            modelAdd.ISGOOD = model.ISGOOD;

            try
            {
                context.LoaiSanPhams.Add(modelAdd);
                context.SaveChanges();
                // Xử lý giá
                GiaThayDoi giaThayDoi = new GiaThayDoi();
                giaThayDoi.GIAMOI = model.GIAMOI;
                giaThayDoi.NGAYAPDUNG = DateTime.Now;
                giaThayDoi.MALSP = model.MALSP;
                giaThayDoi.MANV = model.MANV;
                context.GiaThayDois.Add(giaThayDoi);
                context.SaveChanges();
                return Ok(new { success = true, message = "Thêm loại sản phẩm thành công" });
            }
            catch (Exception e)
            {
                return Ok(new { success = false, message = "Thêm loại sản phẩm thất bại" });
            }
           
        }

        [HttpPut]
        public ActionResult chinhSuaLoaiSanPham(LoaiSanPham model)
        {
            //maLSP la maLSP cũ
/*            var checkPK = context.LoaiSanPhams.Where(x => x.MALSP == model.MALSP && x.MALSP != maLSP.Trim()).FirstOrDefault();
            if (checkPK != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại mã loại sản phẩm này" });
            }*/
            var checkName = context.LoaiSanPhams.Where(x => x.TENLSP.ToLower().Trim() == model.TENLSP.ToLower().Trim() && x.MALSP != model.MALSP.Trim()).FirstOrDefault();
            if (checkName != null)
            {
                return Ok(new { success = false, message = "Đã tồn tại tên loại sản phẩm này" });
            }
            var lsp = context.LoaiSanPhams.Where(x => x.MALSP == model.MALSP.Trim()).FirstOrDefault();
            lsp.HARDDRIVE = model.HARDDRIVE.Trim();
            lsp.ANHLSP = model.ANHLSP.Trim();
            lsp.CPU = model.CPU.Trim();
            lsp.TENLSP = model.TENLSP.Trim();
            lsp.RAM = model.RAM.Trim();
            lsp.MOTA = model.MOTA.Trim();
            lsp.OS = model.OS.Trim();
            lsp.CARDSCREEN = model.CARDSCREEN.Trim();
            lsp.MAHANG = model.MAHANG;
            lsp.SOLUONG = model.SOLUONG;
            context.Entry(lsp).State = EntityState.Modified;
            /*        if (maLSP != model.MALSP.Trim())
                    {
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(new SqlParameter("@pk", maLSP));
                        param.Add(new SqlParameter("@pk_Update", model.MALSP));
                        param.Add(new SqlParameter("@table_Name", "LOAISANPHAM"));
                        var execute = new SQLHelper(_configuration).ExecuteQuery("sp_Update_PK_LoaiSP", param);
                    }*/

            int count = context.SaveChanges(); 
            if (count > 0)
                return Ok(new { success = true, message = $"Chỉnh sửa thành công " });
            return Ok(new { success = false, message = $"Chỉnh sửa thất bại" });

        }
        [HttpDelete]
        public async Task<ActionResult> xoaLoaiSanPham(String? maLSP)
        {
            if (maLSP == null)
            {
                return Ok(new { success = false, message = "Mã loại sản phẩm không được để trống" });
            }

            var checkLSP = context.SanPhams.Where(x => x.MALSP == maLSP.Trim()).FirstOrDefault();
            if (checkLSP == null)
                return Ok(new { success = false, message = "Mã loại sản phẩm không tồn tại" });
            try
            {
                context.SanPhams.Remove(checkLSP);

                int count = await context.SaveChangesAsync();
                if (count > 0)
                    return Ok(new { success = true, message = "Xoá loại sản phẩm thành công!" });
                return Ok(new { success = false, message = "Đã có lỗi xảy ra khi xoá!" });
            }
            catch (Exception e)
            {
                return Ok(new { success = false, message = "Không thể xoá loại sản phẩm này" });
            }

        }

    }
}
