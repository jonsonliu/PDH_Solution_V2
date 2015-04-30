using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{
    
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmOrderHistoryResponse))]
    public class AppCrmOrderHistoryResponse
    {
        [DataMember(Order = 1)]
        public string orderName { get; set; }

        [DataMember(Order = 2)]
        public string createdOn { get; set; }

        [DataMember(Order = 3)]
        public string totalAmount { get; set; }

        [DataMember(Order = 4)]
        public string totalRRP { get; set; }

        [DataMember(Order = 5)]
        public List<AppCrmOrderHistoryProduct> product { get; set; }


    }
}
