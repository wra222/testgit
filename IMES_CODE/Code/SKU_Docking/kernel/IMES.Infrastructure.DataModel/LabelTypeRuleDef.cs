using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class LabelTypeRuleDef
    {
        public string LabelType { get; set; }       
        public string Station { get; set; }
        public string Family { get; set; }
        public string Model { get; set; }
        public string ModelConstValue { get; set; }
        public string DeliveryConstValue { get; set; }
        //public string ModelInfoValue { get; set; }
        public int BomLevel { get; set; }
        public string PartNo { get; set; }
        public string BomNodeType { get; set; }
        public string PartDescr { get; set; }
        public string PartType { get; set; }
        public string PartConstValue { get; set; }
       // public string PartInfoValue { get; set; }
        public string Remark { get; set; }
        public string Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }
    }
}
