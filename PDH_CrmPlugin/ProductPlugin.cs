using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace PDH_CrmPlugin
{
    public class ProductPlugin : IPlugin
    {
        /// <summary>
        /// A plug-in that auto generates an account number when an
        /// account is created.
        /// </summary>
        /// <remarks>Register this plug-in on the Create message, account entity,
        /// and pre-operation stage.
        /// </remarks>
        //<snippetAccountNumberPlugin2>
        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                //Context = Info passed to the plugin at runtime
                IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
                IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

                //Service = access to data for modification
                IOrganizationService service = factory.CreateOrganizationService(context.UserId);

                //Entity  PreImage = (Entity)context.PreEntityImages["PreImage"];
                Entity PostImage = (Entity)context.PostEntityImages["PostImage"];

                // Adding Basic Http Binding and its properties.
                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "BasicHttpBinding_Service";
                myBinding.Security.Mode = BasicHttpSecurityMode.None;
                myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                myBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                myBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

                // Endpoint Address defining the asmx Service to be called.
                EndpointAddress endPointAddress = new EndpointAddress(@"http://petdreamhouse.co.uk/soap/product_soap.php");
                // Call to the Web Service using the Binding and End Point Address.
                ProductService.ProductSoapClient client = new ProductService.ProductSoapClient(myBinding, endPointAddress);
                ProductService.BaseRequest req = new ProductService.BaseRequest();

                if (context.OutputParameters.Contains("id"))
                {
                    req.crm_product_id = context.OutputParameters["id"].ToString();
                }
                if (PostImage.Contains("name"))
                {
                    req.name = (string)PostImage.Attributes["name"];
                }
                if (PostImage.Contains("description"))
                {
                    //replace "\n" with <br/>
                    string o_desc = (string)PostImage.Attributes["description"];
                    req.description = o_desc.Replace("\r\n", "<br/>");
                    req.description = o_desc.Replace("\r", "<br/>");
                    req.description = o_desc.Replace("\n", "<br/>");
                }
                if (PostImage.Contains("productnumber"))
                {
                    req.productnumber = (string)PostImage.Attributes["productnumber"];
                }
                if (PostImage.Contains("new_rrp"))
                {
                    req.new_rrp = ((Money)PostImage.Attributes["new_rrp"]).Value.ToString();
                }
                if (PostImage.Contains("price"))
                {
                     req.new_listprice = ((Money)PostImage.Attributes["price"]).Value.ToString();
                }
                if (PostImage.Contains("quantityonhand"))
                {
                    req.quantityonhand = ((decimal)PostImage.Attributes["quantityonhand"]).ToString();
                }
                if (PostImage.Contains("new_unitdepthcm"))
                {
                    req.new_unitdepthcm = ((decimal)PostImage.Attributes["new_unitdepthcm"]).ToString();
                }
                if (PostImage.Contains("new_unitwidthcm"))
                {
                    req.new_unitwidthcm = ((decimal)PostImage.Attributes["new_unitwidthcm"]).ToString();
                }
                if (PostImage.Contains("new_unitheight"))
                {
                    req.new_unitheight = ((decimal)PostImage.Attributes["new_unitheight"]).ToString();
                }

                ProductService.BaseResponse res = client.CreateProduct(req);

                //if (res != null)
                //{
                //    throw new InvalidPluginExecutionException("Success" + res.ErrCode);
                //}
                //else
                //{
                //    throw new InvalidPluginExecutionException("Failure");
                //}
            }
            catch (Exception ex) 
            {
                throw new InvalidPluginExecutionException("错误："+ex.ToString());
            }
        }
    }
}
