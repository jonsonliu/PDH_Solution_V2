using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model
{
    [DataContract]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(AddAccountAndContact))]
    public class AddAccountAndContact
    {
        [DataMember(Order = 1)]
        public string AccAccountName { get; set; }
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

        [DataMember(Order = 14)]
        public string ContFirstName { get; set; }
        [DataMember(Order = 15)]
        public string ContLastName { get; set; }
        [DataMember(Order = 16)]
        public string ContCompanyName { get; set; }
        [DataMember(Order = 17)]
        public string ContEmail { get; set; }
        [DataMember(Order = 18)]
        public string ContTelephone1 { get; set; }
        [DataMember(Order = 19)]
        public string ContStreet1 { get; set; }
        [DataMember(Order = 20)]
        public string ContStreet2 { get; set; }
        [DataMember(Order = 21)]
        public string ContCity { get; set; }
        [DataMember(Order = 22)]
        public string ContStateProvince { get; set; }
        [DataMember(Order = 23)]
        public string ContCountryRegion { get; set; }
        [DataMember(Order = 24)]
        public string ContZIPPostalCode { get; set; }
    }
}
