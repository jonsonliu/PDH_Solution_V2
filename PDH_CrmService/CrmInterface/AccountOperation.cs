using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDH_Model;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Crm.Sdk.Messages;
using PDH_Model.RestCrmModel;

namespace PDH_CrmService.CrmInterface
{
    public class AccountOperation
    {
        log4net.ILog log = log4net.LogManager.GetLogger("fileLog");//获取一个日志记录器
        public AddAccountAndContactResponse AddAccountAndContact(AddAccountAndContact AddAccountAndContact) 
        {
            log.Info("调用AddAccountAndContact接口。");
            AddAccountAndContactResponse response = new AddAccountAndContactResponse();
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            try
            {
                //1.Create Contact
                Entity contact = new Entity("contact");
                log.Info("request的值为："+AddAccountAndContact.ToString());
                //if (AddAccountAndContact != null) {
                //    log.Info("request is not null");
                //    log.Info(AddAccountAndContact);
                //    foreach (System.Reflection.PropertyInfo p in AddAccountAndContact.GetType().GetProperties())
                //    {
                //        log.Info("Name:"+p.Name+"--Value:"+p.GetValue(AddAccountAndContact));
                //    }
                //}
                contact["firstname"] = AddAccountAndContact.ContFirstName;
                contact["lastname"] = AddAccountAndContact.ContLastName;
                contact["emailaddress1"] = AddAccountAndContact.ContEmail;
                contact["telephone1"] = AddAccountAndContact.ContTelephone1;
                contact["address1_line1"] = AddAccountAndContact.ContStreet1;
                contact["address1_line2"] = AddAccountAndContact.ContStreet2;
                contact["address1_city"] = AddAccountAndContact.ContCity;
                contact["address1_stateorprovince"] = AddAccountAndContact.ContStateProvince;
                contact["address1_country"] = AddAccountAndContact.ContCountryRegion;
                contact["address1_postalcode"] = AddAccountAndContact.ContZIPPostalCode;

                Guid contactId = organizationProxy.Create(contact);
                log.Info("创建contact成功：" + contactId.ToString());
                //2.Create Account
                Entity account = new Entity("account");
                account["name"] = AddAccountAndContact.AccAccountName;
                account["telephone1"] = AddAccountAndContact.AccPhone;
                account["fax"] = AddAccountAndContact.AccFax;
                account["emailaddress1"] = AddAccountAndContact.AccEmail;
                account["address1_line1"] = AddAccountAndContact.AccStreet1;
                account["address1_line2"] = AddAccountAndContact.AccStreet2;
                account["address1_city"] = AddAccountAndContact.AccCity;
                account["address1_stateorprovince"] = AddAccountAndContact.AccStateProvince;
                account["address1_country"] = AddAccountAndContact.AccCountryRegion;
                account["address1_postalcode"] = AddAccountAndContact.AccZIPPostalCode;
                if(AddAccountAndContact.AccNewsletter == "0")
                {
                    account["new_newslettersubscribe"] = false;
                }
                else if (AddAccountAndContact.AccNewsletter == "1")
                {
                    account["new_newslettersubscribe"] = true;
                }

                account["primarycontactid"] = new EntityReference("contact", contactId);

                Guid accountId = organizationProxy.Create(account);
                log.Info("创建account成功：" + accountId.ToString());

                //update contact company..
                UpdateContactCompany(contactId, accountId);

                response.AccountId = accountId.ToString();
                response.ErrCode = "0";
                return response;
            }
            catch (Exception ex) {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return null;
        }

        public void UpdateContactCompany(Guid contactId,Guid accountId)
        {
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();

            try
            {
                Entity contact = new Entity("contact");
                ColumnSet attributes = new ColumnSet(new string[] { "firstname", "ownerid" });

                contact = organizationProxy.Retrieve(contact.LogicalName, contactId, attributes);
                log.Info("Update contact  company: " + contact["firstname"]);//contact["firstname"]必须有值，否则报错：given key not in 。。

                contact["parentcustomerid"] = new EntityReference("account", accountId);

                organizationProxy.Update(contact);


            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "\r\n" + ex.ToString());
            }
        }

