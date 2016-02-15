using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    public class FRUWeightLog
    {
        public int ID { get; set; }
        public string SN { get; set; }
        public decimal Weight { get; set; }
        public string Line { get; set; }
        public string Station { get; set; }
        public string Editor { get; set; }
        public DateTime Cdt { get; set; }
    }
}
