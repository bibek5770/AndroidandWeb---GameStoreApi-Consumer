using FrontEnd.Controllers;
using FrontEnd.UserRole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace FrontEnd
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                string role = string.Empty;
                if (ticket.Name != "")
                {
                    var temp = ticket.Name.Split(' ');
                    role = temp[2].ToString();
                }
                UserIdentity identity = new UserIdentity(ticket.Name, role);
                UserPrincipal principal = new UserPrincipal(identity);
                HttpContext.Current.User = principal;
            }
        }
    }
}
