using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{
    
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmOrderHistoryProduct))]
    public class AppCrmOrderHistoryProduct
    {

        [DataMember(Order = 1)]
        public string ProductId { get; set; }
        [DataMember(Order = 2)]
        public string ProductSku { get; set; }
        [DataMember(Order = 3)]
        public string PriceList { get; set; }
        [DataMember(Order = 4)]
        public string Quantity { get; set; }
        [DataMember(Order = 5)]
        public string RRP { get; set; }
    }
}
