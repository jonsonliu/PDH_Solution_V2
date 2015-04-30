using Newtonsoft.Json;
using PDH_Model.RestCrmModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SupplierPortal.Views.DataTable
{
    
    public partial class ViewCrmProductData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                //http://petdreamhouse-a.cloudapp.net:8000/restapp/v1/GetProductBySupplier/test@pdh.com

                string queryString = Session["User"].ToString();
                string url = "http://petdreamhouse-a.cloudapp.net:8000/restapp/v1/GetProductBySupplier/";


                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url + queryString);

                httpWebRequest.ContentType = "application/json;charset=utf-8;";
                httpWebRequest.Method = "GET";
                httpWebRequest.Timeout = 20000;

                //byte[] btBodys = Encoding.UTF8.GetBytes(body);
                //httpWebRequest.ContentLength = btBodys.Length;
                //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                string responseContent = streamReader.ReadToEnd();
                httpWebResponse.Close();
                streamReader.Close();
                //Response.Write(responseContent);
                //Response.End();

                Response.Clear();
                Response.Write(responseContent);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();

               // List<SupplierCrmProduct> products = JsonConvert.DeserializeObject<List<SupplierCrmProduct>>(responseContent);
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return null;
            }

        }

    }
}