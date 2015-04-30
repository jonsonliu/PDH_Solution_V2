using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using PDH_Model;
using System.ServiceModel.Web;
using Microsoft.Xrm.Sdk;
using PDH_Model.RestCrmModel;

namespace PDH_RestService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ICrmRestService”。
    [ServiceContract]
    public interface ICrmRestService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/restapp/v1/GetAllProduct", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<AppCrmProduct> AppGetAllProduct();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/restapp/v1/GetProductById/{productId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AppCrmProduct AppGetProductById(string productId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/restapp/v1/Authorization",BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AppCrmAuthorization AppAuthorization(string email);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/restapp/v1/CreateOrder", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AppCrmOrderResponse AppCreateOrder(AppCrmOrderRequest orderRequest);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/analysis/v1/GetAllProduct", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<AnalysisCrmProduct> AnalysisGetAllProduct();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/analysis/v1/GetProductById/{productId}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AnalysisCrmProduct AnalysisGetProductById(string productId);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/restapp/v1/GetProductBySupplier/{email}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<SupplierCrmProduct> AppGetProductBySupplier(string email);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/restapp/v1/UpdateProduct", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        SupplierCrmProductResponse AppUpdateProduct(SupplierCrmProduct product);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/restapp/v1/GetAccountByEmail/{email}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AppCrmAccount AppGetAccountByEmail(string email);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/restapp/v1/OrderHistory", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<AppCrmOrderHistoryResponse> AppOrderHistory(AppCrmOrderHistoryRequest orderHistoryRequest);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/restapp/v1/GetProdcutCategory", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AppCrmProductCategory GetProdcutCategory();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/restapp/v1/GetProductsByCategory", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<AppCrmProduct> GetProductsByCategory(AppCrmProductCategoryRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/restapp/v1/SupplierAuthorization", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        AppCrmAuthorization SupplierAuthorization(string email);
  

    }
}
