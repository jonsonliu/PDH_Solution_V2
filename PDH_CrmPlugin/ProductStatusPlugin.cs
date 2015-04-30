using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace PDH_CrmPlugin
{
    public class ProductStatusPlugin : IPlugin
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

                Entity  PreImage = (Entity)context.PreEntityImages["PreImage"];
                Entity PostImage = (Entity)context.PostEntityImages["PostImage"];

                // Adding Basic Http Binding and its properties.
                BasicHttpBinding myBinding = new BasicHttpBinding();
                myBinding.Name = "BasicHttpBinding_Service";
                myBinding.Security.Mode = BasicHttpSecurityMode.None;
                myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                myBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                myBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

                // Endpoint Address defining the asmx Service to be called.
                EndpointAddress endPointAddress = new EndpointAddress(@"http://petdreamhouse.co.uk/soap/product_status_soap.php");
                // Call to the Web Service using the Binding and End Point Address.
                ProductStatusService.ProductStatusSoapClient client = new ProductStatusService.ProductStatusSoapClient(myBinding, endPointAddress);
                ProductStatusService.BaseRequest req = new ProductStatusService.BaseRequest();

                if (PostImage.Contains("productid"))
                {
                    req.crm_product_id = ((Guid)PostImage.Attributes["productid"]).ToString();
                }
                //if (context.OutputParameters.Contains("id"))
                //{
                //    req.crm_product_id = context.OutputParameters["id"].ToString();
                //}
                //(CRM)Live-7,Pre Live-2,Approved-3,Pending-4,Price Check-6,Pre-Selection-5,Discontinued-8
                //(CMS)Disabled-0,Enabled-1
                if (PostImage.Contains("producttypecode"))
                {
                    //int crmStatus = ((OptionSetValue)PostImage.Attributes["producttypecode"]).Value;
                    int pre_product_type_code = ((OptionSetValue)PreImage.Attributes["producttypecode"]).Value;
                    int post_product_type_code = ((OptionSetValue)PostImage.Attributes["producttypecode"]).Value;

                    if (pre_product_type_code != 7 && post_product_type_code == 7)
                    {
                        req.status = "1";
                        ProductStatusService.BaseResponse res = client.UpdateProductStatus(req);
                    }
                    else if (pre_product_type_code == 7 && post_product_type_code != 7)
                    {
                        req.status = "0";
                        ProductStatusService.BaseResponse res = client.UpdateProductStatus(req);
                    }
                }
                
                
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("错误：" + ex.ToString());
            }
        }
    }
}
