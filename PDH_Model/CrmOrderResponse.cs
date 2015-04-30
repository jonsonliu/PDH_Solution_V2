using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model
{
    
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CrmOrderResponse))]
    public class CrmOrderResponse
    {
        public enum ErrCodeType
        {
            Success = 0,                    //操作成功
            Failure = 1                     //操作失败
        }

        [DataMember(Order = 1)]
        public string ErrCode { get; set; }

    }
}
