using Newtonsoft.Json;
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
    class Product
    {
        public string ProductId { get; set; }
        public string Quantity { get; set; }
    }
    class Result
    {
        public string result { get; set; }
    }
    public partial class UpdateCrmProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                string productid = Request["productid"].ToString();
                string p2 = Request.Form["productid"];
                string quantity = Request["quantity"].ToString();
                var jsonObj = new Product() { ProductId = productid, Quantity = quantity };

                string requestUrl = "http://petdreamhouse-a.cloudapp.net:8000/restapp/v1/UpdateProduct";

                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;

                string sb = "{\"product\":"+JsonConvert.SerializeObject(jsonObj)+"}";

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
                    StreamReader sr = new StreamReader(stream1);
                    string strsb = sr.ReadToEnd();
                    Result objResponse = JsonConvert.DeserializeObject<Result>(strsb);
                    if (("success").Equals(objResponse.result))//用户名是否正确
                    {
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
                Console.WriteLine(ex.Message);
                //return null;
            }
        }
    }
}