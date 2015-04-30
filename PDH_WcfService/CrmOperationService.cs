using Microsoft.Xrm.Sdk.Client;
using PDH_CrmService;
using PDH_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PDH_WcfService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true, InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CrmOperationService : ICrmOperationService
    {
        public static CrmUtil cu = new CrmUtil();
        public static OrganizationServiceProxy organizationProxy = cu.getCrmService();

        public void AddAccountAndContact(AddAccountAndContact AddAccountAndContact)
        {
            PDH_CrmService.CrmInterface.AccountOperation accountOp = new PDH_CrmService.CrmInterface.AccountOperation();
            AddAccountAndContactResponse response = accountOp.AddAccountAndContact(AddAccountAndContact);
            
            //return response;
        }

        public void UpdateTradeRegistration(String accountId)
        {
            PDH_CrmService.CrmInterface.AccountOperation accountOp = new PDH_CrmService.CrmInterface.AccountOperation();
            UpdateTradeRegistrationResponse response = accountOp.UpdateTradeRegistration(accountId);

            //return response;
        }

        public void CreateCrmOrder(CreateCrmOrder CreateCrmOrder)
        {
            PDH_CrmService.CrmInterface.OrderOperation orderOp = new PDH_CrmService.CrmInterface.OrderOperation();
            CrmOrderResponse response = orderOp.CreateCrmOrder(CreateCrmOrder);
            //return response;
 
        }

        public void UpdateProductQuantity(String productId, String quantity)
        {
            PDH_CrmService.CrmInterface.ProductOperation productOp = new PDH_CrmService.CrmInterface.ProductOperation();
            UpdateProductQuantityResponse response = productOp.UpdateProductQuantity(productId, quantity);
            //return response;
        }
    }
}
