using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model
{
    
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CrmProduct))]
    public class CrmProduct
    {

        [DataMember(Order = 1)]
        public string ProductName { get; set; }
        [DataMember(Order = 2)]
        public string Quantity { get; set; }
        [DataMember(Order = 3)]
        public string FreightAmount { get; set; }
        [DataMember(Order = 4)]
        public string PercentDiscount { get; set; }
        [DataMember(Order = 5)]
        public string AmountDiscount { get; set; }
        [DataMember(Order = 6)]
        public string ManufacturerId { get; set; }
    }
}
