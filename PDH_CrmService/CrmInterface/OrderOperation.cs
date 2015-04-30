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
using PDH_Model.ExportCrmModel;
using Microsoft.Xrm.Sdk.Messages;
using System.Xml;

namespace PDH_CrmService.CrmInterface
{
    public class OrderOperation
    {
        log4net.ILog log = log4net.LogManager.GetLogger("fileLog");//获取一个日志记录器
        public CrmOrderResponse CreateCrmOrder(CreateCrmOrder CreateCrmOrder)
        {


            log.Info("Call CreateCrmOrder Interface....");
            CrmOrderResponse response = new CrmOrderResponse();
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            try
            {
                //1.Create order
                Entity salesorder = new Entity("salesorder");
                //if (CreateCrmOrder != null)
                //{
                //    log.Info("request is not null");
                //    log.Info(CreateCrmOrder);
                //    foreach (System.Reflection.PropertyInfo p in CreateCrmOrder.GetType().GetProperties())
                //    {
                //        log.Info("Name:" + p.Name + "--Value:" + p.GetValue(CreateCrmOrder));
                //    }
                //}

                
                salesorder["name"] = CreateCrmOrder.OrderName;

                Entity email_account = RetriveAccountByEmail(organizationProxy, CreateCrmOrder.CustomerId);
                salesorder["customerid"] = new EntityReference("account", email_account.Id);

                //salesorder["pricelevelid"] = new EntityReference("pricelevel", new Guid(CreateCrmOrder.PricelevelId));
                
                //salesorder["pricelevelid"] = new EntityReference("pricelevel", new Guid("52CFEDBC-FF7A-E411-80D6-AC162DB4BC5C"));
                salesorder["pricelevelid"] = new EntityReference("pricelevel", new Guid(cu.pricelevelid_value));

                string Po_Numbser = CreateCrmOrder.PoNumbser;
                if (Po_Numbser != null && Po_Numbser != "" && Po_Numbser.Length > 390)
                {
                    Po_Numbser = Po_Numbser.Substring(0, 390);
                }
                salesorder["new_ordernumber"] = Po_Numbser;
                OptionSetValue pv = new OptionSetValue(int.Parse(CreateCrmOrder.PaymenttermsCode));
                salesorder["paymenttermscode"] = pv;
                salesorder["billto_name"] = CreateCrmOrder.BilltoName;
                salesorder["billto_line1"] = CreateCrmOrder.BilltoLine1;
                salesorder["billto_line2"] = CreateCrmOrder.BilltoLine2;
                salesorder["billto_city"] = CreateCrmOrder.BilltoCity;
                salesorder["billto_stateorprovince"] = CreateCrmOrder.BilltoStateOrProvince;
                salesorder["billto_country"] = CreateCrmOrder.BilltoCountry;
                salesorder["billto_postalcode"] = CreateCrmOrder.BilltoPostalcode;
                salesorder["shipto_name"] = CreateCrmOrder.ShiptoName;
                salesorder["shipto_line1"] = CreateCrmOrder.ShiptoLine1;
                salesorder["shipto_line2"] = CreateCrmOrder.ShiptoLine2;
                salesorder["shipto_city"] = CreateCrmOrder.ShiptoCity;
                salesorder["shipto_stateorprovince"] = CreateCrmOrder.ShiptoStateOrProvince;
                salesorder["shipto_country"] = CreateCrmOrder.ShiptoCountry;
                salesorder["shipto_postalcode"] = CreateCrmOrder.ShiptoPostalcode;

                List<CrmProduct> topProdcutList = CreateCrmOrder.CrmProductList;

                IEnumerable<IGrouping<string, CrmProduct>> query = topProdcutList.GroupBy(x => x.ManufacturerId);
                foreach (IGrouping<string, CrmProduct> info in query)
                {
                    List<CrmProduct> prodcutList = info.ToList<CrmProduct>();//分组后的集合

                    Entity vendor_account = RetriveAccountByManuFacturer(organizationProxy, prodcutList[0].ManufacturerId);
                    salesorder["new_vendor"] = new EntityReference("account", vendor_account.Id);

                    //Create Order
                    Guid orderId = organizationProxy.Create(salesorder);
                    log.Info("Create salesorder Success：" + orderId.ToString());


                    //2.Add prodcut to Order
                    
                    log.Info("ProductList Count：" + prodcutList.Count);
                    

                    #region Execute Multiple with Results
                    // Create an ExecuteMultipleRequest object.
                    ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
                    {
                        // Assign settings that define execution behavior: continue on error, return responses. 
                        Settings = new ExecuteMultipleSettings()
                        {
                            ContinueOnError = false,
                            ReturnResponses = true
                        },
                        // Create an empty organization request collection.
                        Requests = new OrganizationRequestCollection()
                    };

                    // Create several (local, in memory) entities in a collection. 
                    EntityCollection input = GetCollectionOfEntitiesToCreate(prodcutList, orderId, cu.uomid_value, organizationProxy);

                    // Add a CreateRequest for each entity to the request collection.
                    foreach (var entity in input.Entities)
                    {
                        CreateRequest createRequest = new CreateRequest { Target = entity };
                        requestWithResults.Requests.Add(createRequest);
                    }

                    // Execute all the requests in the request collection using a single web method call.
                    ExecuteMultipleResponse responseWithResults =
                        (ExecuteMultipleResponse)organizationProxy.Execute(requestWithResults);

                    // Display the results returned in the responses.
                    foreach (var responseItem in responseWithResults.Responses)
                    {
                        // A valid response.
                        if (responseItem.Response != null)
                        {
                            OrganizationRequest orq = requestWithResults.Requests[responseItem.RequestIndex];
                            OrganizationResponse ors = responseItem.Response;
                        }

                        // An error has occurred.
                        else if (responseItem.Fault != null)
                        {
                            OrganizationRequest orq = requestWithResults.Requests[responseItem.RequestIndex];
                            int idx = responseItem.RequestIndex;
                            OrganizationServiceFault fa = responseItem.Fault;
                        }
                    }
                    #endregion Execute Multiple with Results

                    #region Execute Multiple with No Results

                    ExecuteMultipleRequest requestWithNoResults = new ExecuteMultipleRequest()
                    {
                        // Set the execution behavior to not continue after the first error is received
                        // and to not return responses.
                        Settings = new ExecuteMultipleSettings()
                        {
                            ContinueOnError = false,
                            ReturnResponses = false
                        },
                        Requests = new OrganizationRequestCollection()
                    };

                    // Update the entities that were previously created.
                    EntityCollection update = GetCollectionOfEntitiesToUpdate(prodcutList, organizationProxy);

                    foreach (var entity in update.Entities)
                    {
                        UpdateRequest updateRequest = new UpdateRequest { Target = entity };
                        requestWithNoResults.Requests.Add(updateRequest);
                    }

                    ExecuteMultipleResponse responseWithNoResults =
                        (ExecuteMultipleResponse)organizationProxy.Execute(requestWithNoResults);

                    // There should be no responses unless there was an error. Only the first error 
                    // should be returned. That is the behavior defined in the settings.
                    if (responseWithNoResults.Responses.Count > 0)
                    {
                        foreach (var responseItem in responseWithNoResults.Responses)
                        {
                            if (responseItem.Fault != null)
                            {
                                OrganizationRequest orq = requestWithNoResults.Requests[responseItem.RequestIndex];
                                int ri = responseItem.RequestIndex;
                                OrganizationServiceFault fa = responseItem.Fault;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("All account records have been updated successfully.");
                    }

                    #endregion Execute Multiple with No Results



                    ////2.Add prodcut to Order
                    //log.Info("ProductList数量："+CreateCrmOrder.CrmProductList.Count);
                    //List<CrmProduct> prodcutList = CreateCrmOrder.CrmProductList;
                    //foreach (CrmProduct prd in prodcutList) 
                    //{
                    //    log.Info("Create salesorderdetail Begin：" + prd.ProductName);
                    //    Entity salesorderdetail = new Entity("salesorderdetail");
                    //    salesorderdetail["productid"] = new EntityReference("product", new Guid(prd.ProductName));
                    //    salesorderdetail["quantity"] = Decimal.Parse(prd.Quantity);
                    //    salesorderdetail["salesorderid"] = new EntityReference("salesorder", orderId);
                    //    //salesorderdetail["uomid"] = new EntityReference("uom", new Guid("CCFF5EBE-5407-4214-BD04-60461B7161AA"));
                    //    salesorderdetail["uomid"] = new EntityReference("uom", new Guid(cu.uomid_value));
                    //    //salesorderdetail["productid"] = prd.FreightAmount;
                    //    //salesorderdetail["productid"] = prd.AmountDiscount;
                    //    log.Info("Befor Create....");
                    //    Guid salesorderdetailId = organizationProxy.Create(salesorderdetail);
                    //    log.Info("创建salesorderdetail成功：" + salesorderdetailId.ToString());
                    //    //同时修改product的库存，用减法
                    //    Entity product = new Entity("product");
                    //    ColumnSet attributes = new ColumnSet(new string[] { "quantityonhand" });

                    //    product = organizationProxy.Retrieve(product.LogicalName, new Guid(prd.ProductName), attributes);
                    //    log.Info("Retrieved quantityonhand: " + product["quantityonhand"]);

                    //    decimal nowQuantity = (Decimal)product["quantityonhand"];
                    //    decimal newQuantity = nowQuantity - Decimal.Parse(prd.Quantity);
                    //    product["quantityonhand"] = newQuantity;
                    //    log.Info("newQuantity: " + newQuantity);

                    //    organizationProxy.Update(product);
                    //    log.Info("Update product quantity Success：" + prd.ProductName.ToString());
                    //}
                    //Add Free Shipping 4608DBBF-6E8F-E411-80DA-AC162DB4BC5C Flat Shipping Rate

                    //取消运费功能
                    //string isFreeShipping = CreateCrmOrder.IsFreeShipping;
                    //if(isFreeShipping.Equals("N"))//不免运费，加上运费的price list item
                    //{
                    //    Entity salesorderdetail = new Entity("salesorderdetail");
                    //    //salesorderdetail["productid"] = new EntityReference("product", new Guid("8698AE03-F994-E411-80E2-AC162DB41AC0"));
                    //    salesorderdetail["productid"] = new EntityReference("product", new Guid(cu.freeshipping_product_value));
                    //    salesorderdetail["quantity"] = Decimal.Parse("1");
                    //    salesorderdetail["salesorderid"] = new EntityReference("salesorder", orderId);
                    //    //salesorderdetail["uomid"] = new EntityReference("uom", new Guid("CCFF5EBE-5407-4214-BD04-60461B7161AA"));
                    //    salesorderdetail["uomid"] = new EntityReference("uom", new Guid(cu.uomid_value));
                    //    //salesorderdetail["productid"] = prd.FreightAmount;
                    //    //salesorderdetail["productid"] = prd.AmountDiscount;
                    //    Guid salesorderdetailId = organizationProxy.Create(salesorderdetail);
                    //    log.Info("创建运费成功：" + salesorderdetailId.ToString());
                    //}

                    Entity isExistOrder = new Entity("salesorder");
                    ColumnSet order_attributes = new ColumnSet(new string[] { "myce_taxratingid" });

                    isExistOrder = organizationProxy.Retrieve(isExistOrder.LogicalName, orderId, order_attributes);

                    isExistOrder["myce_taxratingid"] = new EntityReference("myce_taxrating", new Guid(cu.myce_taxratingid_value));
                    organizationProxy.Update(isExistOrder);

                    //fullfill salesorder
                    // FulfillSalesOrder(organizationProxy, orderId);

                    //create invoices
                    // CreateInvoice(organizationProxy, orderId);
                }

                





                response.ErrCode = "0";
                return response;
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return null;
        }

        

        /// <summary>
        /// Create a collection of new entity objects.
        /// </summary>
        /// <returns>A collection of entity objects.</returns>
        private EntityCollection GetCollectionOfEntitiesToCreate(List<CrmProduct> prodcutList,Guid orderId,string uomid_value, OrganizationServiceProxy organizationProxy)
        {
            EntityCollection collection = new EntityCollection()
            {
                EntityName = SalesOrderDetail.EntityLogicalName
            };
            foreach (CrmProduct prd in prodcutList)
            {
                log.Info("Create salesorderdetail Begin：" + prd.ProductName);
                Entity salesorderdetail = new Entity("salesorderdetail");
                salesorderdetail["productid"] = new EntityReference("product", new Guid(prd.ProductName));
                salesorderdetail["quantity"] = Decimal.Parse(prd.Quantity);
                salesorderdetail["salesorderid"] = new EntityReference("salesorder", orderId);
                salesorderdetail["uomid"] = new EntityReference("uom", new Guid(uomid_value));
                
                collection.Entities.Add(salesorderdetail);
                //Guid salesorderdetailId = organizationProxy.Create(salesorderdetail);
                //log.Info("创建salesorderdetail成功：" + salesorderdetailId.ToString());
            }
            return collection;
            
        }

        /// <summary>
        /// Create a collection of entity objects for updating. Give these entities a new
        /// name for the update.
        /// </summary>
        /// <returns>An entity collection.</returns>
        private EntityCollection GetCollectionOfEntitiesToUpdate(List<CrmProduct> prodcutList, OrganizationServiceProxy organizationProxy)
        {
            EntityCollection collection = new EntityCollection()
            {
                EntityName = Product.EntityLogicalName
            };
            foreach (CrmProduct prd in prodcutList)
            {
                //同时修改product的库存，用减法
                Entity product = new Entity("product");
                ColumnSet attributes = new ColumnSet(new string[] { "productid","quantityonhand" });

                product = organizationProxy.Retrieve(product.LogicalName, new Guid(prd.ProductName), attributes);
                log.Info("Retrieved quantityonhand: " + product["quantityonhand"]);

                decimal nowQuantity = (Decimal)product["quantityonhand"];
                decimal newQuantity = nowQuantity - Decimal.Parse(prd.Quantity);
                product["quantityonhand"] = newQuantity;
                log.Info("newQuantity: " + newQuantity);

                collection.Entities.Add(product);
                //organizationProxy.Update(product);
            }
            return collection;
        }

        /// <summary>
        /// Create a collection of new entity objects.
        /// </summary>
        /// <returns>A collection of entity objects.</returns>
        private EntityCollection GetCollectionOfEntitiesToCreate2(List<AppCrmOrderProduct> prodcutList, Guid orderId, string uomid_value, OrganizationServiceProxy organizationProxy)
        {
            EntityCollection collection = new EntityCollection()
            {
                EntityName = SalesOrderDetail.EntityLogicalName
            };
            foreach (AppCrmOrderProduct prd in prodcutList)
            {
                log.Info("Create salesorderdetail Begin：" + prd.ProductId);
                Entity salesorderdetail = new Entity("salesorderdetail");
                salesorderdetail["productid"] = new EntityReference("product", new Guid(prd.ProductId));
                salesorderdetail["quantity"] = Decimal.Parse(prd.Quantity);
                salesorderdetail["salesorderid"] = new EntityReference("salesorder", orderId);
                salesorderdetail["uomid"] = new EntityReference("uom", new Guid(uomid_value));

                collection.Entities.Add(salesorderdetail);
                //Guid salesorderdetailId = organizationProxy.Create(salesorderdetail);
                //log.Info("创建salesorderdetail成功：" + salesorderdetailId.ToString());
            }
            return collection;

        }

        /// <summary>
        /// Create a collection of entity objects for updating. Give these entities a new
        /// name for the update.
        /// </summary>
        /// <returns>An entity collection.</returns>
        private EntityCollection GetCollectionOfEntitiesToUpdate2(List<AppCrmOrderProduct> prodcutList, OrganizationServiceProxy organizationProxy)
        {
            EntityCollection collection = new EntityCollection()
            {
                EntityName = Product.EntityLogicalName
            };
            foreach (AppCrmOrderProduct prd in prodcutList)
            {
                //同时修改product的库存，用减法
                Entity product = new Entity("product");
                ColumnSet attributes = new ColumnSet(new string[] { "productid", "quantityonhand" });

                product = organizationProxy.Retrieve(product.LogicalName, new Guid(prd.ProductId), attributes);
                log.Info("Retrieved quantityonhand: " + product["quantityonhand"]);

                decimal nowQuantity = (Decimal)product["quantityonhand"];
                decimal newQuantity = nowQuantity - Decimal.Parse(prd.Quantity);
                product["quantityonhand"] = newQuantity;
                log.Info("newQuantity: " + newQuantity);

                collection.Entities.Add(product);
                //organizationProxy.Update(product);
            }
            return collection;
        }

        //private void CloseSalesOrder(OrganizationServiceProxy service,Guid salesorderId)
        //{
        //    SetStateRequest setStateReq = new SetStateRequest();
        //    setStateReq.EntityMoniker = new EntityReference("salesorder", salesorderId);
        //    setStateReq.State = new OptionSetValue(1);
        //    setStateReq.Status = new OptionSetValue(100001);

        //    SetStateResponse response = (SetStateResponse)service.Execute(setStateReq);
        //}

        private void FulfillSalesOrder(OrganizationServiceProxy service, Guid salesorderId)
        {
            FulfillSalesOrderRequest req = new FulfillSalesOrderRequest();
            OrderClose orderClose = new OrderClose();
            orderClose.SalesOrderId = new EntityReference("salesorder", salesorderId);
            orderClose.Subject = "Sales Order Closed";
            req.OrderClose = orderClose;
            OptionSetValue o = new OptionSetValue();
            o.Value = 100001;
            req.Status = o;
            FulfillSalesOrderResponse resp = (FulfillSalesOrderResponse)service.Execute(req);
        }

        private Entity RetriveRecord(OrganizationServiceProxy service, String recordId)
        {
            QueryByAttribute querybyattribute = new QueryByAttribute("account");
            querybyattribute.ColumnSet = new ColumnSet(true);

            //  Attribute to query.
            querybyattribute.Attributes.AddRange("name");

            //  Value of queried attribute to return.
            querybyattribute.Values.AddRange(recordId);

            //  Query passed to service proxy.
            EntityCollection retrieved = service.RetrieveMultiple(querybyattribute);
            foreach (Entity entity in retrieved.Entities)
            {
                return entity;
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

        private Entity RetriveAccountByManuFacturer(OrganizationServiceProxy service, String manuFacturerId)
        {
            QueryByAttribute querybyattribute = new QueryByAttribute("account");
            querybyattribute.ColumnSet = new ColumnSet(true);

            //  Attribute to query.
            querybyattribute.Attributes.AddRange("new_manufacturerid");

            //  Value of queried attribute to return.
            querybyattribute.Values.AddRange(manuFacturerId);

            //  Query passed to service proxy.
            EntityCollection retrieved = service.RetrieveMultiple(querybyattribute);
            foreach (Entity entity in retrieved.Entities)
            {
                return entity;
            }
            return null;
        }
        

        private Entity CreateInvoice(OrganizationServiceProxy service, Guid salesorderId)
        {
            ColumnSet invoiceColumns = new ColumnSet("invoiceid", "totalamount");

            // Convert the sales order to an Invoice
            ConvertSalesOrderToInvoiceRequest req =
                new ConvertSalesOrderToInvoiceRequest()
                {
                    SalesOrderId = salesorderId,
                    ColumnSet = invoiceColumns
                };
            ConvertSalesOrderToInvoiceResponse resp =(ConvertSalesOrderToInvoiceResponse)service.Execute(req);
            Invoice invoice = (Invoice)resp.Entity;
            return invoice;
        }

        public AppCrmOrderResponse AppCreateOrder(AppCrmOrderRequest orderRequest)
        {
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            try
            {
                
                string email = orderRequest.email;  
                log.Info("order-request：" + email);
                List<AppCrmOrderProduct> prodcutList = orderRequest.product;
                log.Info("order-request product:" + "----" + prodcutList.Count);

                string datetime_now = DateTime.Now.ToString("yyyyMMddHHmmss");

                Entity salesorder = new Entity("salesorder");
                

                Entity email_account = RetriveAccountByEmail(organizationProxy, email);
                salesorder["customerid"] = new EntityReference("account", email_account.Id);

                salesorder["pricelevelid"] = new EntityReference("pricelevel", new Guid(cu.pricelevelid_value));
                //salesorder["pricelevelid"] = new EntityReference("pricelevel", new Guid("780D3E7B-D6F9-E211-8ABD-984BE17C68D3"));

                //AN000001
                string new_orderno = this.readAppOrderNoFromXML();
                salesorder["name"] = "AN" + new_orderno;
                salesorder["new_ordernumber"] = "PO" + new_orderno;
                Int32 exit_orderno = Int32.Parse(new_orderno);
                exit_orderno += 1;
                string new_exit_orderno = exit_orderno.ToString().PadLeft(6, '0');
                this.writeAppOrderNoToXML(new_exit_orderno);

                //From Mobile
                OptionSetValue orderfrom = new OptionSetValue(100000001);
                salesorder["new_orderfrom"] =  orderfrom;

                //Payment
                OptionSetValue payment = new OptionSetValue(Int32.Parse(orderRequest.payment));
                salesorder["paymenttermscode"] = payment;

                //contact time
                salesorder["new_requestedringback"] = DateTime.Parse(orderRequest.contactTime);
                

                //OptionSetValue pv = new OptionSetValue(1);
                //salesorder["paymenttermscode"] = pv;
                //salesorder["billto_name"] = CreateCrmOrder.BilltoName;
                //salesorder["billto_line1"] = CreateCrmOrder.BilltoLine1;
                //salesorder["billto_line2"] = CreateCrmOrder.BilltoLine2;
                //salesorder["billto_city"] = CreateCrmOrder.BilltoCity;
                //salesorder["billto_stateorprovince"] = CreateCrmOrder.BilltoStateOrProvince;
                //salesorder["billto_country"] = CreateCrmOrder.BilltoCountry;
                //salesorder["billto_postalcode"] = CreateCrmOrder.BilltoPostalcode;
                //salesorder["shipto_line1"] = CreateCrmOrder.ShiptoLine1;
                //salesorder["shipto_line2"] = CreateCrmOrder.ShiptoLine2;
                //salesorder["shipto_city"] = CreateCrmOrder.ShiptoCity;
                //salesorder["shipto_stateorprovince"] = CreateCrmOrder.ShiptoStateOrProvince;
                //salesorder["shipto_country"] = CreateCrmOrder.ShiptoCountry;
                //salesorder["shipto_postalcode"] = CreateCrmOrder.ShiptoPostalcode;

                Guid orderId = organizationProxy.Create(salesorder);
                log.Info("创建salesorder成功：" + orderId.ToString());


                //2.Add prodcut to Order
                log.Info("ProductList数量：" + prodcutList.Count);
                #region Execute Multiple with Results
                // Create an ExecuteMultipleRequest object.
                ExecuteMultipleRequest requestWithResults = new ExecuteMultipleRequest()
                {
                    // Assign settings that define execution behavior: continue on error, return responses. 
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = false,
                        ReturnResponses = true
                    },
                    // Create an empty organization request collection.
                    Requests = new OrganizationRequestCollection()
                };

                // Create several (local, in memory) entities in a collection. 
                EntityCollection input = GetCollectionOfEntitiesToCreate2(prodcutList, orderId, cu.uomid_value, organizationProxy);

                // Add a CreateRequest for each entity to the request collection.
                foreach (var entity in input.Entities)
                {
                    CreateRequest createRequest = new CreateRequest { Target = entity };
                    requestWithResults.Requests.Add(createRequest);
                }

                // Execute all the requests in the request collection using a single web method call.
                ExecuteMultipleResponse responseWithResults =
                    (ExecuteMultipleResponse)organizationProxy.Execute(requestWithResults);

                // Display the results returned in the responses.
                foreach (var responseItem in responseWithResults.Responses)
                {
                    // A valid response.
                    if (responseItem.Response != null)
                    {
                        OrganizationRequest orq = requestWithResults.Requests[responseItem.RequestIndex];
                        OrganizationResponse ors = responseItem.Response;
                    }

                    // An error has occurred.
                    else if (responseItem.Fault != null)
                    {
                        OrganizationRequest orq = requestWithResults.Requests[responseItem.RequestIndex];
                        int idx = responseItem.RequestIndex;
                        OrganizationServiceFault fa = responseItem.Fault;
                    }
                }
                #endregion Execute Multiple with Results

                #region Execute Multiple with No Results

                ExecuteMultipleRequest requestWithNoResults = new ExecuteMultipleRequest()
                {
                    // Set the execution behavior to not continue after the first error is received
                    // and to not return responses.
                    Settings = new ExecuteMultipleSettings()
                    {
                        ContinueOnError = false,
                        ReturnResponses = false
                    },
                    Requests = new OrganizationRequestCollection()
                };

                // Update the entities that were previously created.
                EntityCollection update = GetCollectionOfEntitiesToUpdate2(prodcutList, organizationProxy);

                foreach (var entity in update.Entities)
                {
                    UpdateRequest updateRequest = new UpdateRequest { Target = entity };
                    requestWithNoResults.Requests.Add(updateRequest);
                }

                ExecuteMultipleResponse responseWithNoResults =
                    (ExecuteMultipleResponse)organizationProxy.Execute(requestWithNoResults);

                // There should be no responses unless there was an error. Only the first error 
                // should be returned. That is the behavior defined in the settings.
                if (responseWithNoResults.Responses.Count > 0)
                {
                    foreach (var responseItem in responseWithNoResults.Responses)
                    {
                        if (responseItem.Fault != null)
                        {
                            OrganizationRequest orq = requestWithNoResults.Requests[responseItem.RequestIndex];
                            int ri = responseItem.RequestIndex;
                            OrganizationServiceFault fa = responseItem.Fault;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("All account records have been updated successfully.");
                }

                #endregion Execute Multiple with No Results


                //Add Free Shipping 4608DBBF-6E8F-E411-80DA-AC162DB4BC5C Flat Shipping Rate
                //string isFreeShipping = CreateCrmOrder.IsFreeShipping;
                //if(isFreeShipping.Equals("N"))//不免运费，加上运费的price list item
                //{
                //    Entity salesorderdetail = new Entity("salesorderdetail");
                //    //salesorderdetail["productid"] = new EntityReference("product", new Guid("8698AE03-F994-E411-80E2-AC162DB41AC0"));
                //    salesorderdetail["productid"] = new EntityReference("product", new Guid("85ACAB90-7248-E411-9212-D89D67639EF0"));
                //    salesorderdetail["quantity"] = Decimal.Parse("1");
                //    salesorderdetail["salesorderid"] = new EntityReference("salesorder", orderId);
                //    //salesorderdetail["uomid"] = new EntityReference("uom", new Guid("CCFF5EBE-5407-4214-BD04-60461B7161AA"));
                //    salesorderdetail["uomid"] = new EntityReference("uom", new Guid("3E055750-D6F9-E211-8ABD-984BE17C68D3"));
                //    //salesorderdetail["productid"] = prd.FreightAmount;
                //    //salesorderdetail["productid"] = prd.AmountDiscount;
                //    Guid salesorderdetailId = organizationProxy.Create(salesorderdetail);
                //    log.Info("创建运费成功：" + salesorderdetailId.ToString());
                //}

                //Entity isExistOrder = new Entity("salesorder");
                //ColumnSet order_attributes = new ColumnSet(new string[] { "myce_taxratingid" });

                //isExistOrder = organizationProxy.Retrieve(isExistOrder.LogicalName, orderId, order_attributes);

                //isExistOrder["myce_taxratingid"] = new EntityReference("myce_taxrating", new Guid("99596F96-054E-E411-9212-D89D67639EF0"));
                //organizationProxy.Update(isExistOrder);

                AppCrmOrderResponse rs = new AppCrmOrderResponse();
                rs.orderId = orderId.ToString();
                return rs;
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return null;

        }

        private string readAppOrderNoFromXML()
        {
            string xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "/conf/AppOrderNo.xml";
            XmlNodeReader reader = null;
            string s = "", v = "";
            try
            {
                
                XmlDocument doc = new XmlDocument();
                // 装入指定的XML文档
                doc.Load(xmlFilePath);
                
                // 设定XmlNodeReader对象来打开XML文件
                reader = new XmlNodeReader(doc);
                // 读取XML文件中的数据，并显示出来
                while (reader.Read())
                {
                    //判断当前读取得节点类型
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            s = reader.Name;
                            break;
                        case XmlNodeType.Text:
                            {
                                if (s.Equals("Name"))
                                    v = reader.Value;
                                else
                                    v = reader.Value;
                            }
                            break;
                    }
                }
            }
            finally
            {
                //清除打开的数据流
                if (reader != null)
                    reader.Close();
            }
            return v;
        }

        private void writeAppOrderNoToXML(string orderNo)
        {
            string xmlFilePath = AppDomain.CurrentDomain.BaseDirectory + "/conf/AppOrderNo.xml";
          
            XmlDocument doc = new XmlDocument();
            // 装入指定的XML文档
            doc.Load(xmlFilePath);

            XmlNode node =  doc.SelectSingleNode("orderNo");
            XmlElement xe = (XmlElement)node;//将子节点类型转换为XmlElement类型
            xe.InnerText = orderNo;//则修改
            doc.Save(AppDomain.CurrentDomain.BaseDirectory + "/conf/AppOrderNo.xml");//保存。
        }

        public ExportCrmOrder getOrderInfoById(Guid orderId)
        {
            ExportCrmOrder crmOrder = new ExportCrmOrder();
            List<ExportCrmOrderProduct> crmOrderProduct = new List<ExportCrmOrderProduct>();
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            try
            {
                Entity isExistOrder = new Entity("salesorder");
                ColumnSet order_attributes = new ColumnSet(true);

                isExistOrder = organizationProxy.Retrieve(isExistOrder.LogicalName, orderId, order_attributes);

                crmOrder.OrderId = orderId.ToString();
                crmOrder.BillToName = isExistOrder.GetAttributeValue<string>("shipto_name");
                crmOrder.Street1 = isExistOrder.GetAttributeValue<string>("shipto_line1");
                crmOrder.Street2 = isExistOrder.GetAttributeValue<string>("shipto_line2");
                crmOrder.City = isExistOrder.GetAttributeValue<string>("shipto_city");
                crmOrder.PostCode = isExistOrder.GetAttributeValue<string>("shipto_postalcode");
                crmOrder.OrderNo = isExistOrder.GetAttributeValue<string>("name");
                crmOrder.RefNo = isExistOrder.GetAttributeValue<string>("new_pickinglistnumber");
                crmOrder.ShipDate = isExistOrder.GetAttributeValue<DateTime>("new_pickingdate").ToLocalTime().ToString("dd/MM/yyyy");
                if (isExistOrder.Contains("shippingmethodcode") && isExistOrder["shippingmethodcode"] != null)
                {
                    crmOrder.ShipVia = isExistOrder.FormattedValues["shippingmethodcode"];
                }
                crmOrder.Note = isExistOrder.GetAttributeValue<string>("new_packinglistnote");


                //Query Product by OrderId
                QueryExpression qe = new QueryExpression();
                qe.EntityName = "salesorderdetail";
                qe.ColumnSet = new ColumnSet();
                qe.ColumnSet.Columns.Add("quantity");
                qe.ColumnSet.Columns.Add("productid");

                ConditionExpression condition1 = new ConditionExpression();
                condition1.AttributeName = "salesorderid";
                condition1.Operator = ConditionOperator.Equal;
                condition1.Values.Add(orderId);        

                FilterExpression filter1 = new FilterExpression();
                filter1.Conditions.Add(condition1);

                qe.Criteria.AddFilter(filter1);

                qe.LinkEntities.Add(new LinkEntity("salesorderdetail", "product", "productid", "productid", JoinOperator.Inner));
                qe.LinkEntities[0].Columns.AddColumns("new_stocklocation", "name", "new_stockstatus", "new_suppliera1","producttypecode","currentcost");
                qe.LinkEntities[0].EntityAlias = "orderproduct";


                //  Query passed to service proxy.
                EntityCollection retrieved = organizationProxy.RetrieveMultiple(qe);
                foreach (Entity entity in retrieved.Entities)
                {
                    bool isProduct = true;

                    //Product Status is Service Rate 10
                    if (entity.GetAttributeValue<AliasedValue>("orderproduct.producttypecode") != null)
                    {
                        OptionSetValue op = (OptionSetValue)(entity.GetAttributeValue<AliasedValue>("orderproduct.producttypecode").Value);

                        int ov = op.Value;
                        if (ov.Equals(10))
                        {
                            isProduct = false;
                        }
                    }

                    if (isProduct)
                    {
                        ExportCrmOrderProduct product = new ExportCrmOrderProduct();
                        if (entity.Contains("orderproduct.new_stocklocation") && entity["orderproduct.new_stocklocation"] != null)
                        {
                            product.Location = entity.GetAttributeValue<AliasedValue>("orderproduct.new_stocklocation").Value.ToString();

                        }
                        if (entity.Contains("orderproduct.currentcost") && entity["orderproduct.currentcost"] != null)
                        {
                            Money cost = (Money)(entity.GetAttributeValue<AliasedValue>("orderproduct.currentcost").Value);
                            product.LandedCost = cost.Value.ToString("0.00");

                        }

                        //Non Stock-->1,Purchased Stock-->2,Third Party Stock-->3
                        if (entity.GetAttributeValue<AliasedValue>("orderproduct.new_stockstatus") != null)
                        {
                            OptionSetValue op = (OptionSetValue)(entity.GetAttributeValue<AliasedValue>("orderproduct.new_stockstatus").Value);

                            //string text = entity.FormattedValues.Where(a => a.Key == "orderproduct.new_stockstatus").FirstOrDefault().Value;
                            product.StockStatus = op.Value.ToString();
                        }
                        product.ProductName = entity.GetAttributeValue<AliasedValue>("orderproduct.name").Value.ToString();
                        product.QtyOrdered = entity.GetAttributeValue<decimal>("quantity").ToString("#");
                        if (entity.Contains("orderproduct.new_suppliera1") && entity["orderproduct.new_suppliera1"] != null)
                        {
                            EntityReference supplier_a = (EntityReference)(entity.GetAttributeValue<AliasedValue>("orderproduct.new_suppliera1").Value);
                            product.SupplierA = supplier_a.Name;
                        }

                        crmOrderProduct.Add(product);
                    }
                    
                }
                //排序
                List<ExportCrmOrderProduct> SortedList = crmOrderProduct.OrderBy(o => o.StockStatus).ThenBy(o => o.Location).ToList();
                crmOrder.products = SortedList;
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return crmOrder;
        }

        public List<AppCrmOrderHistoryResponse> AppGetOrderHistory( AppCrmOrderHistoryRequest orderHistoryRequest)
        {
            
            log.Info("AppGetOrderHistory：" + orderHistoryRequest.email);
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            List<AppCrmOrderHistoryResponse> response_s = new List<AppCrmOrderHistoryResponse>();
            
            try
            {
                //Entity account = RetriveAccountByEmail(organizationProxy, orderHistoryRequest.email);
                
                QueryExpression qe = new QueryExpression();
                qe.EntityName = "salesorder";
                qe.ColumnSet = new ColumnSet(true);


                ConditionExpression condition11 = new ConditionExpression();
                condition11.AttributeName = "emailaddress1";
                condition11.Operator = ConditionOperator.Equal;
                condition11.Values.Add(orderHistoryRequest.email);

                FilterExpression filter11 = new FilterExpression();
                filter11.Conditions.Add(condition11);

                string beginDate = orderHistoryRequest.beginDate;
                string endDate = orderHistoryRequest.endDate;
                if ((beginDate != null && !beginDate.Equals("")) && (endDate != null && !endDate.Equals(""))) 
                {
                    ConditionExpression condition12 = new ConditionExpression("createdon", ConditionOperator.Between, beginDate, endDate);

                    FilterExpression filter12 = new FilterExpression();
                    filter12.Conditions.Add(condition12);

                    qe.Criteria.AddFilter(filter12);
                }
                
                //ConditionExpression condition12 = new ConditionExpression();
                //condition12.AttributeName = "createdon";
                //condition12.Operator = ConditionOperator.Between;
                //condition12.Values.Add(new string[] { orderHistoryRequest.beginDate, orderHistoryRequest.endDate });




                qe.LinkEntities.Add(new LinkEntity("salesorder", "account", "customerid", "accountid", JoinOperator.Inner));
                qe.LinkEntities[0].Columns.AddColumns("accountid");
                qe.LinkEntities[0].EntityAlias = "customer";
                qe.LinkEntities[0].LinkCriteria.AddFilter(filter11);



                //  Query passed to service proxy.
                EntityCollection retrieved = organizationProxy.RetrieveMultiple(qe);
                foreach (Entity salseorder in retrieved.Entities)
                {
                    AppCrmOrderHistoryResponse res = new AppCrmOrderHistoryResponse();
                    res.orderName = salseorder.GetAttributeValue<string>("name");
                    if (salseorder.Contains("totalamount") && salseorder["totalamount"] != null)
                    {
                        decimal totalRRP = 0;
                        res.totalAmount = salseorder.GetAttributeValue<Money>("totalamount").Value.ToString("0.00");

                        res.createdOn = salseorder.GetAttributeValue<DateTime>("createdon").ToLocalTime().ToString("yyyyMMddHHmmss");

                        List<AppCrmOrderHistoryProduct> res_products = new List<AppCrmOrderHistoryProduct>();
                        Guid orderId = salseorder.GetAttributeValue<Guid>("salesorderid");

                        QueryExpression qe2 = new QueryExpression();
                        qe2.EntityName = "salesorderdetail";
                        qe2.ColumnSet = new ColumnSet(true);
                        //qe2.ColumnSet.Columns.Add("quantity");
                        //qe2.ColumnSet.Columns.Add("priceperunit");
                        //qe2.ColumnSet.Columns.Add("baseamount");
                        //qe2.ColumnSet.Columns.Add("extendedamount");
                        //qe2.ColumnSet.Columns.Add("productid");

                        ConditionExpression condition2 = new ConditionExpression();
                        condition2.AttributeName = "salesorderid";
                        condition2.Operator = ConditionOperator.Equal;
                        condition2.Values.Add(orderId);

                        FilterExpression filter2 = new FilterExpression();
                        filter2.Conditions.Add(condition2);

                        qe2.Criteria.AddFilter(filter2);

                        
                        //  Query passed to service proxy.
                        EntityCollection retrieved2 = organizationProxy.RetrieveMultiple(qe2);
                        foreach (Entity entity in retrieved2.Entities)
                        {
                            bool isProduct = true;

                            //Product Status is Service Rate 10
                            //if (entity.GetAttributeValue<AliasedValue>("orderproduct.producttypecode") != null)
                            //{
                            //    OptionSetValue op = (OptionSetValue)(entity.GetAttributeValue<AliasedValue>("orderproduct.producttypecode").Value);

                            //    int ov = op.Value;
                            //    if (ov.Equals(10))
                            //    {
                            //        isProduct = false;
                            //    }
                            //}

                            if (isProduct)
                            {
                                AppCrmOrderHistoryProduct product = new AppCrmOrderHistoryProduct();

                                if (entity.Contains("productid") && entity["productid"] != null)
                                {
                                    EntityReference pro = (EntityReference)entity["productid"];
                                    product.ProductId = pro.Id.ToString();
                                    product.ProductSku = pro.Name;

                                    Entity product_entity = new Entity("product");
                                    ColumnSet attributes = new ColumnSet(new string[] { "price", "new_rrp" });

                                    product_entity = organizationProxy.Retrieve(product_entity.LogicalName, pro.Id, attributes);
                                    if (product_entity.Contains("price") && product_entity["price"] != null)
                                    {
                                        product.PriceList = product_entity.GetAttributeValue<Money>("price").Value.ToString("#.##");
                                    }
                                    if (product_entity.Contains("new_rrp") && product_entity["new_rrp"] != null)
                                    {
                                        product.RRP = product_entity.GetAttributeValue<Money>("new_rrp").Value.ToString("#.##");
                                        totalRRP += (product_entity.GetAttributeValue<Money>("new_rrp").Value) * (entity.GetAttributeValue<decimal>("quantity"));
                                    }

                                }
                                product.Quantity = entity.GetAttributeValue<decimal>("quantity").ToString("#");
                                
                                res_products.Add(product);
                            }
                        }
                        res.totalRRP = totalRRP.ToString("0.00");
                        res.product = res_products;
                    }
                    response_s.Add(res);
                }
                return response_s;

            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }

            return response_s;

        }



    }
}
