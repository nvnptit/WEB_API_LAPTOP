using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WEB_API_LAPTOP.Controllers
{
    [Route("api/loai-san-pham")]
    [ApiController]
    public class LoaiSanPhamController : ControllerBase
    {
        [HttpGet]
        public ActionResult getLoaiSanPham()
        {
            using(var context=new BanLaptopEntities())
            {
                var list = context.LoaiSanPhams.ToList();
                return Ok(new { success = true, data = list });
            }
        }
    }
}
