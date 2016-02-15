using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    /// <summary>
    /// 查询DN的条件
    /// </summary>
    [Serializable]
    public class DNQueryCondition
    {
        public string DeliveryNo = null;
        public string PONo = null;
        public string Model = null;

        public DateTime ShipDateFrom = DateTime.MinValue;
        public DateTime ShipDateTo = DateTime.MinValue;

        public string ShipmentNo = null;
        public string DNInfoValue = null;
    }
}
