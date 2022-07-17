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
                var data = context.LoaiSanPhams.ToList();
                return Ok(new { success = true, data = data });
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
                var data = context.LoaiSanPhams.Where(x => x.ISNEW == true).ToList();
                return Ok(new { success = true, data = data });
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
                var data = context.LoaiSanPhams.Where(x => x.ISGOOD == true).ToList();
                return Ok(new { success = true, data = data });
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
        public async Task<ActionResult> themLoaiSanPham(LoaiSanPham model)
        {
                var checkPK = context.LoaiSanPhams.Where(x => x.MALSP == model.MALSP).FirstOrDefault();
                if (checkPK != null)
                {
                    return Ok(new { success = false, message = "Đã tồn tại loại sản phầm này" });
                }
                var checkName= context.LoaiSanPhams.Where(x => x.TENLSP.ToLower().Trim() == model.TENLSP.ToLower().Trim()).FirstOrDefault();
                if (checkName != null)
                {
                    return Ok(new { success = false, message = "Đã tồn tại tên sản phẩm" });
                }
                context.LoaiSanPhams.Add(model);
                context.SaveChanges();
                return Ok(new { success = true, data = model });
        }
        [HttpPut]
        public ActionResult chinhSuaLoaiSanPham(String maLSP, LoaiSanPham model)
        {
            //maLSP la maLSP cũ
                var checkPK = context.LoaiSanPhams.Where(x => x.MALSP == model.MALSP && x.MALSP != maLSP).FirstOrDefault();
                if (checkPK!= null){
                    return Ok(new { success = false, message = "Đã tồn tại mã loại sản phẩm này" });
                }
                var checkName=context.LoaiSanPhams.Where(x=>x.TENLSP.ToLower().Trim()==model.TENLSP.ToLower().Trim()&& x.MALSP!=maLSP).FirstOrDefault();
                if (checkName != null)
                {
                    return Ok(new { success = false, message = "Đã tồn tại tên loại sản phẩm này" });
                }
                var lsp=context.LoaiSanPhams.Where(x=>x.MALSP==maLSP).FirstOrDefault();
                lsp.HARDDRIVE = model.HARDDRIVE;
                lsp.ANHLSP = model.ANHLSP;
                lsp.CPU = model.CPU;
                lsp.TENLSP = model.TENLSP;
                lsp.RAM=model.RAM;
                lsp.MOTA=model.MOTA;
                lsp.OS=model.OS;
                lsp.CARDSCREEN=model.CARDSCREEN;
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
                    var execute = new SQLHelper().ExecuteQuery("sp_Update_PK",param);
                }

                return Ok(new { success = true, data=model});

        }
        [HttpDelete]
        public ActionResult xoaLoaiSanPham(String maLSP)
        {
                var checkSP = context.SanPhams.Where(x => x.MALSP == maLSP.Trim()).FirstOrDefault();
                if (checkSP!= null){
                    return Ok(new { success = false, message = "Đã tồn tại sản phẩm của Loại SP này" });
                }
                var loaiSP= context.LoaiSanPhams.Where(x => x.MALSP == maLSP.Trim()).FirstOrDefault();
                context.Entry(loaiSP).State = EntityState.Deleted;
                context.SaveChanges();
                return Ok(new { success = true, data = loaiSP });

        }

    }
}
