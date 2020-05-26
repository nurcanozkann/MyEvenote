using MyEvernote.Common;
using MyEvernote.WebUI.WebCommon;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyEvernote.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            App.Common = new WebCommons();
        }
    }
}