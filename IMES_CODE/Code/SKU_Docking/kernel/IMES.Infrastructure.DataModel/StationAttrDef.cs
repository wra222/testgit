using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
   public  class StationAttrDef
    {
        public string Station { get; set; }
        public string AttrName { get; set; }
        public string AttrValue { get; set; }
        public string Descr { get; set; }
        public string Editor { get; set; }
        public DateTime Cdt { get; set; }
        public DateTime Udt { get; set; }

    }
}
