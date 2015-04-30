using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CreateCrmOrder))]
    public class CreateCrmOrder
    {

        [DataMember(Order = 1)]
        public string OrderName { get; set; }
        [DataMember(Order = 2)]
        public string CustomerId { get; set; }
        [DataMember(Order = 3)]
        public string PricelevelId { get; set; }
        [DataMember(Order = 4)]
        public string PoNumbser { get; set; }
        [DataMember(Order = 5)]
        public string PaymenttermsCode { get; set; }
        [DataMember(Order = 6)]
        public string BilltoName { get; set; }
        [DataMember(Order = 7)]
        public string BilltoLine1 { get; set; }
        [DataMember(Order = 8)]
        public string BilltoLine2 { get; set; }
        [DataMember(Order = 9)]
        public string BilltoCity { get; set; }
        [DataMember(Order = 10)]
        public string BilltoStateOrProvince { get; set; }
        [DataMember(Order = 11)]
        public string BilltoCountry { get; set; }
        [DataMember(Order = 12)]
        public string BilltoPostalcode { get; set; }
        [DataMember(Order = 13)]
        public string ShiptoName { get; set; }
        [DataMember(Order = 14)]
        public string ShiptoLine1 { get; set; }
        [DataMember(Order = 15)]
        public string ShiptoLine2 { get; set; }
        [DataMember(Order = 16)]
        public string ShiptoCity { get; set; }
        [DataMember(Order = 17)]
        public string ShiptoStateOrProvince { get; set; }
        [DataMember(Order = 18)]
        public string ShiptoCountry { get; set; }
        [DataMember(Order = 19)]
        public string ShiptoPostalcode { get; set; }
        [DataMember(Order = 20)]
        public string IsFreeShipping { get; set; }
        [DataMember(Order = 21)]
        public List<CrmProduct> CrmProductList { get; set; }


    }
}
