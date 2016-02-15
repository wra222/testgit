using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IMES.Query.Interface.QueryIntf
{
    public interface IFA_PIADefectList
    {
        DataTable GetQueryResult(string Connection, DateTime FromRepairDate, DateTime ToRepairDate,
              string Station, string Defect, string Cause, List<string> PdLine);
        DataTable GetQueryResult(string Connection, DateTime FromRepairDate, DateTime ToRepairDate,
              List<string> Station, List<string> Defect, List<string> Cause, List<string> PdLine, string ModelCategory);

    }

    [Serializable]
    public class QueryCondition
    {
        public string DBType;
        public int DBIndex;
        public DateTime FromRepairDate;
        public DateTime ToRepairDate;
        public string Station;
        public string Defect;
        public string Cause;        
    }
    
}
