using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class DeliveryForRCTO146
    {
        public  string DeliveryNo{get;set;}
        public string ShipmentNo { get; set; }
        public string Model { get; set; }
        public DateTime ShipDate { get; set; }
        public string Status { get; set; }
        public int Qty { get; set; }
        public string ShipWay { get; set; }
        public int QtyPerCarton { get; set; }
        public int CartonQty { get; set; }
        public string OrderType { get; set; }
        public string MessageCode { get; set; }
       
    }
}
