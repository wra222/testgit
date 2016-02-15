using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMES.DataModel
{
    [Serializable]
    public class SQEDefectProductRepairReportInfo
    {
        public string DefectCode = string.Empty;
        public string Cause = string.Empty;
        public string Obligation = string.Empty;
        public string Component = string.Empty;
        public string MajorPart = string.Empty;
        public string Remark = string.Empty;
        public string VendorCT = string.Empty;
        public string Editor = string.Empty;
        public DateTime Cdt = DateTime.MinValue;
        public DateTime Udt = DateTime.MinValue;
    }
}
