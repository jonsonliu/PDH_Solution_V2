using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace SupplierPortal.Filter
{
    public class RoleFilter: IHttpModule
    {
        public String ModuleName
        {
            get { return "RoleFilter"; }
        }
        public void Init(HttpApplication application)
        {
            application.AcquireRequestState += new EventHandler(Application_AcquireRequestState);
        }
        private void Application_AcquireRequestState(Object source, EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpContext context = application.Context;
            HttpSessionState session = context.Session;
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            String contextPath = request.ApplicationPath;
            string url = request.Path;
            if (url.EndsWith(".aspx")) {
                if (url.Contains("LoginPage.aspx"))
                {
                    return;
                }
                else if (session == null)
                {
                    response.Redirect("LoginPage.aspx");
                }
                else if (session["User"] == null)
                {
                    response.Redirect("LoginPage.aspx");
                }
            }
           

        }
        public void Dispose()
        {
        }
    }
}