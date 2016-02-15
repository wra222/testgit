using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    public enum SpecialOrderStatus
    {
        Created=0,
        Active,
        Closed
    }

    [Serializable]
    public class SpecialOrderInfo
    {
        public string FactoryPO { get; set; }
        public string Category { get; set; }
        public string AssetTag { get; set; }
        public int Qty { get; set; }
        public SpecialOrderStatus Status { get; set; }
        public string Remark { get; set; }
        public string Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
    }
}
