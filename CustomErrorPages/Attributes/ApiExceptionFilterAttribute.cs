using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CustomErrorPages.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ApiExceptionFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            try
            {
                if (!filterContext.ExceptionHandled)
                {
                    HttpException httpException = filterContext.Exception as HttpException;

                    if (httpException == null)
                    {
                        httpException = new HttpException((int)HttpStatusCode.InternalServerError, "Server Error");
                    }
                    else if (httpException.GetHttpCode() == (int)HttpStatusCode.Unauthorized)
                    {
                        // NOTE: this *used* to be an issue, though it *appears* to be fixed.

                        // MVC intercepts 401's and returns 200...so use 403 
                        //httpException = new HttpException((int)HttpStatusCode.Forbidden, "Your session has timed out. Please log back in to continue.");
                    }

                    filterContext.Result = new JsonResult
                    {
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        Data = new { success = false, errorMessage = httpException.Message }
                    };

                    // ensure Response has correct StatusCode
                    filterContext.HttpContext.Response.StatusCode = httpException.GetHttpCode();
                    filterContext.ExceptionHandled = true;
                    // Avoid IIS7 getting in the middle
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                }
            }
            // swallow any exception at this point so original exception's StatusCode is passed to Response
            catch (Exception) { }
        }
    }

}