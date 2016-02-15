using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_SAReceiveReturnMBQuery
    {
        DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime);
        DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime, IList<string> MBCode);
        //DataTable GetMBRepairInfo(string DBConnection, DateTime StartTime, DateTime EndTime, string Family, IList<string> Model, IList<string> Station);
        DataTable GetMBCode(string DBConnection);
    }

    [Serializable]
    public class SAReceiveReturnMBInfo 
    {
        public string MBSN;
        public string PartNo;
        public string MAC;
        public string ECR;
        public string SMTMO;
        public string CustomSN;
        public string Station;
        public string StationDescr;
        public string Line;
        public int Status;
        public int TestFailCount;
        public DateTime Udt;
    }
}
