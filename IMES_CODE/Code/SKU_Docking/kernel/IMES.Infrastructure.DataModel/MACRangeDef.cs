using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{

    [Serializable]
    public struct MACRangeDef
    {
        public String id;
        public String Code;
        public String BegNo;
        public String EndNo;
        public String Status;
        public String StatusText;
        public String Editor;
        public String Cdt;
        public String Udt;
    }
}
