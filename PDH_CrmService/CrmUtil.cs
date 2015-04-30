using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Reflection;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Crm.Sdk.Messages;

namespace PDH_CrmService
{
    public class CrmUtil
    {

        #region Class Level Members
        // To get discovery service address and organization unique name, 
        // Sign in to your CRM org and click Settings, Customization, Developer Resources.
        // On Developer Resource page, find the discovery service address under Service Endpoints and organization unique name under Your Organization Information.
       
        //环境切换 pro
        private String _discoveryServiceAddress = "https://disco.crm4.dynamics.com/XRMServices/2011/Discovery.svc";
        private String _organizationUniqueName = "petdreamhouseltd";
        private String _userName = "wen.liu@petdreamhouseltd.onmicrosoft.com";
        private String _password = "Petdreamhouse001";

        public  string pricelevelid_value = "780D3E7B-D6F9-E211-8ABD-984BE17C68D3";
        public string uomid_value = "3E055750-D6F9-E211-8ABD-984BE17C68D3";
        public string freeshipping_product_value = "85ACAB90-7248-E411-9212-D89D67639EF0";
        public string myce_taxratingid_value = "99596F96-054E-E411-9212-D89D67639EF0";

        //环境切换 test
        //private String _discoveryServiceAddress = "https://disco.crm.dynamics.com/XRMServices/2011/Discovery.svc";
        //private String _organizationUniqueName = "org2db9bf1b";
        //private String _userName = "liuzhen@liuzhen.onmicrosoft.com";
        //private String _password = "password_123";

        

        // Provide domain name for the On-Premises org.
        private String _domain = "";



        private OrganizationServiceProxy organizationProxy = null;

        private int flag = 5;

        log4net.ILog log = log4net.LogManager.GetLogger("fileLog");//获取一个日志记录器

        #endregion Class Level Members

