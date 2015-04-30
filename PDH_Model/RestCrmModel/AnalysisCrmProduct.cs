using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{

    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CrmProduct))]
    public class AnalysisCrmProduct
    {
        [DataMember(Order = 1)]
        public string ProductName { get; set; }
        [DataMember(Order = 2)]
        public string ProductSku { get; set; }
        [DataMember(Order = 3)]
        public string ProductStatus { get; set; }
        [DataMember(Order = 4)]
        public string SupplierA { get; set; }
        [DataMember(Order = 5)]
        public string Unit { get; set; }
        [DataMember(Order = 6)]
        public string DecimalsSupported { get; set; }
        [DataMember(Order = 7)]
        public string PriceList { get; set; }
        [DataMember(Order = 8)]
        public string Brand { get; set; }
        [DataMember(Order = 9)]
        public string Quantity { get; set; }
        [DataMember(Order = 10)]
        public string BrandedPackage { get; set; }
        [DataMember(Order = 11)]
        public string PetDreamHouseURL { get; set; }
        [DataMember(Order = 12)]
        public string ListPrice { get; set; }
        [DataMember(Order = 13)]
        public string RRP { get; set; }
        [DataMember(Order = 14)]
        public string ProductColor { get; set; }
        [DataMember(Order = 15)]
        public string ProductBarcode { get; set; }
        [DataMember(Order = 16)]
        public string ProductDimensions { get; set; }
        [DataMember(Order = 17)]
        public string ProductWeight { get; set; }
        [DataMember(Order = 18)]
        public string IdenticalUnitPerCase { get; set; }
        [DataMember(Order = 19)]
        public string CaseBarcode { get; set; }
        [DataMember(Order = 20)]
        public string CaseDimensions { get; set; }
        [DataMember(Order = 21)]
        public string CaseWeight { get; set; }
        [DataMember(Order = 22)]
        public string CTNDetails { get; set; }
        [DataMember(Order = 23)]
        public string ParentCatalog { get; set; }
        [DataMember(Order = 24)]
        public string SubCatalog { get; set; }
        [DataMember(Order = 25)]
        public string AdditionalCatalog { get; set; }
        [DataMember(Order = 26)]
        public string ProductDescription { get; set; }
        [DataMember(Order = 27)]
        public string UnitBarCode { get; set; }
        [DataMember(Order = 28)]
        public string UnitWeight_KG { get; set; }
        [DataMember(Order = 29)]
        public string UnitDepthLength_CM { get; set; }
        [DataMember(Order = 30)]
        public string UnitWidthGirth_CM { get; set; }
        [DataMember(Order = 31)]
        public string UnitHeightNeck_CM { get; set; }
        [DataMember(Order = 32)]
        public string CartonBarcode { get; set; }
        [DataMember(Order = 33)]
        public string UnitsPerCarton { get; set; }
        [DataMember(Order = 34)]
        public string CartonWeight_KG { get; set; }
        [DataMember(Order = 35)]
        public string CartonDepth_CM { get; set; }
        [DataMember(Order = 36)]
        public string CartonWidth_CM { get; set; }
        [DataMember(Order = 37)]
        public string CartonHeight_CM { get; set; }
        [DataMember(Order = 38)]
        public string CreatedOn { get; set; }
        [DataMember(Order = 39)]
        public string ProductId { get; set; }
    }
}
