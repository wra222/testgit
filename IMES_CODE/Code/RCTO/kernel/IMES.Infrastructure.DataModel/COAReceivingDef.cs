using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class COAReceivingDef
    {
        public int id;
        public string customerName;
        public string po;
        public string shippingDate;
        public string custPN;
        public string iecPN;
        public string description;
        public string begNO;
        public string endNO;
        public string qty;
        public string pc;
        public string editor;
        public string cdt;
        public string udt;
    }
}
