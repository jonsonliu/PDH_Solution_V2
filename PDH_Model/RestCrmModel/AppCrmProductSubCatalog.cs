using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmProductSubCatalog))]
    public class AppCrmProductSubCatalog
    {
        [DataMember(Order = 1)]
        public string SubCatalogName { get; set; }
        [DataMember(Order = 2)]
        public string SubCatalogValue { get; set; }
    }
}
