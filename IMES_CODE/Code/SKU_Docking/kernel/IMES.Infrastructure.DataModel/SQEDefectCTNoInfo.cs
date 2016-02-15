using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class SQEDefectCTNoInfo
    {
        public string Line;
        public string Defect;
        public string Cause;
        public DateTime Cdt;
        public DateTime Udt;
    }
}
