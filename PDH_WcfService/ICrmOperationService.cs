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
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IAccountService”。
    [ServiceContract]
    public interface ICrmOperationService
    {
        
        //新增Account、Contact。
        [OperationContract(IsOneWay = true)]
        void AddAccountAndContact(AddAccountAndContact AddAccountAndContact);

        //CMS-->Approved
        [OperationContract(IsOneWay = true)]
        void UpdateTradeRegistration(String accountId);

        //CMS-->Create Order
        [OperationContract(IsOneWay = true)]
        void CreateCrmOrder(CreateCrmOrder CreateCrmOrder);

        //CMS-->Product Quantity
        [OperationContract(IsOneWay = true)]
        void UpdateProductQuantity(String productId, String quantity);
    }
}
