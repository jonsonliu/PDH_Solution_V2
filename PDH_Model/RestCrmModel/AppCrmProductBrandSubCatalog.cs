using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmProductFeaturedBrandsCatalog))]
    public class AppCrmProductFeaturedBrandsCatalog
    {
        [DataMember(Order = 1)]
        public string FeaturedBrandsCatalogName { get; set; }
        [DataMember(Order = 2)]
        public string FeaturedBrandsCatalogValue { get; set; } 
    }
}
