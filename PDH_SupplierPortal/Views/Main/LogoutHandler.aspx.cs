using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SupplierPortal.Views.Main
{
    public partial class LogoutHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();//session值清空.
            Response.Write("{\"success\":true,\"msg\":\"成功\"}");
            Response.End();
        }
    }
}