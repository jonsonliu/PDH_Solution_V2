using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.RestCrmModel
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AppCrmAccount))]
    public class AppCrmAccount
    {

        [DataMember(Order = 1)]
        public string AccName { get; set; }
        [DataMember(Order = 2)]
        public string AccPhone { get; set; }
        [DataMember(Order = 3)]
        public string AccFax { get; set; }
        [DataMember(Order = 4)]
        public string AccEmail { get; set; }
        [DataMember(Order = 5)]
        public string AccNewsletter { get; set; }
        [DataMember(Order = 6)]
        public string AccStreet1 { get; set; }
        [DataMember(Order = 7)]
        public string AccStreet2 { get; set; }
        [DataMember(Order = 8)]
        public string AccCity { get; set; }
        [DataMember(Order = 9)]
        public string AccStateProvince { get; set; }
        [DataMember(Order = 10)]
        public string AccCountryRegion { get; set; }
        [DataMember(Order = 11)]
        public string AccZIPPostalCode { get; set; }
        [DataMember(Order = 12)]
        public string AccTradeRegistration { get; set; }
        [DataMember(Order = 13)]
        public string AccPrimaryContact { get; set; }
    }
}
