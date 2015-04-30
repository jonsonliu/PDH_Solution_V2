using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmProductBrandCatalog))]
    public class AppCrmProductBrandCatalog
    {
        [DataMember(Order = 1)]
        public string BrandCatalogName { get; set; }
        [DataMember(Order = 2)]
        public string BrandCatalogValue { get; set; }
    }
}
