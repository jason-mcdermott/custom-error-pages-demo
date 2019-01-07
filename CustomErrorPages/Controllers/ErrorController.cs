using System.Net;
using System.Web.Mvc;
using CustomErrorPages.Attributes;

namespace CustomErrorPages.Controllers
{
    [RoutePrefix("errors")]
    public class ErrorController : Controller
    {
        public ActionResult ServerError()
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            ViewData["HttpStatusCode"] = HttpStatusCode.InternalServerError;
            return View("ServerError");
        }

        [PreventDirectAccess]
        public ActionResult Unauthorized()
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            ViewData["HttpStatusCode"] = HttpStatusCode.Unauthorized;
            return View("Unauthorized");
        }

        [PreventDirectAccess]
        public ActionResult AccessDenied()
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            ViewData["HttpStatusCode"] = HttpStatusCode.Forbidden;
            return View("Forbidden");
        }

        [PreventDirectAccess]
        public ActionResult NotFound()
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            ViewData["HttpStatusCode"] = HttpStatusCode.NotFound;
            return View("NotFound");
        }

        [PreventDirectAccess]
        public ActionResult ServiceUnavailable()
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            ViewData["HttpStatusCode"] = HttpStatusCode.ServiceUnavailable;
            return View("ServiceUnavailable");
        }

        [PreventDirectAccess]
        public ActionResult OtherHttpStatusCode(int httpStatusCode)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            ViewData["HttpStatusCode"] = httpStatusCode;
            return View("GenericHttpError");
        }
    }
}