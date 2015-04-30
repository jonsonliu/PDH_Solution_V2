using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace PDH_CrmPlugin
{
    public class OrderPlugin : IPlugin
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
            //Context = Info passed to the plugin at runtime
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));

            //Service = access to data for modification
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            // Adding Basic Http Binding and its properties.
            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.Name = "BasicHttpBinding_Service";
            myBinding.Security.Mode = BasicHttpSecurityMode.None;
            myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            myBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            myBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            // Endpoint Address defining the asmx Service to be called.
            EndpointAddress endPointAddress = new EndpointAddress(@"http://petdreamhouse.co.uk/soap/account_soap.php");

            // Call to the Web Service using the Binding and End Point Address.
            //TestService1.TestSoapClient client = new TestService1.TestSoapClient(myBinding, endPointAddress);
            //TestService1.BaseAddRequest req = new TestService1.BaseAddRequest();
            //req.arg1 = 1;
            //req.arg2 = 2;
            //TestService1.BaseAddResponse res = new TestService1.BaseAddResponse();
            //res = client.Add(req);

            //if (res != null)
            //{
            //    throw new InvalidPluginExecutionException("Success"+res.args);
            //}
            //else
            //{
            //    throw new InvalidPluginExecutionException("Failure");
            //}
        }
    }
}
