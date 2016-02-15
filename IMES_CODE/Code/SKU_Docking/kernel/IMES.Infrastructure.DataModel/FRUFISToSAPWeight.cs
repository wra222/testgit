using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    public class FRUFISToSAPWeight
    {
        public string Shipment { get; set; }
        public string Type { get; set; }
        public decimal Weight { get; set; }
        public string Status { get; set; }
        public DateTime Cdt { get; set; }
    }
}
