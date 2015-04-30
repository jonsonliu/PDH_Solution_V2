using PDH_Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PDH_CrmService.CrmInterface;
using Microsoft.Xrm.Sdk;
using PDH_Model.RestCrmModel;
using PDH_CrmService;
using Microsoft.Xrm.Sdk.Client;

namespace PDH_RestService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“CrmRestService”。
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class CrmRestService : ICrmRestService
    {
        //public static CrmUtil cu = new CrmUtil();
        //public static OrganizationServiceProxy organizationProxy = cu.getCrmService();

        public List<AppCrmProduct> AppGetAllProduct()
        {
            ProductOperation po = new ProductOperation();
            List<AppCrmProduct> products = po.AppGetAllProduct();
            return products;
        }
        public AppCrmProduct AppGetProductById(string productId)
        {
            ProductOperation po = new ProductOperation();
            AppCrmProduct product = po.AppGetProductById(productId);
            return product;
        }
        public AppCrmAuthorization AppAuthorization(string email)
        {
            AccountOperation ao = new AccountOperation();
            AppCrmAuthorization auth = ao.AppAuthorization(email);
            return auth;
        }

        public AppCrmOrderResponse AppCreateOrder(AppCrmOrderRequest orderRequest)
        {
            OrderOperation oo = new OrderOperation();
            AppCrmOrderResponse rs = oo.AppCreateOrder(orderRequest);
            return rs;
        }

        public List<AnalysisCrmProduct> AnalysisGetAllProduct()
        {
            ProductOperation po = new ProductOperation();
            List<AnalysisCrmProduct> products = po.AnalysisGetAllProduct();
            return products;
        }
        public AnalysisCrmProduct AnalysisGetProductById(string productId)
        {
            ProductOperation po = new ProductOperation();
            AnalysisCrmProduct product = po.AnalysisGetProductById(productId);
            return product;
        }

        public List<SupplierCrmProduct> AppGetProductBySupplier(string email)
        {
            ProductOperation po = new ProductOperation();
            List<SupplierCrmProduct> products = po.AppGetProductBySupplier(email);
            return products;
        }

        public SupplierCrmProductResponse AppUpdateProduct(SupplierCrmProduct product)
        {
            ProductOperation po = new ProductOperation();
            SupplierCrmProductResponse response = po.AppUpdateProduct(product);
            return response;
        }
        public AppCrmAccount AppGetAccountByEmail(string email)
        {
            AccountOperation ao = new AccountOperation();
            AppCrmAccount account = ao.AppGetAccountByEmail(email);
            return account;
 
        }
        public List<AppCrmOrderHistoryResponse> AppOrderHistory(AppCrmOrderHistoryRequest orderHistoryRequest)
        {
            OrderOperation op = new OrderOperation();
            List<AppCrmOrderHistoryResponse> response = op.AppGetOrderHistory(orderHistoryRequest);
            return response;

        }
        public AppCrmProductCategory GetProdcutCategory()
        {
            ProductOperation po = new ProductOperation();
            AppCrmProductCategory response = po.AppGetProdcutCategory();
            return response;
        }
        public List<AppCrmProduct> GetProductsByCategory(AppCrmProductCategoryRequest request)
        {
            ProductOperation po = new ProductOperation();
            List<AppCrmProduct> response = po.AppGetProductsByCategory(request);
            return response;
        }
        public AppCrmAuthorization SupplierAuthorization(string email)
        {
            AccountOperation ao = new AccountOperation();
            AppCrmAuthorization auth = ao.SupplierAuthorization( email);
            return auth;
        }
        
    }
}
