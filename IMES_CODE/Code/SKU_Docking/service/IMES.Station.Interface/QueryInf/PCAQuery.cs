using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Station.Interface.QueryInf
{
    public interface IPCAQuery
    {
        MBInfo GetMBInfo(string MBSN, out List< NextStation> NextStationList);
        
        DataTable GetMBHistory(string MBSN);
        DataTable GetStation();
        DataTable GetMultiPCBInfo(IList<string> PCBNoList);
        void UpdatePCBStatus(IList<string> PCBNoList, string station, int status, string editor);  

    }

    [Serializable]
    public class MBInfo 
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

    [Serializable]
    public class NextStation
    {
        public string Station;
        public string Description;        
    }
}
