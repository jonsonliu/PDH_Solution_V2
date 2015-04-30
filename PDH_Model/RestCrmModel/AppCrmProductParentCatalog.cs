using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{

    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmProductParentCatalog))]
    public class AppCrmProductParentCatalog
    {
        [DataMember(Order = 1)]
        public string ParentCatalogName { get; set; }
        [DataMember(Order = 2)]
        public string ParentCatalogValue { get; set; }
    }
}
