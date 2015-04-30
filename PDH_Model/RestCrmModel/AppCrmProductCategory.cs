using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmProductCategory))]
    public class AppCrmProductCategory
    {
        [DataMember(Order = 1)]
        public List<AppCrmProductParentCatalog> ParentCatalog { get; set; }
        [DataMember(Order = 2)]
        public List<AppCrmProductSubCatalog> SubCatalog { get; set; }
        [DataMember(Order = 3)]
        public List<AppCrmProductBrandCatalog> BrandCatalog { get; set; }
        [DataMember(Order = 4)]
        public List<AppCrmProductFeaturedBrandsCatalog> FeaturedBrandsCatalog { get; set; }
        
        
        
        
    }
}
