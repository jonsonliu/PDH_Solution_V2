using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SupplierPortal.Util;
using System.Text;
using System.Net;
using System.IO;


namespace SupplierPortal.Views.Main
{
    class Account
    {
        public string email { get; set; }
        
    }
    class Result
    {
        public string IsLoginSuccess { get; set; }
    }
    public partial class LoginHandler : System.Web.UI.Page
    {
        protected System.Data.SqlClient.SqlConnection Cn;
        protected System.Data.SqlClient.SqlCommand Cm;
        protected System.Data.SqlClient.SqlDataAdapter Da;
        protected System.Data.DataSet Ds;
        protected System.Data.SqlClient.SqlDataReader Dr;


        log4net.ILog log = log4net.LogManager.GetLogger("fileLog");//获取一个日志记录器
        protected void Page_Load(object sender, EventArgs e)
        {


            string username = Request.Form["loginname"];
            var jsonObj = new Account() { email = username };


            try
            {
                string requestUrl = "http://petdreamhouse-a.cloudapp.net:8000/restapp/v1/SupplierAuthorization";

                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                //WebRequest WR = WebRequest.Create(requestUrl);
               
                string sb= JsonConvert.SerializeObject(jsonObj);

                request.Method = "POST";// "POST";
                request.ContentType = "application/json"; // "application/json";
               
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();


                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).",
                        response.StatusCode,
                        response.StatusDescription));
                  //  DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));
                   // object objResponse = JsonConvert.DeserializeObject();
                    Stream stream1 = response.GetResponseStream();
                    StreamReader sr=new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    Result objResponse = JsonConvert.DeserializeObject<Result>(strsb);
                    if (("true").Equals(objResponse.IsLoginSuccess))//用户名是否正确
                    {
                        Session["User"] = username;
                        //Response.Write("<script>alert('登陆成功');window.window.location.href='MasterPage.aspx';</script>");
                        Response.Write("{\"success\":true,\"msg\":\"success\"}");
                        Response.End();

                    }
                    else
                    {
                       
                        Response.Write("{\"success\":false,\"msg\":\"failure\"}");
                        Response.End();

                    }
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                //return null;
            }
        

        }

        
    }
}