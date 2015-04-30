using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace PDH_Model.RestCrmModel
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmAuthorization))]
    public class AppCrmAuthorization
    {
        [DataMember(Order = 1)]
        public string IsLoginSuccess { get; set; }
    }
}
