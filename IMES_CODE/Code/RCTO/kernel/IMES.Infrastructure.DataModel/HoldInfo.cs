using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class HoldInfo
    {
        public string ProductID { get; set; }
        public string Model { get; set; }
        public string CUSTSN {get; set;}
        public string Station { get; set; }
        public string PreStation { get; set; }
        public int PreStatus { get; set; }
        public string PreLine { get; set; }
        public string HoldUser { get; set; }
        public DateTime HoldTime { get; set; }
        public string HoldCode { get; set; }
        public string HoldCodeDescr { get; set; }
        public int HoldID { get; set; }
    }
}
