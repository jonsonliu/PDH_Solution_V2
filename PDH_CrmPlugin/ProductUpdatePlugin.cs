using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using System.ServiceModel;
using Microsoft.Xrm.Sdk.Query;

namespace PDH_CrmPlugin
{
    public class ProductUpdatePlugin : IPlugin
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
                //product 状态变为Pre Live触发同步动作
                //(CRM)Live-7,Pre Live-2,Approved-3,Pending-4,Price Check-6,Pre-Selection-5,Discontinued-8
                if (PostImage.Contains("producttypecode"))
                {
                     
                    int pre_product_type_code = ((OptionSetValue)PreImage.Attributes["producttypecode"]).Value;
                    int post_product_type_code = ((OptionSetValue)PostImage.Attributes["producttypecode"]).Value;
                    //throw new InvalidPluginExecutionException("---" + pre_product_type_code.ToString() + "---" + post_product_type_code.ToString());
                    
                    //新建
                    if (pre_product_type_code == 5 && post_product_type_code == 2) 
                    {
                        
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
                        //ProductService.ProductSoapClient client = new ProductService.ProductSoapClient(myBinding, endPointAddress);
                        //ProductService.BaseRequest req = new ProductService.BaseRequest();
                        ProductService.BaseRequest req = new ProductService.BaseRequest();

                        if (PostImage.Contains("productid"))
                        {
                            req.crm_product_id = ((Guid)PostImage.Attributes["productid"]).ToString();
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
                            string m_rrp = ((Money)PostImage.Attributes["new_rrp"]).Value.ToString();
                            req.new_rrp = m_rrp.Substring(0,m_rrp.IndexOf(".")+3);
                        }
                        if (PostImage.Contains("price"))
                        {
                           string m_listprice  = ((Money)PostImage.Attributes["price"]).Value.ToString();
                           req.new_listprice = m_listprice.Substring(0, m_listprice.IndexOf(".") + 3);
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
                        //product attribute update
                        if (PostImage.Contains("new_colour"))
                        {
                            req.new_colour = (string)PostImage.Attributes["new_colour"];
                        }
                        if (PostImage.Contains("new_productbarcode"))
                        {
                            req.new_productbarcode = (string)PostImage.Attributes["new_productbarcode"];
                        }
                        if (PostImage.Contains("new_size"))
                        {
                            req.new_size = (string)PostImage.Attributes["new_size"];
                        }
                        if (PostImage.Contains("new_productweight"))
                        {
                            req.new_productweight = (string)PostImage.Attributes["new_productweight"];
                        }
                        if (PostImage.Contains("new_casequantity"))
                        {
                            req.new_casequantity = (string)PostImage.Attributes["new_casequantity"];
                        }
                        if (PostImage.Contains("new_casebarcode"))
                        {
                            req.new_casebarcode = (string)PostImage.Attributes["new_casebarcode"];
                        }
                        if (PostImage.Contains("new_casedimensions"))
                        {
                            req.new_casedimensions = (string)PostImage.Attributes["new_casedimensions"];
                        }
                        if (PostImage.Contains("new_caseweight"))
                        {
                            req.new_caseweight = (string)PostImage.Attributes["new_caseweight"];
                        }
                        if (PostImage.Contains("new_ctndetails"))
                        {
                            req.new_ctndetails = (string)PostImage.Attributes["new_ctndetails"];
                        }
                        //product subject update
                        //if (PostImage.Contains("subjectid"))
                        //{
                        //    EntityReference subject = (EntityReference)PostImage.Attributes["subjectid"];
                        //    req.subjectid = subject.Id.ToString();
                        //}
                        //if (PostImage.Contains("new_catalog1"))
                        //{
                            
                        //    int new_catalog1 = ((OptionSetValue)PostImage.Attributes["new_catalog1"]).Value;
                        //    req.new_catalog1 = new_catalog1.ToString();
                        //}
                        //if (PostImage.Contains("new_catalog2"))
                        //{

                        //    int new_catalog2 = ((OptionSetValue)PostImage.Attributes["new_catalog2"]).Value;
                        //    req.new_catalog2 = new_catalog2.ToString();
                        //}
                        //if (PostImage.Contains("new_catalog3"))
                        //{

                        //    int new_catalog3 = ((OptionSetValue)PostImage.Attributes["new_catalog3"]).Value;
                        //    req.new_catalog3 = new_catalog3.ToString();
                        //}
                        if (PostImage.Contains("new_parentcatalog"))
                        {

                            int new_catalog1 = ((OptionSetValue)PostImage.Attributes["new_parentcatalog"]).Value;
                            req.new_catalog1 = new_catalog1.ToString();
                        }
                        if (PostImage.Contains("new_subcatelog"))
                        {

                            int new_catalog2 = ((OptionSetValue)PostImage.Attributes["new_subcatelog"]).Value;
                            req.new_catalog2 = new_catalog2.ToString();
                        }
                        if (PostImage.Contains("new_additionalcatelog"))
                        {

                            int new_catalog3 = ((OptionSetValue)PostImage.Attributes["new_additionalcatelog"]).Value;
                            req.new_catalog3 = new_catalog3.ToString();
                        }
                        if (PostImage.Contains("new_suppliera1"))
                        {

                            EntityReference  supplier = (EntityReference )PostImage.Attributes["new_suppliera1"];

                            Entity supplier_acc = new Entity("account");
                            ColumnSet attributes = new ColumnSet(new string[] { "accountid", "new_manufacturerid" });

                            supplier_acc = service.Retrieve(supplier_acc.LogicalName, supplier.Id, attributes);

                            req.new_manufacturerid = (string)supplier_acc.Attributes["new_manufacturerid"];
                        }
                        
                        
                        //throw new InvalidPluginExecutionException(req.new_catalog1 + "--" + req.new_catalog2 + "--" + req.new_catalog3);
                        ProductService.BaseResponse res = client.CreateProduct(req);
                        
                    }

                    //更新，只有Live状态的product才允许更新，或者由pre-live变为live的时候也允许更新
                    else if (pre_product_type_code == 7 || (pre_product_type_code == 2 && post_product_type_code == 7))
                    {
                        // Adding Basic Http Binding and its properties.
                        BasicHttpBinding myBinding = new BasicHttpBinding();
                        myBinding.Name = "BasicHttpBinding_Service";
                        myBinding.Security.Mode = BasicHttpSecurityMode.None;
                        myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                        myBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
                        myBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

                        // Endpoint Address defining the asmx Service to be called.
                        EndpointAddress endPointAddress = new EndpointAddress(@"http://petdreamhouse.co.uk/soap/product_update_soap.php");
                        // Call to the Web Service using the Binding and End Point Address.
                        ProductUpdateService.ProductSoapClient client = new ProductUpdateService.ProductSoapClient(myBinding, endPointAddress);
                        //ProductService.ProductSoapClient client = new ProductService.ProductSoapClient(myBinding, endPointAddress);
                        //ProductService.BaseRequest req = new ProductService.BaseRequest();
                        ProductUpdateService.BaseRequest req = new ProductUpdateService.BaseRequest();

                        if (PostImage.Contains("productid"))
                        {
                            req.crm_product_id = ((Guid)PostImage.Attributes["productid"]).ToString();
                        }
                        if (PostImage.Contains("name"))
                        {
                            req.name = (string)PostImage.Attributes["name"];
                        }
                        //if (PostImage.Contains("description"))
                        //{
                        //    //replace "\n" with <br/>
                        //    string o_desc = (string)PostImage.Attributes["description"];
                        //    req.description = o_desc.Replace("\r\n", "<br/>");
                        //    req.description = o_desc.Replace("\r", "<br/>");
                        //    req.description = o_desc.Replace("\n", "<br/>");
                        //}
                        if (PostImage.Contains("productnumber"))
                        {
                            req.productnumber = (string)PostImage.Attributes["productnumber"];
                        }

                        if (PostImage.Contains("new_rrp"))
                        {
                            string m_rrp = ((Money)PostImage.Attributes["new_rrp"]).Value.ToString();
                            req.new_rrp = m_rrp.Substring(0, m_rrp.IndexOf(".") + 3);
                        }
                        if (PostImage.Contains("price"))
                        {
                            string m_listprice = ((Money)PostImage.Attributes["price"]).Value.ToString();
                            req.new_listprice = m_listprice.Substring(0, m_listprice.IndexOf(".") + 3);
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
                        //product attribute update
                        if (PostImage.Contains("new_colour"))
                        {
                            req.new_colour = (string)PostImage.Attributes["new_colour"];
                        }
                        if (PostImage.Contains("new_productbarcode"))
                        {
                            req.new_productbarcode = (string)PostImage.Attributes["new_productbarcode"];
                        }
                        if (PostImage.Contains("new_size"))
                        {
                            req.new_size = (string)PostImage.Attributes["new_size"];
                        }
                        if (PostImage.Contains("new_productweight"))
                        {
                            req.new_productweight = (string)PostImage.Attributes["new_productweight"];
                        }
                        if (PostImage.Contains("new_casequantity"))
                        {
                            req.new_casequantity = (string)PostImage.Attributes["new_casequantity"];
                        }
                        if (PostImage.Contains("new_casebarcode"))
                        {
                            req.new_casebarcode = (string)PostImage.Attributes["new_casebarcode"];
                        }
                        if (PostImage.Contains("new_casedimensions"))
                        {
                            req.new_casedimensions = (string)PostImage.Attributes["new_casedimensions"];
                        }
                        if (PostImage.Contains("new_caseweight"))
                        {
                            req.new_caseweight = (string)PostImage.Attributes["new_caseweight"];
                        }
                        if (PostImage.Contains("new_ctndetails"))
                        {
                            req.new_ctndetails = (string)PostImage.Attributes["new_ctndetails"];
                        }
                        //product subject update
                        //if (PostImage.Contains("subjectid"))
                        //{
                        //    EntityReference subject = (EntityReference)PostImage.Attributes["subjectid"];
                        //    req.subjectid = subject.Id.ToString();
                        //}

                        //if (PostImage.Contains("new_catalog1"))
                        //{

                        //    int new_catalog1 = ((OptionSetValue)PostImage.Attributes["new_catalog1"]).Value;
                        //    req.new_catalog1 = new_catalog1.ToString();
                        //}
                        //if (PostImage.Contains("new_catalog2"))
                        //{

                        //    int new_catalog2 = ((OptionSetValue)PostImage.Attributes["new_catalog2"]).Value;
                        //    req.new_catalog2 = new_catalog2.ToString();
                        //}
                        //if (PostImage.Contains("new_catalog3"))
                        //{

                        //    int new_catalog3 = ((OptionSetValue)PostImage.Attributes["new_catalog3"]).Value;
                        //    req.new_catalog3 = new_catalog3.ToString();
                        //}
                        if (PostImage.Contains("new_parentcatalog"))
                        {

                            int new_catalog1 = ((OptionSetValue)PostImage.Attributes["new_parentcatalog"]).Value;
                            req.new_catalog1 = new_catalog1.ToString();
                        }
                        if (PostImage.Contains("new_subcatelog"))
                        {

                            int new_catalog2 = ((OptionSetValue)PostImage.Attributes["new_subcatelog"]).Value;
                            req.new_catalog2 = new_catalog2.ToString();
                        }
                        if (PostImage.Contains("new_additionalcatelog"))
                        {

                            int new_catalog3 = ((OptionSetValue)PostImage.Attributes["new_additionalcatelog"]).Value;
                            req.new_catalog3 = new_catalog3.ToString();
                        }
                        if (PostImage.Contains("new_suppliera1"))
                        {

                            EntityReference supplier = (EntityReference)PostImage.Attributes["new_suppliera1"];

                            Entity supplier_acc = new Entity("account");
                            ColumnSet attributes = new ColumnSet(new string[] { "accountid", "new_manufacturerid" });

                            supplier_acc = service.Retrieve(supplier_acc.LogicalName, supplier.Id, attributes);

                            req.new_manufacturerid = (string)supplier_acc.Attributes["new_manufacturerid"];
                        }

                        //throw new InvalidPluginExecutionException(req.new_catalog1 + "--" + req.new_catalog2 + "--" + req.new_catalog3);

                        ProductUpdateService.BaseResponse res = client.UpdateProduct(req);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("错误：" + ex.ToString()+"\r\n"+ex.Message+"\r\n"+ex.InnerException);
            }
        }

    }
}
