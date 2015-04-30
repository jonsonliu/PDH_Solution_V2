using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmOrderHistoryRequest))]
    public class AppCrmOrderHistoryRequest
    {
        [DataMember(Order = 1)]
        public string email { get; set; }

        [DataMember(Order = 2)]
        public string beginDate { get; set; }

        [DataMember(Order = 3)]
        public string endDate { get; set; }

        
    }
}
