using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class ShipTypeMaintain
    {
        public string shipType { get; set; }
        public string Description { get; set; }
        public string Editor { get; set; }
        public DateTime Udt { get; set; }
        public DateTime Cdt { get; set; }
    }
}