        #region How To Sample Code
        /// <summary>
        /// 
        /// </summary>
        public OrganizationServiceProxy getCrmService()
        {
            
            try
            {
                if (organizationProxy == null)
                {
                    flag--; 
                    //<snippetAuthenticateWithNoHelp1>
                    IServiceManagement<IDiscoveryService> serviceManagement =
                                ServiceConfigurationFactory.CreateManagement<IDiscoveryService>(
                                new Uri(_discoveryServiceAddress));
                    AuthenticationProviderType endpointType = serviceManagement.AuthenticationType;

                    // Set the credentials.
                    AuthenticationCredentials authCredentials = GetCredentials(serviceManagement, endpointType);


                    String organizationUri = String.Empty;
                    // Get the discovery service proxy.
                    using (DiscoveryServiceProxy discoveryProxy =
                        GetProxy<IDiscoveryService, DiscoveryServiceProxy>(serviceManagement, authCredentials))
                    {
                        // Obtain organization information from the Discovery service. 
                        if (discoveryProxy != null)
                        {
                            // Obtain information about the organizations that the system user belongs to.
                            OrganizationDetailCollection orgs = DiscoverOrganizations(discoveryProxy);
                            // Obtains the Web address (Uri) of the target organization.
                            organizationUri = FindOrganization(_organizationUniqueName,
                                orgs.ToArray()).Endpoints[EndpointType.OrganizationService];

                        }
                    }
                    //</snippetAuthenticateWithNoHelp1>


                    if (!String.IsNullOrWhiteSpace(organizationUri))
                    {
                        //<snippetAuthenticateWithNoHelp3>
                        IServiceManagement<IOrganizationService> orgServiceManagement =
                            ServiceConfigurationFactory.CreateManagement<IOrganizationService>(
                            new Uri(organizationUri));

                        // Set the credentials.
                        AuthenticationCredentials credentials = GetCredentials(orgServiceManagement, endpointType);

                        // Get the organization service proxy.
                        using (organizationProxy =
                            GetProxy<IOrganizationService, OrganizationServiceProxy>(orgServiceManagement, credentials))
                        {
                            // This statement is required to enable early-bound type support.
                            organizationProxy.EnableProxyTypes();
                            TimeSpan time = new TimeSpan(0, 5, 0);//TimeSpan(H,M,S)
                            organizationProxy.Timeout = time;

                            log.Info("create crm organizationProxy success!" + organizationProxy.UserPrincipalName);
                            // Now make an SDK call with the organization service proxy.
                            // Display information about the logged on user.
                            //Guid userid = ((WhoAmIResponse)organizationProxy.Execute(
                            //    new WhoAmIRequest())).UserId;

                            //Entity account = new Entity("account");
                            //account["name"] = "Fourth Coffee";
                            //organizationProxy.Create(account);
                            //return organizationProxy;
                        }
                        //</snippetAuthenticateWithNoHelp3>
                    }
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
            {
                // You can handle an exception here or pass it back to the calling method.
                log.Info("create crm organizationProxy failure!");
                //尝试5次
                if (flag > 0) 
                {
                    getCrmService();
                }
                
            }
            
            //RenewTokenIfRequired(organizationProxy);
            return organizationProxy;

        }

        //internal static void RenewTokenIfRequired(OrganizationServiceProxy proxy)
        //{
        //    if ((proxy.SecurityTokenResponse != null) && (DateTime.UtcNow.AddMinutes(15) >= proxy.SecurityTokenResponse.Response.Lifetime.Expires))
        //    {
        //        try
        //        {
        //            proxy.Authenticate();
        //        }
        //        catch (CommunicationException ce)
        //        {
        //            if (proxy.SecurityTokenResponse == null || DateTime.UtcNow >= proxy.SecurityTokenResponse.Response.Lifetime.Expires)
        //            {
        //                //Help! Somebody! Do something!
        //            }
        //        }
        //    }
        //}

        //<snippetAuthenticateWithNoHelp2>
        /// <summary>
        /// Obtain the AuthenticationCredentials based on AuthenticationProviderType.
        /// </summary>
        /// <param name="service">A service management object.</param>
        /// <param name="endpointType">An AuthenticationProviderType of the CRM environment.</param>
        /// <returns>Get filled credentials.</returns>
        private AuthenticationCredentials GetCredentials<TService>(IServiceManagement<TService> service, AuthenticationProviderType endpointType)
        {
            AuthenticationCredentials authCredentials = new AuthenticationCredentials();

            switch (endpointType)
            {
                case AuthenticationProviderType.ActiveDirectory:
                    authCredentials.ClientCredentials.Windows.ClientCredential =
                        new System.Net.NetworkCredential(_userName,
                            _password,
                            _domain);
                    break;
                case AuthenticationProviderType.LiveId:
                    authCredentials.ClientCredentials.UserName.UserName = _userName;
                    authCredentials.ClientCredentials.UserName.Password = _password;
                    authCredentials.SupportingCredentials = new AuthenticationCredentials();
                    //authCredentials.SupportingCredentials.ClientCredentials =
                    //    DeviceIdManager.LoadOrRegisterDevice();
                    break;
                default: // For Federated and OnlineFederated environments.                    
                    authCredentials.ClientCredentials.UserName.UserName = _userName;
                    authCredentials.ClientCredentials.UserName.Password = _password;
                    // For OnlineFederated single-sign on, you could just use current UserPrincipalName instead of passing user name and password.
                    // authCredentials.UserPrincipalName = UserPrincipal.Current.UserPrincipalName;  // Windows Kerberos

                    // The service is configured for User Id authentication, but the user might provide Microsoft
                    // account credentials. If so, the supporting credentials must contain the device credentials.
                    if (endpointType == AuthenticationProviderType.OnlineFederation)
                    {
                        IdentityProvider provider = service.GetIdentityProvider(authCredentials.ClientCredentials.UserName.UserName);
                        if (provider != null && provider.IdentityProviderType == IdentityProviderType.LiveId)
                        {
                            authCredentials.SupportingCredentials = new AuthenticationCredentials();
                            //authCredentials.SupportingCredentials.ClientCredentials =
                            //    DeviceIdManager.LoadOrRegisterDevice();
                        }
                    }

                    break;
            }

            return authCredentials;
        }
        //</snippetAuthenticateWithNoHelp2>

        /// <summary>
        /// Discovers the organizations that the calling user belongs to.
        /// </summary>
        /// <param name="service">A Discovery service proxy instance.</param>
        /// <returns>Array containing detailed information on each organization that 
        /// the user belongs to.</returns>
        public OrganizationDetailCollection DiscoverOrganizations(
            IDiscoveryService service)
        {
            if (service == null) throw new ArgumentNullException("service");
            RetrieveOrganizationsRequest orgRequest = new RetrieveOrganizationsRequest();
            RetrieveOrganizationsResponse orgResponse =
                (RetrieveOrganizationsResponse)service.Execute(orgRequest);

            return orgResponse.Details;
        }

        /// <summary>
        /// Finds a specific organization detail in the array of organization details
        /// returned from the Discovery service.
        /// </summary>
        /// <param name="orgUniqueName">The unique name of the organization to find.</param>
        /// <param name="orgDetails">Array of organization detail object returned from the discovery service.</param>
        /// <returns>Organization details or null if the organization was not found.</returns>
        /// <seealso cref="DiscoveryOrganizations"/>
        public OrganizationDetail FindOrganization(string orgUniqueName,
            OrganizationDetail[] orgDetails)
        {
            if (String.IsNullOrWhiteSpace(orgUniqueName))
                throw new ArgumentNullException("orgUniqueName");
            if (orgDetails == null)
                throw new ArgumentNullException("orgDetails");
            OrganizationDetail orgDetail = null;

            foreach (OrganizationDetail detail in orgDetails)
            {
                if (String.Compare(detail.UniqueName, orgUniqueName,
                    StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    orgDetail = detail;
                    break;
                }
            }
            return orgDetail;
        }

        /// <summary>
        /// Generic method to obtain discovery/organization service proxy instance.
        /// </summary>
        /// <typeparam name="TService">
        /// Set IDiscoveryService or IOrganizationService type to request respective service proxy instance.
        /// </typeparam>
        /// <typeparam name="TProxy">
        /// Set the return type to either DiscoveryServiceProxy or OrganizationServiceProxy type based on TService type.
        /// </typeparam>
        /// <param name="serviceManagement">An instance of IServiceManagement</param>
        /// <param name="authCredentials">The user's Microsoft Dynamics CRM logon credentials.</param>
        /// <returns></returns>
        /// <snippetAuthenticateWithNoHelp4>
        private TProxy GetProxy<TService, TProxy>(
            IServiceManagement<TService> serviceManagement,
            AuthenticationCredentials authCredentials)
            where TService : class
            where TProxy : ServiceProxy<TService>
        {
            Type classType = typeof(TProxy);

            if (serviceManagement.AuthenticationType !=
                AuthenticationProviderType.ActiveDirectory)
            {
                AuthenticationCredentials tokenCredentials =
                    serviceManagement.Authenticate(authCredentials);
                // Obtain discovery/organization service proxy for Federated, LiveId and OnlineFederated environments. 
                // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and SecurityTokenResponse.
                return (TProxy)classType
                    .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(SecurityTokenResponse) })
                    .Invoke(new object[] { serviceManagement, tokenCredentials.SecurityTokenResponse });
            }

            // Obtain discovery/organization service proxy for ActiveDirectory environment.
            // Instantiate a new class of type using the 2 parameter constructor of type IServiceManagement and ClientCredentials.
            return (TProxy)classType
                .GetConstructor(new Type[] { typeof(IServiceManagement<TService>), typeof(ClientCredentials) })
                .Invoke(new object[] { serviceManagement, authCredentials.ClientCredentials });
        }
        /// </snippetAuthenticateWithNoHelp4>

        #endregion How To Sample Code
    }
}
