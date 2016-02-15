using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.FisObject.Common.QTime
{
    [Serializable]
    public class LineStationLastProcessTime
    {
        public string Line { get; set; }
        public string Station { get; set; }
        public string ProductID { get; set; }
        public DateTime ProcessTime { get; set; }
        public DateTime Now { get; set; }
        public int SpeedTime { get; set; } //unit is second
        public string Editor { get; set; }
       
    }
}
