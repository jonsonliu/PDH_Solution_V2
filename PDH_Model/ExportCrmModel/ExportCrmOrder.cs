using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDH_Model.ExportCrmModel
{
    public class ExportCrmOrder
    {
        public string OrderId { get; set; }
        public string BillToName { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string OrderNo { get; set; }
        public string RefNo { get; set; }
        public string ShipDate { get; set; }
        public string ShipVia { get; set; }
        public string Note { get; set; }
        public List<ExportCrmOrderProduct> products { get; set; }
    }
}