        public UpdateTradeRegistrationResponse UpdateTradeRegistration(String accountId)
        {
            log.Info("CMS-->Approved：" +  accountId);
            UpdateTradeRegistrationResponse response = new UpdateTradeRegistrationResponse();
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();

            try
            {
            //Update Account TradeRegistration
            //Entity account = new Entity("account");
            //ColumnSet attributes = new ColumnSet(new string[] { "new_check" });

            //account = organizationProxy.Retrieve(account.LogicalName, new Guid(accountId), attributes);
            //log.Info("Retrieved new_check: " + account["new_check"]);

            Entity account = RetriveAccountByEmail(organizationProxy, accountId);

            OptionSetValue register = new OptionSetValue(1);

            //account.Attributes.Add("new_check",  register);
            account["new_check"] = true;
            

            organizationProxy.Update(account);



            response.ErrCode = "0";
            return response;
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return null;
        }
        private Entity RetriveAccountByEmail(OrganizationServiceProxy service, String email)
        {
            QueryByAttribute querybyattribute = new QueryByAttribute("account");
            querybyattribute.ColumnSet = new ColumnSet(true);

            //  Attribute to query.
            querybyattribute.Attributes.AddRange("emailaddress1");

            //  Value of queried attribute to return.
            querybyattribute.Values.AddRange(email);

            //  Query passed to service proxy.
            EntityCollection retrieved = service.RetrieveMultiple(querybyattribute);
            foreach (Entity entity in retrieved.Entities)
            {
                return entity;
            }
            return null;
        }

        public AppCrmAuthorization AppAuthorization(string email)
        {
            log.Info("APP Login-->：" + email);
            AppCrmAuthorization auth = new AppCrmAuthorization();
            auth.IsLoginSuccess = "false";
            try
            {
                CrmUtil cu = new CrmUtil();
                OrganizationServiceProxy organizationProxy = cu.getCrmService();
                Entity account = RetriveAccountByEmail(organizationProxy, email);
                if (account != null)
                {
                    if (account.Contains("new_check") && account.GetAttributeValue<bool>("new_check"))
                    {
                        auth.IsLoginSuccess = "true";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return auth;
        }
        public AppCrmAuthorization SupplierAuthorization(string email)
        {
            log.Info("Supplier Login-->：" + email);
            AppCrmAuthorization auth = new AppCrmAuthorization();
            auth.IsLoginSuccess = "false";
            try
            {
                CrmUtil cu = new CrmUtil();
                OrganizationServiceProxy organizationProxy = cu.getCrmService();
                Entity account = RetriveAccountByEmail(organizationProxy, email);
                if (account != null)
                {
                    if (account.Contains("new_signedsuppliers") && account.GetAttributeValue<bool>("new_signedsuppliers"))
                    {
                        auth.IsLoginSuccess = "true";
                    }
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return auth;
        }
        

        public AppCrmAccount AppGetAccountByEmail(string email)
        {
            log.Info("AppGetAccountByEmail-->：" + email);
            AppCrmAccount new_account = new AppCrmAccount();
            try
            {
                CrmUtil cu = new CrmUtil();
                OrganizationServiceProxy organizationProxy = cu.getCrmService();
                Entity account = RetriveAccountByEmail(organizationProxy, email);

                
                new_account.AccName = account.GetAttributeValue<string>("name");
                new_account.AccPhone = account.GetAttributeValue<string>("telephone1");
                new_account.AccFax = account.GetAttributeValue<string>("fax");
                new_account.AccEmail = account.GetAttributeValue<string>("emailaddress1");
                new_account.AccStreet1 = account.GetAttributeValue<string>("address1_line1");
                new_account.AccStreet2 = account.GetAttributeValue<string>("address1_line2");
                new_account.AccCity = account.GetAttributeValue<string>("address1_city");
                new_account.AccStateProvince = account.GetAttributeValue<string>("address1_stateorprovince");
                new_account.AccCountryRegion = account.GetAttributeValue<string>("address1_country");
                new_account.AccZIPPostalCode = account.GetAttributeValue<string>("address1_postalcode");

                if (account.Contains("new_check") && account.GetAttributeValue<bool>("new_check"))
                {
                    new_account.AccTradeRegistration = "true";
                }
                else
                {
                    new_account.AccTradeRegistration = "false";
                }
                

            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return new_account;
        }

    }
}
