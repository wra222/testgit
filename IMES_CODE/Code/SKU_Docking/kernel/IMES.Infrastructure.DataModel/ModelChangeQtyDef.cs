using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    public class ModelChangeQtyDef
    {
        public int ID { get; set; }
        public string Line { get; set; }
        public string Model { get; set; }
        public int Qty { get; set; }
        public DateTime ShipDate { get; set; }
        public int AssignedQty { get; set; }
        public string Status { get; set; }
        public string Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
    }
}
