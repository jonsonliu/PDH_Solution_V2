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
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace PDH_CrmService.CrmInterface
{
    
    public class ProductOperation
    {
        log4net.ILog log = log4net.LogManager.GetLogger("fileLog");//获取一个日志记录器
        public UpdateProductQuantityResponse UpdateProductQuantity(String productId, String quantity)
        {
            log.Info("CMS-->Product Quantity：" + quantity);
            UpdateProductQuantityResponse response = new UpdateProductQuantityResponse();
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();

            try
            {
            //Update Account TradeRegistration
            Entity product = new Entity("product");
            ColumnSet attributes = new ColumnSet(new string[] { "quantityonhand" });

            product = organizationProxy.Retrieve(product.LogicalName, new Guid(productId), attributes);
            log.Info("Retrieved quantityonhand: " + product["quantityonhand"]);

            //OptionSetValue register = new OptionSetValue(1);

            //account.Attributes.Add("new_check",  register);
            product["quantityonhand"] = Decimal.Parse(quantity); ;


            organizationProxy.Update(product);



            response.ErrCode = "0";
            return response;
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return null;
        }

        public AnalysisCrmProduct AnalysisGetProductById(String productId)
        {
            log.Info("AnalysisGetProductById==" + productId);
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            AnalysisCrmProduct res_product = new AnalysisCrmProduct();
            try
            {
                //Update Account TradeRegistration
                Entity product = new Entity("product");
                ColumnSet attributes = new ColumnSet(true);

                product = organizationProxy.Retrieve(product.LogicalName, new Guid(productId), attributes);

                res_product.ProductName = product.GetAttributeValue<string>("productnumber");
                res_product.ProductSku = product.GetAttributeValue<string>("name");

                if (product.Contains("producttypecode") && product["producttypecode"] != null)
                {
                    res_product.ProductStatus = product.FormattedValues["producttypecode"];
                }
                if (product.Contains("defaultuomid") && product["defaultuomid"] != null)
                {
                    EntityReference unit = (EntityReference)product["defaultuomid"];
                    res_product.Unit = unit.Name;
                }

                res_product.DecimalsSupported = product.GetAttributeValue<int>("quantitydecimal").ToString();

                if (product.Contains("pricelevelid") && product["pricelevelid"] != null)
                {
                    EntityReference pricelist = (EntityReference)product["pricelevelid"];
                    res_product.PriceList = pricelist.Name;
                }
                if (product.Contains("new_suppliera1") && product["new_suppliera1"] != null)
                {
                    EntityReference supplier_a = (EntityReference)product["new_suppliera1"];
                    res_product.SupplierA = supplier_a.Name;
                }
                res_product.Brand = product.GetAttributeValue<string>("suppliername");

                res_product.Quantity = product.GetAttributeValue<decimal>("quantityonhand").ToString("#.##");

                res_product.BrandedPackage = product.GetAttributeValue<bool>("new_brandedpackage").ToString();
                res_product.PetDreamHouseURL =  product.GetAttributeValue<string>("producturl");
                
                if (product.Contains("price") && product["price"] != null)
                {
                    res_product.ListPrice = product.GetAttributeValue<Money>("price").Value.ToString("#.##");
                }

                if (product.Contains("new_rrp") && product["new_rrp"] != null)
                {
                    res_product.RRP = product.GetAttributeValue<Money>("new_rrp").Value.ToString("#.##");
                }

                res_product.ProductColor = product.GetAttributeValue<string>("new_colour");
                res_product.ProductBarcode =product.GetAttributeValue<string>("new_productbarcode");
                res_product.ProductDimensions = product.GetAttributeValue<string>("new_size");
                res_product.ProductWeight =  product.GetAttributeValue<string>("new_productweight");
                res_product.IdenticalUnitPerCase =product.GetAttributeValue<string>("new_casequantity");
                res_product.CaseBarcode = product.GetAttributeValue<string>("new_casebarcode");
                res_product.CaseDimensions =  product.GetAttributeValue<string>("new_casedimensions");
                res_product.CaseWeight =  product.GetAttributeValue<string>("new_caseweight");
                res_product.CTNDetails =  product.GetAttributeValue<string>("new_ctndetails");

                if (product.Contains("new_parentcatalog") && product["new_parentcatalog"] != null)
                {
                    res_product.ParentCatalog = product.FormattedValues["new_parentcatalog"];
                }
                if (product.Contains("new_subcatelog") && product["new_subcatelog"] != null)
                {
                    res_product.SubCatalog = product.FormattedValues["new_subcatelog"];
                }
                if (product.Contains("new_additionalcatelog") && product["new_additionalcatelog"] != null)
                {
                    res_product.AdditionalCatalog = product.FormattedValues["new_additionalcatelog"];
                }

                res_product.ProductDescription =  product.GetAttributeValue<string>("description");
                res_product.UnitBarCode = product.GetAttributeValue<string>("new_barcode");
                res_product.UnitWeight_KG =  product.GetAttributeValue<decimal>("new_grossunitweight").ToString("#.##");
                res_product.UnitDepthLength_CM =  product.GetAttributeValue<decimal>("new_unitdepthcm").ToString("#.##");
                res_product.UnitWidthGirth_CM =  product.GetAttributeValue<decimal>("new_unitwidthcm").ToString("#.##");
                res_product.UnitHeightNeck_CM = product.GetAttributeValue<decimal>("new_unitheight").ToString("#.##");
                res_product.CartonBarcode =  product.GetAttributeValue<string>("new_mastercartonbarcode");
                res_product.UnitsPerCarton = product.GetAttributeValue<string>("new_packspermasterbox");
                res_product.CartonWeight_KG =  product.GetAttributeValue<decimal>("new_mastercartonweightkg").ToString("#.##");
                res_product.CartonDepth_CM =  product.GetAttributeValue<decimal>("new_mastercartondepthcm").ToString("#.##");
                res_product.CartonWidth_CM =  product.GetAttributeValue<decimal>("new_mastercartonwidthcm").ToString("#.##");
                res_product.CartonHeight_CM = product.GetAttributeValue<decimal>("new_mastercartonheightcm").ToString("#.##");
                res_product.CreatedOn = product.GetAttributeValue<DateTime>("createdon").ToLocalTime().ToString("yyyyMMddHHmmss");

                return res_product;
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return null;
        }

        public List<AnalysisCrmProduct> AnalysisGetAllProduct()
        {
            log.Info("AnalysisGetAllProduct：");
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            List<AnalysisCrmProduct> products = new List<AnalysisCrmProduct>();
            
            try
            {
                //Update Account TradeRegistration
                QueryExpression qe = new QueryExpression();
                qe.EntityName = "product";
                qe.ColumnSet = new ColumnSet(true);
                //qe.ColumnSet.Columns.Add("name");
                //qe.ColumnSet.Columns.Add("quantityonhand");

                //qe.LinkEntities.Add(new LinkEntity("account", "contact", "primarycontactid", "contactid", JoinOperator.Inner));
                //qe.LinkEntities[0].Columns.AddColumns("firstname", "lastname");
                //qe.LinkEntities[0].EntityAlias = "primarycontact";

                EntityCollection ec = organizationProxy.RetrieveMultiple(qe);

                foreach (Entity product in ec.Entities)
                {
                    AnalysisCrmProduct res_product = new AnalysisCrmProduct();

                    res_product.ProductId = product.GetAttributeValue<Guid>("productid").ToString();
                    res_product.ProductName = product.GetAttributeValue<string>("productnumber");
                    res_product.ProductSku = product.GetAttributeValue<string>("name");

                    if (product.Contains("producttypecode") && product["producttypecode"] != null)
                    {
                        res_product.ProductStatus = product.FormattedValues["producttypecode"];
                    }
                    if (product.Contains("defaultuomid") && product["defaultuomid"] != null)
                    {
                        EntityReference unit = (EntityReference)product["defaultuomid"];
                        res_product.Unit = unit.Name;
                    }

                    res_product.DecimalsSupported = product.GetAttributeValue<int>("quantitydecimal").ToString();

                    if (product.Contains("pricelevelid") && product["pricelevelid"] != null)
                    {
                        EntityReference pricelist = (EntityReference)product["pricelevelid"];
                        res_product.PriceList = pricelist.Name;
                    }
                    if (product.Contains("new_suppliera1") && product["new_suppliera1"] != null)
                    {
                        EntityReference supplier_a = (EntityReference)product["new_suppliera1"];
                        res_product.SupplierA = supplier_a.Name;
                    }
                    res_product.Brand = product.GetAttributeValue<string>("suppliername");

                    res_product.Quantity = product.GetAttributeValue<decimal>("quantityonhand").ToString("#.##");

                    res_product.BrandedPackage = product.GetAttributeValue<bool>("new_brandedpackage").ToString();
                    res_product.PetDreamHouseURL = product.GetAttributeValue<string>("producturl");

                    if (product.Contains("price") && product["price"] != null)
                    {
                        res_product.ListPrice = product.GetAttributeValue<Money>("price").Value.ToString("#.##");
                    }

                    if (product.Contains("new_rrp") && product["new_rrp"] != null)
                    {
                        res_product.RRP = product.GetAttributeValue<Money>("new_rrp").Value.ToString("#.##");
                    }

                    res_product.ProductColor = product.GetAttributeValue<string>("new_colour");
                    res_product.ProductBarcode = product.GetAttributeValue<string>("new_productbarcode");
                    res_product.ProductDimensions = product.GetAttributeValue<string>("new_size");
                    res_product.ProductWeight = product.GetAttributeValue<string>("new_productweight");
                    res_product.IdenticalUnitPerCase = product.GetAttributeValue<string>("new_casequantity");
                    res_product.CaseBarcode = product.GetAttributeValue<string>("new_casebarcode");
                    res_product.CaseDimensions = product.GetAttributeValue<string>("new_casedimensions");
                    res_product.CaseWeight = product.GetAttributeValue<string>("new_caseweight");
                    res_product.CTNDetails = product.GetAttributeValue<string>("new_ctndetails");

                    if (product.Contains("new_parentcatalog") && product["new_parentcatalog"] != null)
                    {
                        res_product.ParentCatalog = product.FormattedValues["new_parentcatalog"];
                    }
                    if (product.Contains("new_subcatelog") && product["new_subcatelog"] != null)
                    {
                        res_product.SubCatalog = product.FormattedValues["new_subcatelog"];
                    }
                    if (product.Contains("new_additionalcatelog") && product["new_additionalcatelog"] != null)
                    {
                        res_product.AdditionalCatalog = product.FormattedValues["new_additionalcatelog"];
                    }

                    res_product.ProductDescription = product.GetAttributeValue<string>("description");
                    res_product.UnitBarCode = product.GetAttributeValue<string>("new_barcode");
                    res_product.UnitWeight_KG = product.GetAttributeValue<decimal>("new_grossunitweight").ToString("#.##");
                    res_product.UnitDepthLength_CM = product.GetAttributeValue<decimal>("new_unitdepthcm").ToString("#.##");
                    res_product.UnitWidthGirth_CM = product.GetAttributeValue<decimal>("new_unitwidthcm").ToString("#.##");
                    res_product.UnitHeightNeck_CM = product.GetAttributeValue<decimal>("new_unitheight").ToString("#.##");
                    res_product.CartonBarcode = product.GetAttributeValue<string>("new_mastercartonbarcode");
                    res_product.UnitsPerCarton = product.GetAttributeValue<string>("new_packspermasterbox");
                    res_product.CartonWeight_KG = product.GetAttributeValue<decimal>("new_mastercartonweightkg").ToString("#.##");
                    res_product.CartonDepth_CM = product.GetAttributeValue<decimal>("new_mastercartondepthcm").ToString("#.##");
                    res_product.CartonWidth_CM = product.GetAttributeValue<decimal>("new_mastercartonwidthcm").ToString("#.##");
                    res_product.CartonHeight_CM = product.GetAttributeValue<decimal>("new_mastercartonheightcm").ToString("#.##");
                    res_product.CreatedOn = product.GetAttributeValue<DateTime>("createdon").ToLocalTime().ToString("yyyyMMddHHmmss");

                    products.Add(res_product);
                    //Console.WriteLine("account name:" + act["name"]);
                    //Console.WriteLine("primary contact first name:" + act["primarycontact.firstname"]);
                    //Console.WriteLine("primary contact last name:" + act["primarycontact.lastname"]);
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return products;
        }

        public AppCrmProduct AppGetProductById(String productId)
        {
            log.Info("AppGetProductById：" + productId);
            CrmUtil cu = new CrmUtil();
           OrganizationServiceProxy organizationProxy = cu.getCrmService();
            AppCrmProduct res_product = new AppCrmProduct();
            try
            {
                //Update Account TradeRegistration
                Entity product = new Entity("product");
                ColumnSet attributes = new ColumnSet(true);

                product = organizationProxy.Retrieve(product.LogicalName, new Guid(productId), attributes);

                res_product.ProductName = product.GetAttributeValue<string>("productnumber");
                res_product.ProductSku = product.GetAttributeValue<string>("name");
                res_product.Brand = product.GetAttributeValue<string>("suppliername");
                res_product.Quantity = product.GetAttributeValue<decimal>("quantityonhand").ToString("#.##");
                res_product.PictureList = product.GetAttributeValue<string>("new_picturelist");

                if (product.Contains("price") && product["price"] != null)
                {
                    res_product.ListPrice = product.GetAttributeValue<Money>("price").Value.ToString("#.##");
                }

                if (product.Contains("new_rrp") && product["new_rrp"] != null)
                {
                    res_product.RRP = product.GetAttributeValue<Money>("new_rrp").Value.ToString("#.##");
                }

                res_product.ProductColor = product.GetAttributeValue<string>("new_colour");
                res_product.ProductBarcode = product.GetAttributeValue<string>("new_productbarcode");
                res_product.ProductDimensions = product.GetAttributeValue<string>("new_size");
                res_product.ProductWeight = product.GetAttributeValue<string>("new_productweight");
                res_product.IdenticalUnitPerCase = product.GetAttributeValue<string>("new_casequantity");
                res_product.CaseBarcode = product.GetAttributeValue<string>("new_casebarcode");
                res_product.CaseDimensions = product.GetAttributeValue<string>("new_casedimensions");
                res_product.CaseWeight = product.GetAttributeValue<string>("new_caseweight");
                res_product.CTNDetails = product.GetAttributeValue<string>("new_ctndetails");

                if (product.Contains("new_parentcatalog") && product["new_parentcatalog"] != null)
                {
                    res_product.ParentCatalog = product.FormattedValues["new_parentcatalog"];
                }
                if (product.Contains("new_subcatelog") && product["new_subcatelog"] != null)
                {
                    res_product.SubCatalog = product.FormattedValues["new_subcatelog"];
                }
                if (product.Contains("new_additionalcatelog") && product["new_additionalcatelog"] != null)
                {
                    res_product.AdditionalCatalog = product.FormattedValues["new_additionalcatelog"];
                }
                res_product.ProductDescription = product.GetAttributeValue<string>("description");
                res_product.ProductCreatedon = product.GetAttributeValue<DateTime>("createdon").ToLocalTime().ToString("yyyyMMddHHmmss");


                //EntityReference subject = (EntityReference)product["subjectid"];
                //res_product.ProductCategory = subject.Name;
                //res_product.ProductPrice = ((Money)product["new_rrp"]).Value.ToString();

            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return res_product;
        }

       

        public List<AppCrmProduct> AppGetAllProduct()
        {
            log.Info("GetAllProduct：");
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            List<AppCrmProduct> products = new List<AppCrmProduct>();

            try
            {
                //Update Account TradeRegistration
                QueryExpression qe = new QueryExpression();
                qe.EntityName = "product";
                qe.ColumnSet = new ColumnSet(true);

                //qe.LinkEntities.Add(new LinkEntity("account", "contact", "primarycontactid", "contactid", JoinOperator.Inner));
                //qe.LinkEntities[0].Columns.AddColumns("firstname", "lastname");
                //qe.LinkEntities[0].EntityAlias = "primarycontact";

                EntityCollection ec = organizationProxy.RetrieveMultiple(qe);

                foreach (Entity product in ec.Entities)
                {
                    AppCrmProduct res_product = new AppCrmProduct();
                    res_product.ProductId = product.GetAttributeValue<Guid>("productid").ToString();
                    res_product.ProductName =  product.GetAttributeValue<string>("productnumber");
                    res_product.ProductSku = product.GetAttributeValue<string>("name");
                    res_product.Brand = product.GetAttributeValue<string>("suppliername");
                    res_product.Quantity = product.GetAttributeValue<decimal>("quantityonhand").ToString("#.##");
                    res_product.PictureList = product.GetAttributeValue<string>("new_picturelist");

                    if (product.Contains("price") && product["price"] != null)
                    {
                        res_product.ListPrice = product.GetAttributeValue<Money>("price").Value.ToString("#.##");
                    }
                    
                    if (product.Contains("new_rrp") && product["new_rrp"] != null)
                    {
                        res_product.RRP = product.GetAttributeValue<Money>("new_rrp").Value.ToString("#.##");
                    }
                    
                    res_product.ProductColor = product.GetAttributeValue<string>("new_colour");
                    res_product.ProductBarcode = product.GetAttributeValue<string>("new_productbarcode");
                    res_product.ProductDimensions = product.GetAttributeValue<string>("new_size");
                    res_product.ProductWeight = product.GetAttributeValue<string>("new_productweight");
                    res_product.IdenticalUnitPerCase = product.GetAttributeValue<string>("new_casequantity");
                    res_product.CaseBarcode = product.GetAttributeValue<string>("new_casebarcode");
                    res_product.CaseDimensions = product.GetAttributeValue<string>("new_casedimensions");
                    res_product.CaseWeight = product.GetAttributeValue<string>("new_caseweight");
                    res_product.CTNDetails = product.GetAttributeValue<string>("new_ctndetails");

                    if (product.Contains("new_parentcatalog") && product["new_parentcatalog"] != null)
                    {
                        res_product.ParentCatalog = product.FormattedValues["new_parentcatalog"];
                    }
                    if (product.Contains("new_subcatelog") && product["new_subcatelog"] != null)
                    {
                        res_product.SubCatalog = product.FormattedValues["new_subcatelog"];
                    }
                    if (product.Contains("new_additionalcatelog") && product["new_additionalcatelog"] != null)
                    {
                        res_product.AdditionalCatalog = product.FormattedValues["new_additionalcatelog"];
                    }
                    res_product.ProductDescription = product.GetAttributeValue<string>("description");
                    res_product.ProductCreatedon = product.GetAttributeValue<DateTime>("createdon").ToLocalTime().ToString("yyyyMMddHHmmss");

                    
                    products.Add(res_product);
                }
            }
            catch (Exception ex)
            {
                log.Info(ex+"--"+ex.Message + "--" + ex.ToString());
            }
            return products;
        }

        public List<SupplierCrmProduct> AppGetProductBySupplier(String email)
        {
            log.Info("AppGetProductBySupplier：" + email);
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            List<SupplierCrmProduct> res_products = new List<SupplierCrmProduct>();

            try
            {
                QueryExpression qe = new QueryExpression();
                qe.EntityName = "product";
                qe.ColumnSet = new ColumnSet(true);

                ConditionExpression condition1 = new ConditionExpression();
                condition1.AttributeName = "emailaddress1";
                condition1.Operator = ConditionOperator.Equal;
                condition1.Values.Add(email);

                FilterExpression filter1 = new FilterExpression();
                filter1.Conditions.Add(condition1);

                //qe.Criteria.AddFilter(filter1);

                qe.LinkEntities.Add(new LinkEntity("product", "account", "new_suppliera1", "accountid", JoinOperator.Inner));
                qe.LinkEntities[0].Columns.AddColumns("accountid");
                qe.LinkEntities[0].EntityAlias = "supplier";
                qe.LinkEntities[0].LinkCriteria.AddFilter(filter1);



                //  Query passed to service proxy.
                EntityCollection retrieved = organizationProxy.RetrieveMultiple(qe);
                foreach (Entity product in retrieved.Entities)
                {
                    SupplierCrmProduct res_product = new SupplierCrmProduct();
                    res_product.ProductId = product.GetAttributeValue<Guid>("productid").ToString();
                    res_product.ProductName = product.GetAttributeValue<string>("productnumber");
                    res_product.ProductSku = product.GetAttributeValue<string>("name");
                    res_product.Brand = product.GetAttributeValue<string>("suppliername");
                    res_product.Quantity = product.GetAttributeValue<decimal>("quantityonhand").ToString("#.##");

                    
                    res_product.CommissionRate = product.GetAttributeValue<decimal>("new_commisionrate").ToString("#.##");


                    if (product.Contains("price") && product["price"] != null)
                    {
                        res_product.TradePrice = product.GetAttributeValue<Money>("price").Value.ToString("#.##");
                    }
                    if (product.Contains("new_pdhprice") && product["new_pdhprice"] != null)
                    {
                        res_product.PdhPrice = product.GetAttributeValue<Money>("new_pdhprice").Value.ToString("#.##");
                    }

                    if (product.Contains("new_rrp") && product["new_rrp"] != null)
                    {
                        res_product.RRP = product.GetAttributeValue<Money>("new_rrp").Value.ToString("#.##");
                    }

                    //res_product.ProductColor = product.GetAttributeValue<string>("new_colour");
                    //res_product.ProductBarcode = product.GetAttributeValue<string>("new_productbarcode");
                    res_product.ProductDimensions = product.GetAttributeValue<string>("new_size");
                    res_product.ProductWeight = product.GetAttributeValue<string>("new_productweight");
                    res_product.IdenticalUnitPerCase = product.GetAttributeValue<string>("new_casequantity");
                    res_product.CaseBarcode = product.GetAttributeValue<string>("new_casebarcode");
                    res_product.CaseDimensions = product.GetAttributeValue<string>("new_casedimensions");
                    res_product.CaseWeight = product.GetAttributeValue<string>("new_caseweight");
                    res_product.CTNDetails = product.GetAttributeValue<string>("new_ctndetails");

                    if (product.Contains("new_parentcatalog") && product["new_parentcatalog"] != null)
                    {
                        res_product.ParentCatalog = product.FormattedValues["new_parentcatalog"];
                    }
                    if (product.Contains("new_subcatelog") && product["new_subcatelog"] != null)
                    {
                        res_product.SubCatalog = product.FormattedValues["new_subcatelog"];
                    }
                    if (product.Contains("new_additionalcatelog") && product["new_additionalcatelog"] != null)
                    {
                        res_product.AdditionalCatalog = product.FormattedValues["new_additionalcatelog"];
                    }
                    res_product.ProductDescription = product.GetAttributeValue<string>("description");
                    res_product.ProductCreatedon = product.GetAttributeValue<DateTime>("createdon").ToLocalTime().ToString("yyyyMMddHHmmss");

                    res_products.Add(res_product);
                }

            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return res_products;
        }

        public SupplierCrmProductResponse AppUpdateProduct(SupplierCrmProduct newProduct)
        {


            log.Info("CMS-->Product update：" + newProduct.ProductName);
            SupplierCrmProductResponse response = new SupplierCrmProductResponse();
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();

            try
            {
                Entity oldProduct = new Entity("product");
                ColumnSet attributes = new ColumnSet(new string[] { "quantityonhand" });

                oldProduct = organizationProxy.Retrieve(oldProduct.LogicalName, new Guid(newProduct.ProductId), attributes);

                //oldProduct["productnumber"] = newProduct.ProductName;
                //oldProduct["name"] = newProduct.ProductSku;
                //oldProduct["description"] = newProduct.ProductDescription;
                oldProduct["quantityonhand"] = Decimal.Parse(newProduct.Quantity);


                organizationProxy.Update(oldProduct);

                response.result = "success";
                return response;
            }
            catch (Exception ex)
            {
                log.Info(ex.Message + "--" + ex.ToString());
            }
            return null;
        }

        public AppCrmProductCategory AppGetProdcutCategory()
        {
            //new_parentcatalog,new_subcatelog,new_featuredbrandscatalog,new_additionalcatelog
            log.Info("AppGetProdcutCategory：");
            AppCrmProductCategory category = new AppCrmProductCategory();
            List<AppCrmProductParentCatalog> parentCatalogs = new List<AppCrmProductParentCatalog>();
            List<AppCrmProductSubCatalog> subCatalogs = new List<AppCrmProductSubCatalog>();
            List<AppCrmProductBrandCatalog> brandCatalogs = new List<AppCrmProductBrandCatalog>();
            List<AppCrmProductFeaturedBrandsCatalog> featuredBrandsCatalog = new List<AppCrmProductFeaturedBrandsCatalog>();
            try
            {
            

                CrmUtil cu = new CrmUtil();
                OrganizationServiceProxy organizationProxy = cu.getCrmService();

                OptionMetadataCollection optionSetMetaData = getOptionSetValueAndName("product", "new_parentcatalog", organizationProxy);
                foreach (OptionMetadata oc in optionSetMetaData)
                {
                    AppCrmProductParentCatalog parentCatalog = new AppCrmProductParentCatalog();
                    parentCatalog.ParentCatalogName = oc.Label.UserLocalizedLabel.Label;
                    parentCatalog.ParentCatalogValue = oc.Value.ToString();

                    parentCatalogs.Add(parentCatalog);
                }
                OptionMetadataCollection optionSetMetaData1 = getOptionSetValueAndName("product", "new_subcatelog", organizationProxy);
                foreach (OptionMetadata oc in optionSetMetaData1)
                {
                    AppCrmProductSubCatalog subCatalog = new AppCrmProductSubCatalog();
                    subCatalog.SubCatalogName = oc.Label.UserLocalizedLabel.Label;
                    subCatalog.SubCatalogValue = oc.Value.ToString();

                    subCatalogs.Add(subCatalog);
                }
                OptionMetadataCollection optionSetMetaData3 = getOptionSetValueAndName("product", "new_featuredbrandscatalog", organizationProxy);
                foreach (OptionMetadata oc in optionSetMetaData3)
                {
                    AppCrmProductFeaturedBrandsCatalog featuredCatalog = new AppCrmProductFeaturedBrandsCatalog();
                    featuredCatalog.FeaturedBrandsCatalogName = oc.Label.UserLocalizedLabel.Label;
                    featuredCatalog.FeaturedBrandsCatalogValue = oc.Value.ToString();

                    featuredBrandsCatalog.Add(featuredCatalog);
                }
                OptionMetadataCollection optionSetMetaData2 = getOptionSetValueAndName("product", "new_additionalcatelog", organizationProxy);
                foreach (OptionMetadata oc in optionSetMetaData2)
                {
                    AppCrmProductBrandCatalog brandCatalog = new AppCrmProductBrandCatalog();
                    brandCatalog.BrandCatalogName = oc.Label.UserLocalizedLabel.Label;
                    brandCatalog.BrandCatalogValue = oc.Value.ToString();

                    brandCatalogs.Add(brandCatalog);
                }
                

                category.ParentCatalog = parentCatalogs;
                category.SubCatalog = subCatalogs;
                category.BrandCatalog = brandCatalogs;
                category.FeaturedBrandsCatalog = featuredBrandsCatalog;
            }
            catch (Exception ex)
            {
                log.Info(ex + "--" + ex.Message + "--" + ex.ToString());
            }
            return category;
        }
        private OptionMetadataCollection getOptionSetValueAndName(string entityname, string attributename, OrganizationServiceProxy organizationProxy)
        {
            //RetrieveOptionSetRequest optionSetRequest = new RetrieveOptionSetRequest { Name = filedName };
            //RetrieveOptionSetResponse optionSetResponse = (RetrieveOptionSetResponse)organizationProxy.Execute(optionSetRequest);

            //OptionSetMetadata optionSetMetaData = (OptionSetMetadata)optionSetResponse.OptionSetMetadata;
            //return optionSetMetaData;

            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest { 
                EntityLogicalName = entityname, LogicalName = attributename }; 
            // Execute the request. 
            RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)organizationProxy.Execute(retrieveAttributeRequest); 
            OptionMetadataCollection options = ((PicklistAttributeMetadata)retrieveAttributeResponse.AttributeMetadata).OptionSet.Options;

            return options;

        }
        public List<AppCrmProduct> AppGetProductsByCategory(AppCrmProductCategoryRequest request)
        {
            log.Info("AppGetProductsByCategory：");
            CrmUtil cu = new CrmUtil();
            OrganizationServiceProxy organizationProxy = cu.getCrmService();
            List<AppCrmProduct> products = new List<AppCrmProduct>();

            try
            {
                QueryExpression qe = new QueryExpression();
                qe.EntityName = "product";
                qe.ColumnSet = new ColumnSet(true);

                ConditionExpression condition1 = new ConditionExpression();
                condition1.AttributeName = request.categoryType;
                condition1.Operator = ConditionOperator.Equal;
                condition1.Values.Add(request.categoryValue);

                FilterExpression filter1 = new FilterExpression();
                filter1.Conditions.Add(condition1);

                qe.Criteria.AddFilter(filter1);

                EntityCollection ec = organizationProxy.RetrieveMultiple(qe);

                foreach (Entity product in ec.Entities)
                {
                    AppCrmProduct res_product = new AppCrmProduct();
                    res_product.ProductId = product.GetAttributeValue<Guid>("productid").ToString();
                    res_product.ProductName = product.GetAttributeValue<string>("productnumber");
                    res_product.ProductSku = product.GetAttributeValue<string>("name");
                    res_product.Brand = product.GetAttributeValue<string>("suppliername");
                    res_product.Quantity = product.GetAttributeValue<decimal>("quantityonhand").ToString("#.##");
                    res_product.PictureList = product.GetAttributeValue<string>("new_picturelist");

                    if (product.Contains("price") && product["price"] != null)
                    {
                        res_product.ListPrice = product.GetAttributeValue<Money>("price").Value.ToString("#.##");
                    }

                    if (product.Contains("new_rrp") && product["new_rrp"] != null)
                    {
                        res_product.RRP = product.GetAttributeValue<Money>("new_rrp").Value.ToString("#.##");
                    }

                    res_product.ProductColor = product.GetAttributeValue<string>("new_colour");
                    res_product.ProductBarcode = product.GetAttributeValue<string>("new_productbarcode");
                    res_product.ProductDimensions = product.GetAttributeValue<string>("new_size");
                    res_product.ProductWeight = product.GetAttributeValue<string>("new_productweight");
                    res_product.IdenticalUnitPerCase = product.GetAttributeValue<string>("new_casequantity");
                    res_product.CaseBarcode = product.GetAttributeValue<string>("new_casebarcode");
                    res_product.CaseDimensions = product.GetAttributeValue<string>("new_casedimensions");
                    res_product.CaseWeight = product.GetAttributeValue<string>("new_caseweight");
                    res_product.CTNDetails = product.GetAttributeValue<string>("new_ctndetails");

                    if (product.Contains("new_parentcatalog") && product["new_parentcatalog"] != null)
                    {
                        res_product.ParentCatalog = product.FormattedValues["new_parentcatalog"];
                    }
                    if (product.Contains("new_subcatelog") && product["new_subcatelog"] != null)
                    {
                        res_product.SubCatalog = product.FormattedValues["new_subcatelog"];
                    }
                    if (product.Contains("new_additionalcatelog") && product["new_additionalcatelog"] != null)
                    {
                        res_product.AdditionalCatalog = product.FormattedValues["new_additionalcatelog"];
                    }
                    res_product.ProductDescription = product.GetAttributeValue<string>("description");
                    res_product.ProductCreatedon = product.GetAttributeValue<DateTime>("createdon").ToLocalTime().ToString("yyyyMMddHHmmss");


                    products.Add(res_product);
                }
            }
            catch (Exception ex)
            {
                log.Info(ex + "--" + ex.Message + "--" + ex.ToString());
            }
            return products;

        }



    }
}
