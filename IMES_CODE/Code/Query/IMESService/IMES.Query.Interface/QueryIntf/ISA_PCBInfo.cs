﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IMES.Query.Interface.QueryIntf
{
    public interface ISA_PCBInfo
    {
        MBInfo GetMBInfo(String DBConnection, string MBSN, out List<NextStation> NextStationList);

        DataTable GetMBHistory(String DBConnection, string MBSN);
        DataTable GetStation(String MBSN ,String DBConnection);
        DataTable GetChangePCBLog(String DBConnection, String MBSN);
        //void UpdatePCBStatus(IList<string> PCBNoList, string station, int status, string editor);  
        DataTable GetRepairInfo(String DBConnection, String MBSN);
        DataTable GetTestLog(string DBConnection, String MBSN);
        DataTable GetPCBInfo(string DBConnection, string MBSN);

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
        public string IECVER;
        public string CPU;
    }

    //[Serializable]
    //public class NextStation
    //{
    //    public string Station;
    //    public string Description;        
    //}
}
