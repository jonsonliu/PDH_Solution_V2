using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(UpdateTradeRegistrationResponse))]
    public class UpdateProductQuantityResponse
    {
        [DataMember(Order = 1)]
        public string ErrCode { get; set; }
    }
}
