using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CustomErrorPages.Controllers;

namespace CustomErrorPages
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error()
        {
            if (Context.IsCustomErrorEnabled)
            {
                GenerateCustomErrorPage(Server.GetLastError());
            }   
        }

        // used these sources for this error page strategy:
        // http://www.digitallycreated.net/Blog/57/getting-the-correct-http-status-codes-out-of-asp.net-custom-error-pages
        // http://stackoverflow.com/questions/15384148/iis-serves-custom-error-page-as-plain-text-no-content-type-header
        private void GenerateCustomErrorPage(Exception exception)
        {
            // reset and cleanup
            Response.Clear();
            Server.ClearError();
            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;
            // Ensure content-type header is present
            Response.Headers.Add("Content-Type", "text/html");

            HttpException httpException = exception as HttpException;

            if (exception is HttpException)
            {
                HandleHttpException(httpException);
                return;
            }

            HandleNonHttpException(exception);
        }

        private void HandleNonHttpException(Exception exception)
        {
            HttpContext.Current.Response.Redirect("~/Errors/ServerError");

            // Directs the thread to finish, bypassing additional processing
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void HandleHttpException(HttpException httpException)
        {
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode())
            {
                case 401:
                    routeData.Values.Add("action", "Unauthorized");
                    break;
                case 403:
                    routeData.Values.Add("action", "AccessDenied");
                    break;
                case 404:
                    routeData.Values.Add("action", "NotFound");
                    break;
                case 500:
                    routeData.Values.Add("action", "ServerError");
                    break;
                case 503:
                    routeData.Values.Add("action", "ServiceUnavailable");
                    break;
                default:
                    routeData.Values.Add("action", "OtherHttpStatusCode");
                    routeData.Values.Add("httpStatusCode", httpException.GetHttpCode());
                    break;
            }

            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}
