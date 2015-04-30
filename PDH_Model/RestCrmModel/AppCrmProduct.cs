using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{
    
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmProduct))]
    public class AppCrmProduct
    {

        [DataMember(Order = 1)]
        public string ProductName { get; set; }
        [DataMember(Order = 2)]
        public string ProductSku { get; set; }
        [DataMember(Order = 3)]
        public string Brand { get; set; }
        [DataMember(Order = 4)]
        public string Quantity { get; set; }
        [DataMember(Order = 5)]
        public string ListPrice { get; set; }
        [DataMember(Order = 6)]
        public string RRP { get; set; }
        [DataMember(Order = 7)]
        public string ProductColor { get; set; }
        [DataMember(Order = 8)]
        public string ProductBarcode { get; set; }
        [DataMember(Order = 9)]
        public string ProductDimensions { get; set; }
        [DataMember(Order = 10)]
        public string ProductWeight { get; set; }
        [DataMember(Order = 11)]
        public string IdenticalUnitPerCase { get; set; }
        [DataMember(Order = 12)]
        public string CaseBarcode { get; set; }
        [DataMember(Order = 13)]
        public string CaseDimensions { get; set; }
        [DataMember(Order = 14)]
        public string CaseWeight { get; set; }
        [DataMember(Order = 15)]
        public string CTNDetails { get; set; }
        [DataMember(Order = 16)]
        public string ParentCatalog { get; set; }
        [DataMember(Order = 17)]
        public string SubCatalog { get; set; }
        [DataMember(Order = 18)]
        public string AdditionalCatalog { get; set; }
        [DataMember(Order = 19)]
        public string ProductCreatedon { get; set; }
        [DataMember(Order = 20)]
        public string ProductDescription { get; set; }
        [DataMember(Order = 21)]
        public string ProductId { get; set; }
        [DataMember(Order = 22)]
        public string PictureList { get; set; }

        
    }
}
