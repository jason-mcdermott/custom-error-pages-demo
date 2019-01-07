using System.Web;
using System.Web.Mvc;

namespace CustomErrorPages.Controllers
{
    public class HomeController : Controller
    {
        [Route("demo")]
        public ActionResult Demo()
        {
            throw new HttpException(503, "Oh no! The service seems to be unavailable");
        }

        [Route("demo/{statusCode:int}")]
        public ActionResult Demo(int statusCode)
        {
            throw new HttpException(statusCode, "Oh no! The service seems to be unavailable");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}