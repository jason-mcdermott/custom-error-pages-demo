using System.Web;
using System.Web.Mvc;
using CustomErrorPages.Attributes;

namespace CustomErrorPages.Controllers
{
    [ApiExceptionFilter]
    public class ApiController : Controller
    {
        // this call will succeed
        [Route("api/get/data")]
        public JsonResult GetData()
        {
            var message =  new { data = new string[] { "This is some dummy data from an API call" }};
            
            return Json(new { status = "success", message }, JsonRequestBehavior.AllowGet);
        }

        //
        [Route("api/get/400")]
        public JsonResult TryGetDataThrows400Error()
        {
            var message = new { data = new string[] { "This is some dummy data from an API call" } };

            throw new HttpException(400, "Oh no! Bad Request");
        }

        [Route("api/get/503")]
        public JsonResult TryGetDataThrows503Error()
        {
            var message = new { data = new string[] { "This is some dummy data from an API call" } };

            throw new HttpException(503, "Oh no! Service is unavailable");
        }

        [Route("api/get/401")]
        public JsonResult TryGetDataThrows401Error()
        {
            var message = new { data = new string[] { "This is some dummy data from an API call" } };

            throw new HttpException(401, "Oh no! Unauthorized");
        }
    }
}