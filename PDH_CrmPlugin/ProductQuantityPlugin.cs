using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace PDH_CrmPlugin
{
    public class ProductQuantityPlugin : IPlugin
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
                EndpointAddress endPointAddress = new EndpointAddress(@"http://petdreamhouse.co.uk/soap/product_quantity_soap.php");
                // Call to the Web Service using the Binding and End Point Address.
                ProductQuantityService.ProductQuantitySoapClient client = new ProductQuantityService.ProductQuantitySoapClient();
                ProductQuantityService.BaseQuantityRequest req = new ProductQuantityService.BaseQuantityRequest();
                
                if (context.OutputParameters.Contains("id"))
                {
                    req.crm_product_id = context.OutputParameters["id"].ToString();
                }
                if (PostImage.Contains("quantityonhand"))
                {
                    req.quantity = (string)PostImage.Attributes["quantityonhand"];
                }
                ProductQuantityService.BaseQuantityResponse res = client.UpdateProductQuantity(req);


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
                throw new InvalidPluginExecutionException("错误：" + ex.ToString());
            }
        }
    }
}
